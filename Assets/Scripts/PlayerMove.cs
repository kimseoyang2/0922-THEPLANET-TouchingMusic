using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //StickMove();
        //ComMove();
        BLGMove();

    }

    void StickMove()
    {
        Vector2 leftFoot = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 rightFoot = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        float LFVertical = leftFoot.y;
        float LFHorizontal = leftFoot.x;
        float RFVertical = rightFoot.y;
        float RFHorizontal = rightFoot.x;

        //양발 다 앞으로 누르면 전진
        if (LFVertical > 0 && RFVertical > 0)
        {
            Vector3 _lookdir = (LFVertical + RFVertical) * transform.forward;
            transform.position += _lookdir * speed * Time.deltaTime;
        }
        //양발 다 뒤로 누르면 후진
        else if (LFVertical < 0 && RFVertical < 0)
        {
            Vector3 _lookdir = (LFVertical + RFVertical) * transform.forward;
            transform.position += _lookdir * speed * Time.deltaTime;
        }
        //오른발 전진, 왼발 가만히 있으면 왼발을 중심으로 왼쪽 회전
        else if (LFVertical == 0 && RFVertical > 0)
        {
            Vector3 RF = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            Vector3 LF = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
            transform.RotateAround(LF, Vector3.up, -1 * RFVertical);
        }
        //오른발 후진, 왼발 가만히 있으면 왼발을 중심으로 오른쪽 회전
        else if (LFVertical == 0 && RFVertical < 0)
        {
            Vector3 RF = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            Vector3 LF = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

            transform.RotateAround(LF, Vector3.up, -1 * RFVertical);
        }
        //왼발 전진, 오른발 가만히 있으면 오른발 중심으로 오른쪽 회전
        else if (LFVertical > 0 && RFVertical == 0)
        {
            Vector3 RF = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            Vector3 LF = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

            transform.RotateAround(RF, Vector3.up, LFVertical);
        }
        //왼발 후진, 오른발 가만히 있으면 오른발 중심으로 왼쪽 회전
        else if (LFVertical < 0 && RFVertical == 0)
        {
            Vector3 RF = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            Vector3 LF = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

            transform.RotateAround(RF, Vector3.up, LFVertical);
        }
        //왼발 전진, 오른발 후진이면 제자리 오른쪽 회전
        else if (LFVertical > 0 && RFVertical < 0)
        {
            transform.RotateAround(transform.position, Vector3.up, (Mathf.Abs(RFVertical) + LFVertical));
        }
        //왼발 후진, 오른발 전진이면 제자리 왼쪽 회전
        else if (LFVertical < 0 && RFVertical > 0)
        {
            transform.RotateAround(transform.position, Vector3.up, -1 * (Mathf.Abs(LFVertical) + RFVertical));
        }
        //왼발은 왼쪽, 오른발은 오른쪽으로 이동하면 제자리 점프
        else if (LFHorizontal < 0 && RFHorizontal > 0)
        {
            //점프
        }
    }

    void ComMove()
    {
        float a = Input.GetAxis("JoystickX");
        if(a != 0)
        {
            print("JoystickX : " + a);
        }

        a = Input.GetAxis("JoystickY");
        if (a != 0)
        {
            print("JoystickY: " + a);
        }

       // return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = v * transform.forward;

        transform.position += dir * speed * Time.deltaTime;
        transform.Rotate(new Vector3(0, 2 * h, 0));

    }

    void BLGMove()
    {
        float a = Input.GetAxis("JoystickX");
        if (a != 0)
        {
            print("JoystickX : " + a);
        }

        a = Input.GetAxis("JoystickY");
        if (a != 0)
        {
            print("JoystickY: " + a);
        }

            float Blgforword = Input.GetAxis("JoystickX");
            float Blgrotate = Input.GetAxis("JoystickY");

            Vector3 dir = Blgforword * transform.forward;
            transform.position += dir * speed * Time.deltaTime;
       
            transform.Rotate(new Vector3(0, 2 * Blgrotate, 0));


    }
}
