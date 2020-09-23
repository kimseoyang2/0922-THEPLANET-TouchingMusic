#if UNITY_EDITOR_WIN && UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

using UnityEngine;
using UnityEditor;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

public class OVRBundleManager
{
	public const string TRANSITION_APK_VERSION_NAME = "OVRTransitionAPKVersion";
	private const int BUNDLE_CHUNK_SIZE = 30;
	private const string TRANSITION_SCENE_RELATIVE_PATH = "Scenes/OVRTransitionScene.unity";
	private const string BUNDLE_MANAGER_OUTPUT_PATH = "OVRAssetBundles";
	private const string BUNDLE_MANAGER_MASTER_BUNDLE = "OVRMasterBundle";

	private const string EXTERNAL_STORAGE_PATH = "/sdcard/Android/data";
	private const string ADB_TOOL_INITIALIZE_ERROR = "Failed to initialize ADB Tool. Please check Android SDK path in Preferences -> External Tools";

	private static string externalSceneCache;
	private static string transitionScenePath;

	private static string projectDefaultAppIdentifier;
	private static string projectDefaultVersion;
	private static ScriptingImplementation projectScriptImplementation;
#if UNITY_2018_3_OR_NEWER
	private static ManagedStrippingLevel projectManagedStrippingLevel;
#else
	private static StrippingLevel projectStrippingLevel;
#endif
	private static bool projectStripEngineCode;

	public static void BuildDeployTransitionAPK(bool useOptionalTransitionApkPackage)
	{
		OVRBundleTool.PrintLog("Building and deploying transition APK  . . . ", true);

		if (!Directory.Exists(BUNDLE_MANAGER_OUTPUT_PATH))
		{
			Directory.CreateDirectory(BUNDLE_MANAGER_OUTPUT_PATH);
		}

		PrebuildProjectSettingUpdate();

		if (String.IsNullOrEmpty(transitionScenePath))
		{
			// Get current editor script's directory as base path
			string[] res = System.IO.Directory.GetFiles(Application.dataPath, "OVRBundleManager.cs", SearchOption.AllDirectories);
			if (res.Length > 1)
			{
				OVRBundleTool.PrintError("More than one OVRBundleManager editor script has been found, please double check your Oculus SDK import.");
				return;
			}
			else
			{
				// Append Transition Scene's relative path to base path
				var OVREditorScript = Path.GetDirectoryName(res[0]);
				transitionScenePath = Path.Combine(OVREditorScript, TRANSITION_SCENE_RELATIVE_PATH);
			}
		}

		string[] buildScenes = new string[1] { transitionScenePath };
		string apkOutputPath = Path.Combine(BUNDLE_MANAGER_OUTPUT_PATH, "OVRTransition.apk");
		DateTime apkBuildStart = DateTime.Now;

#if UNITY_2018_1_OR_NEWER
		var buildPlayerOptions = new BuildPlayerOptions
		{
			scenes = buildScenes,
			locationPathName = apkOutputPath,
			target = BuildTarget.Android,
			options = BuildOptions.Development |
				BuildOptions.AutoRunPlayer
		};

		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

		if (report.summary.result == BuildResult.Succeeded)
		{
			OVRBundleTool.PrintSuccess();
		}
		else if (report.summary.result == BuildResult.Failed)
		{
			OVRBundleTool.PrintError();
		}
#else
		string error = BuildPipeline.BuildPlayer(buildScenes, apkOutputPath, BuildTarget.Android,
			BuildOptions.Development | BuildOptions.AutoRunPlayer);

		if (string.IsNullOrEmpty(error))
		{
			OVRBundleTool.PrintSuccess();
		}
		else
		{
			OVRBundleTool.PrintError();
		}
#endif
		OVRPlugin.SendEvent("oculus_bundle_tool", "apk_build_time", (DateTime.Now - apkBuildStart).TotalSeconds.ToString());
		PostbuildProjectSettingUpdate();
	}

