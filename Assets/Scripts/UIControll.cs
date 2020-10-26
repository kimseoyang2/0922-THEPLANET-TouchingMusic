using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControll : MonoBehaviour
{
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
        //만약 부딪힌게 UIzone 이라면
        if (other.CompareTag("UI"))
        {
            Debug.Log("UITouch");
            //UI를 키세요
            other.transform.GetChild(0).gameObject.SetActive(true);

        };
        

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UI"))
        {
            Debug.Log("UITouch");
            //UI를 키세요
            other.transform.GetChild(0).gameObject.SetActive(false);

        };
    }
}
