using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));

    void Update()
    {
        print("Mouse X :" + Input.GetAxis("Mouse X"));
        //print("Mouse Y :" + Input.GetAxis("Mouse Y"));

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
