using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public Camera cam;

    bool audioExist;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StickMove();
        ComMove();

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
            Debug.Log("양발 다 앞으로 누르면 전진");
        }
        //양발 다 뒤로 누르면 후진
        else if (LFVertical < 0 && RFVertical < 0)
        {
            Vector3 _lookdir = (LFVertical + RFVertical) * transform.forward;
            transform.position += _lookdir * speed * Time.deltaTime;
            Debug.Log("양발 다 뒤로 누르면 후진");
        }
        //오른발 전진, 왼발 가만히 있으면 왼발을 중심으로 왼쪽 회전
        else if (LFVertical == 0 && RFVertical > 0)
        {
            Vector3 RF = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            Vector3 LF = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
            transform.RotateAround(LF, Vector3.up, -1 * RFVertical);
            Debug.Log("오른발 전진, 왼발 가만히 있으면 왼발을 중심으로 왼쪽 회전");

        }
        //오른발 후진, 왼발 가만히 있으면 왼발을 중심으로 오른쪽 회전
        else if (LFVertical == 0 && RFVertical < 0)
        {
            Vector3 RF = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            Vector3 LF = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

            transform.RotateAround(LF, Vector3.up, -1 * RFVertical);
            Debug.Log("오른발 후진, 왼발 가만히 있으면 왼발을 중심으로 오른쪽 회전");
        }
        //왼발 전진, 오른발 가만히 있으면 오른발 중심으로 오른쪽 회전
        else if (LFVertical > 0 && RFVertical == 0)
        {
            Vector3 RF = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            Vector3 LF = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

            transform.RotateAround(RF, Vector3.up, LFVertical);
            Debug.Log("왼발 전진, 오른발 가만히 있으면 오른발 중심으로 오른쪽 회전");
        }
        //왼발 후진, 오른발 가만히 있으면 오른발 중심으로 왼쪽 회전
        else if (LFVertical < 0 && RFVertical == 0)
        {
            Vector3 RF = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            Vector3 LF = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

            transform.RotateAround(RF, Vector3.up, LFVertical);
            Debug.Log("왼발 후진, 오른발 가만히 있으면 오른발 중심으로 왼쪽 회전");
        }
        //왼발 전진, 오른발 후진이면 제자리 오른쪽 회전
        else if (LFVertical > 0 && RFVertical < 0)
        {
            transform.RotateAround(transform.position, Vector3.up, (Mathf.Abs(RFVertical) + LFVertical));
            Debug.Log("왼발 전진, 오른발 후진이면 제자리 오른쪽 회전");
        }
        //왼발 후진, 오른발 전진이면 제자리 왼쪽 회전
        else if (LFVertical < 0 && RFVertical > 0)
        {
            transform.RotateAround(transform.position, Vector3.up, -1 * (Mathf.Abs(LFVertical) + RFVertical));
            Debug.Log("왼발 후진, 오른발 전진이면 제자리 왼쪽 회전");
        }
        //왼발은 왼쪽, 오른발은 오른쪽으로 이동하면 제자리 점프
        else if (LFHorizontal < 0 && RFHorizontal > 0)
        {
            //점프
        }
    }

    void ComMove()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        
        Vector3 dir = v * transform.forward;

        transform.position += dir * speed * Time.deltaTime;
        transform.Rotate( new Vector3(0, 2*h, 0));

        audioExist = gameObject.GetComponent<AudioSource>();

        /*if (h != 0 || v != 0 && audioExist==true)
        {
            gameObject.GetComponent<AudioSource>().enabled = true;
        }
        else if (audioExist==false)
            gameObject.GetComponent<AudioSource>().enabled = false;*/
    }
}
