using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerTouch : MonoBehaviour
{
    public GameObject TriggerOn;
    public bool isOn=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touch Danger");

        if (isOn)
        {
            TriggerOn.SetActive(false);
            gameObject.GetComponent<AudioSource>().Play();
            isOn = false;
        }

        else if(!isOn)
        {
            TriggerOn.SetActive(true);
            gameObject.GetComponent<AudioSource>().Play();
            isOn = true;
        }



    }
}
