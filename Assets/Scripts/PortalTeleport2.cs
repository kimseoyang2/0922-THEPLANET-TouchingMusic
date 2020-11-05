using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTeleport2 : MonoBehaviour
{
    public int nowSceneNum;

    private void OnTriggerEnter(Collider other)
    {

            SceneManager.LoadScene("Potal");

    }
}
