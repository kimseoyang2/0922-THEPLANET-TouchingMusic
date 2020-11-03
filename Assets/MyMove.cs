using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyMove : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
                Debug.Log("KeyCode down: " + kcode);
        }
    }
}
