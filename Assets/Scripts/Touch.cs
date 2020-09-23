using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    public GameObject particleobj;
    //touch particle
    private ParticleSystem ps;
    Vector3 originPos;
    private void Start()
    {
        ps = particleobj.GetComponent<ParticleSystem>();
        ps.Stop();
        originPos = transform.localPosition;
    }
    private void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("touch player");
        gameObject.GetComponent<AudioSource>().Play();
        if (ps == null)
        {
            print("지정된 파티클이 없어요!");
        }
        else
        {
            ps.Play();
            print("충돌! 파티클 플레이!");
        }
            

    }
    public IEnumerator Shake(float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + originPos;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originPos;

    }
}
