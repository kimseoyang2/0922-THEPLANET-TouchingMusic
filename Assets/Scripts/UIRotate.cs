using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRotate : MonoBehaviour
{
    public Camera ovrCam;
    private Transform tr;
    // Update is called once per frame
    private void Start()
    {
        //메인 카메라의 Transform 컴포넌트를 추출
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        tr.LookAt(ovrCam.transform);
        //transform.LookAt(ovrCam.)
    }
}
