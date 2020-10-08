using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMusic : MonoBehaviour
{
    public GameObject objects;
    public GameObject spawnParticle;
    public GameObject controller;

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
            GameObject spawned = Instantiate(objects, currentPos.position, Quaternion.identity);

            if (spawnParticle != null)
            {
                var particle = Instantiate(spawnParticle, currentPos.position, spawnParticle.transform.rotation);
                Destroy(particle, 3);
            }

        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) )
        {
            GameObject spawned = Instantiate(objects, currentPos.position, Quaternion.identity) ;

            if (spawnParticle != null)
            {
                var particle = Instantiate(spawnParticle, currentPos.position, spawnParticle.transform.rotation);
                Destroy(particle, 3);
            }

        }
    }
}
