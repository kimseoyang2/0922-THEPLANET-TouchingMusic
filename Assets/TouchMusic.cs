using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMusic : MonoBehaviour
{
    public AudioClip[] SheepBoxClip;
    public AudioSource RandomSource;
    public bool isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        RandomSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRandomClip(AudioClip[] audioClips)
    {
        RandomSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        RandomSource.Play();
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("SHEEPBOX")) {
            isOn = true;
            
            Debug.Log("Touch SheepBox");
            if(RandomSource!=null)
            if (isOn)
            {
                PlayRandomClip(SheepBoxClip);
                isOn = false;
            }

            else if (!isOn)
            {

                PlayRandomClip(SheepBoxClip);
                isOn = true;
            }

            
        }
       

      



    }
}
