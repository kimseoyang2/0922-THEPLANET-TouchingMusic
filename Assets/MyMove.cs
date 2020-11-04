using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class MyMove : MonoBehaviour
{
    public float rotSpeed = 5f;
    public float moveSpeed = 3f;
    public float pushThreshold = 0.5f;

    float prevDegree = 0;
    float moveAction = 0;

    void Start()
    {
        //controller = new Blg();
        //controller.BlgMove.Rotate.performed += ctx => peddal = ctx.ReadValue<float>();
        //controller.BlgMove.MoveForward.performed += context => PrintAction(context);
        //controller.BlgMove.MoveForward.performed += ctx => PrintAction(ctx);

        //controller.BlgMove.MoveForward.started += context => isMove = true;
        //controller.BlgMove.MoveForward.canceled += context => isMove = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        float result = context.ReadValue<float>();
        moveAction = 0;

        if (result >= pushThreshold)
        {
            moveAction = 1;
        }
        else if (result <= pushThreshold * -1)
        {
            moveAction = -1;
        }
        else
        {
            moveAction = 0;
        }


    }

    public void PeddalRotate(InputAction.CallbackContext context)
    {
        float peddal = context.ReadValue<float>();
        float delta = peddal - prevDegree;
        prevDegree = peddal;

        peddal = delta >= 0 ? -1 : 1;
        print("축 이동: " + peddal);

        transform.eulerAngles += new Vector3(0, peddal * rotSpeed, 0);
    }

    void Update()
    {
        transform.position += transform.forward * moveAction * moveSpeed * Time.deltaTime;
        print("가즈아~");


        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
                Debug.Log("KeyCode down: " + kcode);
        }


        //if(Input.GetKey(KeyCode.Joystick1Button14))
        //{
        //    print("14번 눌렀어!!!!!!!!!!");
        //}

        //if (Input.GetKey(KeyCode.Joystick1Button15))
        //{
        //    print("15번 눌렀어!!!!!!!!!!");
        //}

        //if (Input.GetKey(KeyCode.Joystick1Button7))
        //{
        //    print("7번 눌렀어!!!!!!!!!!");
        //}

        /*if (Input.GetKey(KeyCode.Joystick1Button14))
        {

        }

        else if (Input.GetKey(KeyCode.Joystick1Button7))
        {
            //transform.position += transform.forward * Time.deltaTime;
        }*/

    }
}
