using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SimplePhysicsToolkit {
	[CustomEditor(typeof(Magnet))]
	public class MagnetEditorWindow : Editor {

	    private Magnet currentScript;

	    void OnEnable(){
	        currentScript = (Magnet)target;
	    }

		public override void OnInspectorGUI(){

			currentScript.enable = EditorGUILayout.Toggle ("Enabled", currentScript.enable);
			currentScript.attract = EditorGUILayout.Toggle ("Attract", currentScript.attract);

			currentScript.magnetForce = EditorGUILayout.FloatField ("Force", currentScript.magnetForce);
			currentScript.innerRadius = EditorGUILayout.FloatField ("Inner Radius", currentScript.innerRadius);
			currentScript.outerRadius = EditorGUILayout.FloatField ("Outer Radius", currentScript.outerRadius);

			currentScript.onlyAffectInteractableItems = EditorGUILayout.Toggle ("Only Interactable Items", currentScript.onlyAffectInteractableItems);

			if (currentScript.onlyAffectInteractableItems) {
				EditorGUILayout.HelpBox("Will only affect objects with the InteractableItem.cs script attached", MessageType.Warning);
			}

			currentScript.realismMode = EditorGUILayout.Toggle ("Realism Mode", currentScript.realismMode);

			if (currentScript.realismMode) {
				EditorGUILayout.HelpBox("Force accounts for object distance as well, increasing realism of effect\n\nAttempts to obey Newton's second law. Thanks to Marc Leatham for his assistance with this.", MessageType.Info);
			}

		}
	}
}
