using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dissolve : MonoBehaviour
{
    public static int gameLevel;

    public Animator transition;
    public float transitionTime = 1f;
    //public GameObject levelUI;
    //public GameObject startUI;
    // Update is called once per frame

    private void Awake()
    {

    }
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    LoadNextLevel();
        //}
        //LoadNextLevel();
    }

    public void SelectLv()
    {
        gameObject.GetComponent<AudioSource>().Play();
        //levelUI.SetActive(true);
        //startUI.SetActive(false);
    }

    public void LoadScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

}