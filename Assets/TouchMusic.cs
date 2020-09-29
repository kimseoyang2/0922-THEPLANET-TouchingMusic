using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TouchMusic : MonoBehaviour
{
    public AudioClip[] RandomClip;
    public AudioSource RandomSource;
    public DOTweenAnimation dOTweenAnimation;
    public bool isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        RandomSource = gameObject.GetComponent<AudioSource>();
        dOTweenAnimation = gameObject.GetComponent<DOTweenAnimation>();
        dOTweenAnimation.DOPause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRandomClip(AudioClip[] audioClips)
    {
        print("PlayRandomClip");
        RandomSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        dOTweenAnimation.DOPlay();
        RandomSource.Play();
    }


    public void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("PLAYER")) {
            isOn = true;
            
            Debug.Log("Touch SheepBox");

            if(RandomSource == null)
            {
               print("RandomSource is null");
               return;
            }

            if (isOn)
            {
                
                PlayRandomClip(RandomClip);
                Invoke("StopMove", 5f);
                isOn = false;
            }

            else 
            {
                //dOTweenAnimation.DOPause();
                //PlayRandomClip(RandomClip);
                isOn = true;
            }

            
        }
       

      



    }

    public void StopMove()
    {
        dOTweenAnimation.DOPause();
    }
}
