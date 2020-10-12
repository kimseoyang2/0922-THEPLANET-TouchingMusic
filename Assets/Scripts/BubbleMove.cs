using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMove : MonoBehaviour
{
    public float range = 30.0f;
    public float moveSpeed = 3.0f;

    public float bounceRange = 0.3f;

    RippleDeformer rd;
    
    Vector3 dir;

    float currentBounce = 0;
    bool increase = true;

    // Start is called before the first frame update
    void Start()
    {
        // 1. 각도 정하기
        float angle = Random.Range(range * -1, range) + 270.0f;

        // 2. 아래 방향 벡터를 기준으로 해당 각도의 벡터를 구한다.
        dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

        // 3. 디포머 캐싱하기
        rd = GetComponent<RippleDeformer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * moveSpeed * Time.deltaTime;
        BounceObject();
    }

    void BounceObject()
    {
        if(increase)
        {
            if(currentBounce < bounceRange)
            {
                currentBounce += 0.01f;
                rd.Frequency = currentBounce;
            }
            else
            {
                increase = !increase;
            }   
        }
        else
        {
            if(currentBounce > bounceRange * -1.0f)
            {
                currentBounce -= 0.01f;
                rd.Frequency = currentBounce;
            }
            else
            {
                increase = !increase;
            }   
        }
        
        
    }
}
