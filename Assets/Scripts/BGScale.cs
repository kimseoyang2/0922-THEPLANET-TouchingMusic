using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScale : MonoBehaviour
{
   public AudioSource audioSource;
   public float updateStep = 0.1f;
   public int sampleDatalLenght = 1024;

   private float currentUpdateTime = 0f;

   public float clipLoundness;
   private float[] clipSampleDate;

   public GameObject sprite;
   public float sizeFactor = 1;

   public float minSize = 0;
   public float maxSize = 500;

   private void Awake()
   {
        clipSampleDate = new float[sampleDatalLenght];
   }

   private void Update()
   {
       currentUpdateTime += Time.deltaTime;
       if(currentUpdateTime >= updateStep)
       {
           currentUpdateTime = 0f;
           audioSource.clip.GetData(clipSampleDate, audioSource.timeSamples);
           clipLoundness = 0f;
           foreach (var sample in clipSampleDate)
           {
               clipLoundness += Mathf.Abs(sample);
           }
           clipLoundness /= sampleDatalLenght;

           clipLoundness *= sizeFactor;
           clipLoundness = Mathf.Clamp(clipLoundness, minSize, maxSize);
           sprite.transform.localScale = new Vector3(1, clipLoundness, 1);
       }
   }
    
}