	private static void PrebuildProjectSettingUpdate()
	{
		// Modify application identifier for transition APK
		projectDefaultAppIdentifier = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
		PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, 
			projectDefaultAppIdentifier + GetTransitionApkOptionalIdentifier());

		// Set VersionCode as a unique identifier for transition APK
		projectDefaultVersion = PlayerSettings.bundleVersion;
		PlayerSettings.bundleVersion = TRANSITION_APK_VERSION_NAME;

		// Modify IL2CPP option as it strips script symbols that are necessary for the scenes at runtime
		projectScriptImplementation = PlayerSettings.GetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup);
		if (projectScriptImplementation != ScriptingImplementation.Mono2x)
		{
			// Show message in console to make it more clear to developers
			OVRBundleTool.PrintLog("Build will use Mono as scripting backend.");
			PlayerSettings.SetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup, ScriptingImplementation.Mono2x);
		}

		// Avoid stripping managed code that are necessary for the scenes at runtime
#if UNITY_2018_3_OR_NEWER
		projectManagedStrippingLevel = PlayerSettings.GetManagedStrippingLevel(BuildTargetGroup.Android);
		if (projectManagedStrippingLevel != ManagedStrippingLevel.Disabled)
		{
			OVRBundleTool.PrintLog("Build will set Managed Stripping Level to Disabled.");
			PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Disabled);
		}
#else
		projectStrippingLevel = PlayerSettings.strippingLevel;
		if (projectStrippingLevel != StrippingLevel.Disabled)
		{
			OVRBundleTool.PrintLog("Build will set Stripping Level to Disabled.");
			PlayerSettings.strippingLevel = StrippingLevel.Disabled;
		}
#endif

		projectStripEngineCode = PlayerSettings.stripEngineCode;
		if (projectStripEngineCode)
		{
			PlayerSettings.stripEngineCode = false;
		}
	}

	private static void PostbuildProjectSettingUpdate()
	{
		// Restore application identifier
		PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android,
			projectDefaultAppIdentifier);

		// Restore version setting
		PlayerSettings.bundleVersion = projectDefaultVersion;

		// Restore scripting backend option
		if (PlayerSettings.GetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup) != projectScriptImplementation)
		{
			PlayerSettings.SetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup, projectScriptImplementation);
		}

		// Restore managed stripping level
#if UNITY_2018_3_OR_NEWER
		if (PlayerSettings.GetManagedStrippingLevel(BuildTargetGroup.Android) != projectManagedStrippingLevel)
		{
			PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, projectManagedStrippingLevel);
		}
#else
		if (PlayerSettings.strippingLevel != projectStrippingLevel)
		{
			PlayerSettings.strippingLevel = projectStrippingLevel;
		}
