using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playParticle_Boa : MonoBehaviour
{
    public GameObject spawnParticle;
    public MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {


        meshRenderer = gameObject.GetComponent<MeshRenderer>();



    }


    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("PLAYER"))
        {
            spawnParticle.SetActive(true);
            meshRenderer.enabled = false;
           
            Invoke("lifeTime", 3f);



        }






    }

    void lifeTime()
    {
        spawnParticle.SetActive(false);
        meshRenderer.enabled = true;

    }

}