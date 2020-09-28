using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayParticle : MonoBehaviour
{
    public GameObject spawnParticle;

    // Start is called before the first frame update
    void Start()
    {






    }


    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("PLAYER"))
        {
            
            spawnParticle.SetActive(true);
            Invoke("lifeTime", 3f);



        }






    }

    void lifeTime()
    {
        spawnParticle.SetActive(false);
    }

}