#endif

		if (PlayerSettings.stripEngineCode != projectStripEngineCode)
		{
			PlayerSettings.stripEngineCode = projectStripEngineCode;
		}
	}

	// Build and deploy a list of scenes. It's suggested to only build and deploy one active scene that's being modified and
	// its dependencies such as scenes that are loaded additively
	public static void BuildDeployScenes(List<OVRBundleTool.EditorSceneInfo> sceneList, bool forceRestart)
	{
		externalSceneCache = EXTERNAL_STORAGE_PATH + "/" + PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android)
			+ GetTransitionApkOptionalIdentifier() + "/cache/scenes";

		BuildSceneBundles(sceneList);
		if (DeploySceneBundles(sceneList))
		{
			if (forceRestart)
			{
				LaunchApplication();
				return;
			}
			OVRBundleTool.PrintSuccess();
			return;
		}
	}

	// Build assets that are used by scenes from the given list.
	// This function will automatically de-duplicated same asset file being referenced in multiple scenes.
	// Scene bundles will be created with name pattern: <SceneName>_<AssetTypeExtention><ChunckNumber>
	private static void BuildSceneBundles(List<OVRBundleTool.EditorSceneInfo> sceneList)
	{
		DateTime totalStart = DateTime.Now;
		// Keeps track of dependent assets across scenes 
		// to ensure each asset is only packaged once in one of the scene bundles.
		// uniqueAssetInSceneBundle is a map from "asset unique identifier" to the first scene that references the asset.
		// It supports different assets with same file name as "asset unique identifier" contain full qualified asset file path
		Dictionary<string, string> uniqueAssetInSceneBundle = new Dictionary<string, string>();

		// List of bundle targets for Unity's build pipeline to package
		List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
		// Map that contains "asset type" to list of assets that are of the asset type
		Dictionary<string, List<string>> extToAssetList = new Dictionary<string, List<string>>();

		// Check if assets in Resources need to be packaged
		if (CheckForResources())
		{
			var resourceDirectories = Directory.GetDirectories("Assets", "Resources", SearchOption.AllDirectories).ToArray();
			// Fetch a list of all files in resource directory
			string[] resourceAssetPaths = AssetDatabase.FindAssets("", resourceDirectories).Select(x => AssetDatabase.GUIDToAssetPath(x)).ToArray();
			ProcessAssets(resourceAssetPaths, "resources", ref uniqueAssetInSceneBundle, ref extToAssetList);

			AssetBundleBuild resourceBundle = new AssetBundleBuild();
			resourceBundle.assetNames = uniqueAssetInSceneBundle.Keys.ToArray();
			resourceBundle.assetBundleName = OVRSceneLoader.resourceBundleName;
			assetBundleBuilds.Add(resourceBundle);
		}

		// Create scene bundle output directory
		string sceneBundleDirectory = Path.Combine(BUNDLE_MANAGER_OUTPUT_PATH, BUNDLE_MANAGER_MASTER_BUNDLE);
		if (!Directory.Exists(sceneBundleDirectory))
		{
			Directory.CreateDirectory(sceneBundleDirectory);
		}

		OVRBundleTool.PrintLog("Building scene bundles . . . ");
		DateTime labelingStart = DateTime.Now;
		foreach (var scene in sceneList)
		{
			// Get all the assets that the scene depends on and sort them by type
			DateTime getDepStart = DateTime.Now;
			string[] assetDependencies = AssetDatabase.GetDependencies(scene.scenePath);
			Debug.Log("[OVRBundleManager] - " + scene.sceneName + " - Calculated scene asset dependencies in: " + (DateTime.Now - getDepStart).TotalSeconds);
			ProcessAssets(assetDependencies, scene.sceneName, ref uniqueAssetInSceneBundle, ref extToAssetList);

			// Add the scene into it's own bundle
			string[] sceneAsset = new string[1] { scene.scenePath };
			AssetBundleBuild sceneBuild = new AssetBundleBuild();
			sceneBuild.assetBundleName = "scene_" + scene.sceneName;
			sceneBuild.assetNames = sceneAsset;
			assetBundleBuilds.Add(sceneBuild);
		}

		// Chunk the asset bundles by number of assets
		foreach (string ext in extToAssetList.Keys)
		{
			int assetCount = extToAssetList[ext].Count;
			int numChunks = (assetCount + BUNDLE_CHUNK_SIZE - 1) / BUNDLE_CHUNK_SIZE;
			//Debug.Log(ext + " has " + assetCount + " asset(s) that will be split into " + numChunks + " chunk(s)");
			for (int i = 0; i < numChunks; i++)
			{
				List<string> assetChunkList;
				if (i == numChunks - 1)
				{
					int size = BUNDLE_CHUNK_SIZE - (numChunks * BUNDLE_CHUNK_SIZE - assetCount);
					assetChunkList = extToAssetList[ext].GetRange(i * BUNDLE_CHUNK_SIZE, size);
				}
				else
				{
					assetChunkList = extToAssetList[ext].GetRange(i * BUNDLE_CHUNK_SIZE, BUNDLE_CHUNK_SIZE);
				}
				AssetBundleBuild build = new AssetBundleBuild();
				build.assetBundleName = "asset_" + ext + i;
				build.assetNames = assetChunkList.ToArray();
				//Debug.Log("Chunk " + i + " has " + assetChunkList.Count + " asset(s)");
				assetBundleBuilds.Add(build);
			}
		}

		Debug.Log("[OVRBundleManager] - Created chucked scene bundles in: " + (DateTime.Now - labelingStart).TotalSeconds);

		// Build asset bundles
		BuildPipeline.BuildAssetBundles(sceneBundleDirectory, assetBundleBuilds.ToArray(),
				BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.Android);

		double bundleBuildTime = (DateTime.Now - totalStart).TotalSeconds;
		Debug.Log("[OVRBundleManager] - Total Time: " + bundleBuildTime);
		OVRPlugin.SendEvent("oculus_bundle_tool", "bundle_build_time", bundleBuildTime.ToString());
	}

	private static void ProcessAssets(string[] assetPaths, 
		string assetParent,
		ref Dictionary<string, string> uniqueAssetInSceneBundle, 
		ref Dictionary<string, List<string>> extToAssetList)
	{
		foreach (string asset in assetPaths)
		{
			string ext = Path.GetExtension(asset);
			if (!string.IsNullOrEmpty(ext))
			{
				ext = ext.Substring(1);
				if (!ext.Equals("cs") && !ext.Equals("unity"))
				{
					// Only process each asset once across all resource and scene bundles
					// Each asset is keyed by full path as a unique identifier
					if (!uniqueAssetInSceneBundle.ContainsKey(asset))
					{
						var assetObject = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath(asset, typeof(UnityEngine.Object));
						if (assetObject == null || (assetObject.hideFlags & HideFlags.DontSaveInBuild) == 0)
						{
							uniqueAssetInSceneBundle[asset] = assetParent;

							if (assetParent != "resources")
							{
								if (!extToAssetList.ContainsKey(ext))
								{
									extToAssetList[ext] = new List<string>();
								}
								extToAssetList[ext].Add(asset);
							}
						}
					}
				}
			}
		}
	}

	private static bool DeploySceneBundles(List<OVRBundleTool.EditorSceneInfo> sceneList)
	{
		// Create Temp directory on local machine if it does not exist
		string tempDirectory = Path.Combine(BUNDLE_MANAGER_OUTPUT_PATH, "Temp");
		if (!Directory.Exists(tempDirectory))
		{
			Directory.CreateDirectory(tempDirectory);
		}
		string absoluteTempPath = Path.Combine(Path.Combine(Application.dataPath, ".."), tempDirectory);

		OVRBundleTool.PrintLog("Deploying scene bundles to device . . . ");

		OVRADBTool adbTool = new OVRADBTool(OVRConfig.Instance.GetAndroidSDKPath());
		if (adbTool.isReady)
		{
			DateTime transferStart = DateTime.Now;

			OVRBundleTool.UpdateSceneBuildStatus(OVRBundleTool.SceneBundleStatus.TRANSFERRING);
			// Transfer all scene bundles that are relavent
			if (!TransferSceneBundles(adbTool, absoluteTempPath, externalSceneCache))
			{
				return false;
			}
			OVRBundleTool.UpdateSceneBuildStatus(OVRBundleTool.SceneBundleStatus.DEPLOYED);

			// Create file to tell transition scene APK which scene to load and push it to the device
			string sceneLoadDataPath = Path.Combine(tempDirectory, OVRSceneLoader.sceneLoadDataName);
			if (File.Exists(sceneLoadDataPath))
			{
				File.Delete(sceneLoadDataPath);
			}

			StreamWriter writer = new StreamWriter(sceneLoadDataPath, true);
			// Write version and scene names
			long unixTime = (int)(DateTimeOffset.UtcNow.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
			writer.WriteLine(unixTime.ToString());
			for (int i = 0; i < sceneList.Count; i++)
			{
				writer.WriteLine(Path.GetFileNameWithoutExtension(sceneList[i].scenePath));
			}

			writer.Close();

			string absoluteSceneLoadDataPath = Path.Combine(absoluteTempPath, OVRSceneLoader.sceneLoadDataName);
			string[] pushCommand = { "-d push", "\"" + absoluteSceneLoadDataPath + "\"", "\"" + externalSceneCache + "\"" };
			string output, error;
			if (adbTool.RunCommand(pushCommand, null, out output, out error) == 0)
			{
				Debug.Log("[OVRBundleManager] Scene Load Data Pushed to Device.");
				OVRPlugin.SendEvent("oculus_bundle_tool", "transfer_bundle_time", (DateTime.Now - transferStart).TotalSeconds.ToString());
				return true;
			}
			OVRBundleTool.PrintError(string.IsNullOrEmpty(error) ? output : error);
		}
		else
		{
			OVRBundleTool.PrintError(ADB_TOOL_INITIALIZE_ERROR);
		}
		return false;
	}

	private static bool TransferSceneBundles(OVRADBTool adbTool, string absoluteTempPath, string externalSceneCache)
	{
		List<string> bundlesToTransfer = new List<string>();
		List<string> bundlesToDelete = new List<string>();
		string manifestFilePath = externalSceneCache + "/" + BUNDLE_MANAGER_MASTER_BUNDLE;

		string[] pullManifestCommand = { "-d pull", "\"" + manifestFilePath + "\"", "\"" + absoluteTempPath + "\"" };

		string output, error;
		if (adbTool.RunCommand(pullManifestCommand, null, out output, out error) == 0)
		{
			// An existing manifest file was found on device. Load hashes and upload bundles that have changed hashes.
			Debug.Log("[OVRBundleManager] - Scene bundle manifest file found. Decoding changes . . .");

			// Load hashes from remote manifest
			AssetBundle remoteBundle = AssetBundle.LoadFromFile(Path.Combine(absoluteTempPath, BUNDLE_MANAGER_MASTER_BUNDLE));
			if (remoteBundle == null)
			{
				OVRBundleTool.PrintError("Failed to load remote asset bundle manifest file.");
				return false;
			}
			AssetBundleManifest remoteManifest = remoteBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

			Dictionary<string, Hash128> remoteBundleToHash = new Dictionary<string, Hash128>();
			if (remoteManifest != null)
			{
				string[] assetBundles = remoteManifest.GetAllAssetBundles();
				foreach (string bundleName in assetBundles)
				{
					remoteBundleToHash[bundleName] = remoteManifest.GetAssetBundleHash(bundleName);
				}
			}
			remoteBundle.Unload(true);

			// Load hashes from local manifest
			AssetBundle localBundle = AssetBundle.LoadFromFile(BUNDLE_MANAGER_OUTPUT_PATH + "\\" + BUNDLE_MANAGER_MASTER_BUNDLE
					+ "\\" + BUNDLE_MANAGER_MASTER_BUNDLE);
			if (localBundle == null)
			{
				OVRBundleTool.PrintError("<color=red>Failed to load local asset bundle manifest file.\n</color>");
				return false;
			}
			AssetBundleManifest localManifest = localBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

			if (localManifest != null)
			{
				Hash128 zeroHash = new Hash128(0, 0, 0, 0);

				// Build a list of dirty bundles that will have to be transfered
				string relativeSceneBundlesPath = Path.Combine(BUNDLE_MANAGER_OUTPUT_PATH, BUNDLE_MANAGER_MASTER_BUNDLE);
				bundlesToTransfer.Add(Path.Combine(relativeSceneBundlesPath, BUNDLE_MANAGER_MASTER_BUNDLE));
				string[] assetBundles = localManifest.GetAllAssetBundles();
				foreach (string bundleName in assetBundles)
				{
					if (!remoteBundleToHash.ContainsKey(bundleName))
					{
						bundlesToTransfer.Add(Path.Combine(relativeSceneBundlesPath, bundleName));
					}
					else
					{
						if (remoteBundleToHash[bundleName] != localManifest.GetAssetBundleHash(bundleName))
						{
							bundlesToTransfer.Add(Path.Combine(relativeSceneBundlesPath, bundleName));
						}
						remoteBundleToHash[bundleName] = zeroHash;
					}
				}

				OVRBundleTool.PrintLog(bundlesToTransfer.Count + " dirty bundle(s) will be transfered.\n");
			}
		}
		else
		{
			if (output.Contains("does not exist"))
			{
				// Fresh install of asset bundles, transfer all asset bundles
				OVRBundleTool.PrintLog("Manifest file not found. Transfering all bundles . . . ");

				string[] mkdirCommand = { "-d shell", "mkdir -p", "\"" + externalSceneCache + "\"" };
				if (adbTool.RunCommand(mkdirCommand, null, out output, out error) == 0)
				{
					string absoluteSceneBundlePath = Path.Combine(Path.Combine(Application.dataPath, ".."),
							Path.Combine(BUNDLE_MANAGER_OUTPUT_PATH, BUNDLE_MANAGER_MASTER_BUNDLE));

					string[] assetBundlePaths = Directory.GetFiles(absoluteSceneBundlePath);
					if (assetBundlePaths.Length != 0)
					{
						foreach (string path in assetBundlePaths)
						{
							if (!path.Contains(".manifest"))
							{
								bundlesToTransfer.Add(path);
							}
						}
					}
					else
					{
						OVRBundleTool.PrintError("Failed to locate scene bundles to transfer.");
						return false;
					}
				}
			}
		}

		// If any adb error occured during manifest calculation, print it and return false
		if (!string.IsNullOrEmpty(error) || output.Contains("error"))
		{
			OVRBundleTool.PrintError(string.IsNullOrEmpty(error) ? output : error);
			return false;
		}

		// Transfer bundles to device
		DateTime transferStart = DateTime.Now;
		foreach (string bundle in bundlesToTransfer)
		{
			string absoluteBundlePath = Path.Combine(Path.Combine(Application.dataPath, ".."), bundle);
			string[] pushBundleCommand = { "-d push", "\"" + absoluteBundlePath + "\"", "\"" + externalSceneCache + "\"" };
			adbTool.RunCommandAsync(pushBundleCommand, null);
		}
		Debug.Log("[OVRBundleManager] - Transfer took " + (DateTime.Now - transferStart).TotalSeconds + " seconds.");

		// Delete stale bundles on device
		if (bundlesToDelete.Count > 0)
		{
			foreach (string bundle in bundlesToDelete)
			{
				string bundlePath = externalSceneCache + "/" + bundle;
				string[] deleteBundleCommand = { "-d shell", "rm", "\"" + bundlePath + "\"" };
				adbTool.RunCommandAsync(deleteBundleCommand, null);
			}
			OVRBundleTool.PrintLog("Deleted " + bundlesToDelete.Count + " bundle(s) that were stale");
		}

		return true;
	}

	public static bool LaunchApplication()
	{
		OVRBundleTool.PrintLog("Launching Application . . . ");

		OVRADBTool adbTool = new OVRADBTool(OVRConfig.Instance.GetAndroidSDKPath());
		if (adbTool.isReady)
		{
			string output, error;
			string appPackagename = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android)
				+ GetTransitionApkOptionalIdentifier();
			string playerActivityName = "\"" + appPackagename + "/com.unity3d.player.UnityPlayerActivity\"";
			string[] appStartCommand = { "-d shell", "am start -a android.intent.action.MAIN -c android.intent.category.LAUNCHER -S -f 0x10200000 -n", playerActivityName };
			if (adbTool.RunCommand(appStartCommand, null, out output, out error) == 0)
			{
				OVRBundleTool.PrintSuccess();
				OVRBundleTool.PrintLog("App package " + appPackagename + " is launched.");
				return true;
			}

			string completeError = "Failed to launch application. Try launching it manually through the device.\n" + (string.IsNullOrEmpty(error) ? output : error);
			OVRBundleTool.PrintError(completeError);
		}
		else
		{
			OVRBundleTool.PrintError(ADB_TOOL_INITIALIZE_ERROR);
		}
		return false;
	}

	public static bool UninstallAPK()
	{
		OVRBundleTool.PrintLog("Uninstalling Application . . .");

		OVRADBTool adbTool = new OVRADBTool(OVRConfig.Instance.GetAndroidSDKPath());
		if (adbTool.isReady)
		{
			string output, error;
			string appPackagename = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android)
				+ GetTransitionApkOptionalIdentifier();
			string[] appStartCommand = { "-d shell", "pm uninstall", appPackagename };
			if (adbTool.RunCommand(appStartCommand, null, out output, out error) == 0)
			{
				OVRBundleTool.PrintSuccess();
				OVRBundleTool.PrintLog("App package " + appPackagename + " is uninstalled.");
				return true;
			}

			OVRBundleTool.PrintError("Failed to uninstall APK.");
		}
		else
		{
			OVRBundleTool.PrintError(ADB_TOOL_INITIALIZE_ERROR);
		}
		return false;
	}

	public static void DeleteRemoteAssetBundles()
	{
		OVRADBTool adbTool = new OVRADBTool(OVRConfig.Instance.GetAndroidSDKPath());
		if (adbTool.isReady)
		{
			bool failure = false;
			string fileExistsError = "No such file or directory";
			OVRBundleTool.PrintLog("Deleting device bundles . . . ");
			string output, error;
			string[] deleteBundleCommand = { "-d shell", "rm -r", externalSceneCache };
			if (adbTool.RunCommand(deleteBundleCommand, null, out output, out error) != 0)
			{
				if (!(output.Contains(fileExistsError) || error.Contains(fileExistsError)))
				{
					failure = true;
				}
			}

			if (failure)
			{
				OVRBundleTool.PrintError(string.IsNullOrEmpty(error) ? output : error);
				OVRBundleTool.PrintError("Failed to delete scene bundle cache directory.");
			}
			else
			{
				OVRBundleTool.PrintSuccess();
			}
		}
		else
		{
			OVRBundleTool.PrintError(ADB_TOOL_INITIALIZE_ERROR);
		}
	}

	public static void DeleteLocalAssetBundles()
	{
		try
		{
			if (Directory.Exists(BUNDLE_MANAGER_OUTPUT_PATH))
			{
				OVRBundleTool.PrintLog("Deleting local bundles . . . ");
				Directory.Delete(BUNDLE_MANAGER_OUTPUT_PATH, true);
			}
		}
		catch (Exception e)
		{
			OVRBundleTool.PrintError(e.Message);
		}
		OVRBundleTool.PrintSuccess();
	}

	private static bool CheckForResources()
	{
		string[] resourceDirectories = Directory.GetDirectories("Assets", "Resources", SearchOption.AllDirectories);
		return resourceDirectories.Length > 0;
	}

	private static string GetTransitionApkOptionalIdentifier()
	{
		string transitionApkOptionalIdentifier;
		// Check option value from editor UI
		if (OVRBundleTool.GetUseOptionalTransitionApkPackage())
		{
			// Append .transition to default app package name to optionally allow both
			// full build apk and transition apk to be installed on device 
			transitionApkOptionalIdentifier = ".transition";
		}
		else
		{
			transitionApkOptionalIdentifier = "";
		}
		return transitionApkOptionalIdentifier;
	}
}
#endif
