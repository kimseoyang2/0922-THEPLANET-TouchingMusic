using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Swing : MonoBehaviour
{
    public AudioSource swing;
    public AudioClip swingSound;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

        if (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).sqrMagnitude > 1.3)
        {
            if (!swing.isPlaying)
            {
                swing.PlayOneShot(swingSound, 0.8f);
            }
        }
    }
}
