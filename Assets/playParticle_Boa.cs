using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playParticle_Boa : MonoBehaviour
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
            this.gameObject.SetActive(false);



        }






    }

    void lifeTime()
    {
        spawnParticle.SetActive(false);
    }

}