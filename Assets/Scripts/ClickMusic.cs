using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMusic : MonoBehaviour
{
    public GameObject objects;
    public GameObject spawnParticle;
    public GameObject controller;

    Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPos = controller.transform.position + new Vector3(0, 0, 3);
            GameObject spawned = Instantiate(objects, currentPos, Quaternion.identity);

            if (spawnParticle != null)
            {
                var particle = Instantiate(spawnParticle, currentPos, spawnParticle.transform.rotation);
                Destroy(particle, 3);
            }

        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) )
        {
            currentPos = controller.transform.position + new Vector3(0, 0, 3);
            GameObject spawned = Instantiate(objects, currentPos, Quaternion.identity) ;

            if (spawnParticle != null)
            {
                var particle = Instantiate(spawnParticle, currentPos, spawnParticle.transform.rotation);
                Destroy(particle, 3);
            }

        }
    }
}
