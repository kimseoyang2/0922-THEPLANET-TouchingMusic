using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTeleport : MonoBehaviour
{
    public int nowSceneNum;
    public GameObject sphere;
    public GameObject loader;
    private void OnTriggerEnter(Collider other)
    {
        sphere.SetActive(true);
        InverseNormalBlack inverseNormalBlack = sphere.GetComponent<InverseNormalBlack>();
        //inverseNormalBlack.onCompleteWhite = OnCompleteWhite;
        inverseNormalBlack.onCompleteWhite = () => {
            LoadingSceneController.LoadScene("Rose 1");
            loader.SetActive(true);
        };
    }
}
