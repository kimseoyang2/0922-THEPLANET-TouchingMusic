using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetAtive : MonoBehaviour
{
    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //시간이 계속 흐른당
        time += Time.deltaTime;
        //만약 일정 시간이 지나면 (20초) UI가 켜지도록 한다
        if(time >= 5f)
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
        
    }
}
