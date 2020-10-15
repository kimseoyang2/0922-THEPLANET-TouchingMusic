using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMusic : MonoBehaviour
{
    public GameObject objects;
    public GameObject spawnParticle;
    public GameObject controller;

    public AudioSource noteClipSource;
    public AudioClip[] forwardNoteClips;
    public Material[] bubbleMat;

    public Transform currentPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            float[] bubbleScale = new float[] { 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f };
            float Scale = bubbleScale[Random.Range(0, bubbleScale.Length)];
            GameObject spawned = Instantiate(objects, currentPos.position, Quaternion.identity);
            spawned.transform.localScale = new Vector3(Scale, Scale, Scale);

            spawned.GetComponent<MeshRenderer>().material = bubbleMat[Random.Range(0, bubbleMat.Length)];
            PlayRandomClip(forwardNoteClips);

            if (spawnParticle != null)
            {
                var particle = Instantiate(spawnParticle, currentPos.position, spawnParticle.transform.rotation);
                Destroy(particle, 3);
            }

        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) )
        {
            float[] bubbleScale = new float[] { 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f };
            float Scale = bubbleScale[Random.Range(0, bubbleScale.Length)];
            GameObject spawned = Instantiate(objects, currentPos.position, Quaternion.identity) ;
            spawned.transform.localScale = new Vector3(Scale, Scale, Scale);
            PlayRandomClip(forwardNoteClips);

            if (spawnParticle != null)
            {
                var particle = Instantiate(spawnParticle, currentPos.position, spawnParticle.transform.rotation);
                Destroy(particle, 3);
            }

        }
    }

    public void PlayRandomClip(AudioClip[] audioClips)
    {
        noteClipSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        noteClipSource.Play();
    }
}
