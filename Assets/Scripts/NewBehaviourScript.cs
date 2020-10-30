using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewBehaviourScript : MonoBehaviour
{
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));

    void Update()
    {
       // print("Mouse X :" + Input.GetAxisRaw("Mouse X"));
        //print("Mouse Y :" + Input.GetAxisRaw("Mouse Y"));

        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in keyCodes)
            {
                if (Input.GetKey(keyCode))
                {
                    Debug.Log("KeyCode down: " + keyCode);
                    break;
                }
            }

        }
    }
}
