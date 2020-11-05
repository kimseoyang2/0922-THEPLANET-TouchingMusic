using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dissolve : MonoBehaviour
{
    public int gameLevel;

    public Animator transition;
    public float transitionTime = 1f;

    public GameObject sphere;
    public GameObject loader;
    //public GameObject levelUI;
    //public GameObject startUI;
    // Update is called once per frame

    private void Awake()
    {

    }
    void Update()
    {
        //LoadScene();
    }

    public void SelectLv()
    {
        gameObject.GetComponent<AudioSource>().Play();
        //levelUI.SetActive(true);
        //startUI.SetActive(false);
    }



    public void StartGame()
    {
        sphere.SetActive(true);
        if (InverseNormalBlack.getWhite == true)
        {
            Debug.Log("하얗다");
            LoadingSceneController.LoadScene("DesertMain_Fox2");
            loader.SetActive(true);
        }
    }

    public void Tutorial()
    {
        sphere.SetActive(true);
        if (InverseNormalBlack.getWhite == true)
        {
            LoadingSceneController.LoadScene("Tutorial");
            loader.SetActive(true);
        }
    }

    /*IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    */
}