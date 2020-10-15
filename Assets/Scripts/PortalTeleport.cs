using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTeleport : MonoBehaviour
{
    public int nowSceneNum;
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(nowSceneNum + 1);
    }
}
