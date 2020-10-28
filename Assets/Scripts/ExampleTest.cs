using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string[] names = Input.GetJoystickNames();

        for(int i = 0; i < names.Length; i++)
        {
            if (names[i].Length > 0) print("joystick : " + names[i]);
        }

        string str = Input.inputString;
        if(str.Length > 0)
        {
            print(str);
        }
        
        if (Input.anyKey)
            Debug.Log(Input.inputString);
    }
}
