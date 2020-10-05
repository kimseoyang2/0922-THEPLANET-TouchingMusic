using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    static public bool makeSound;
    //public GameObject particleobj;
    //touch particle
    //private ParticleSystem ps;
    private float soundCount;
    Vector3 originPos;
    private void Start()
    {
//<<<<<<< Updated upstream
        //ps = particleobj.GetComponent<ParticleSystem>();
        //ps.Stop();
        originPos = transform.localPosition;
    }
    private void Update()
    {
       
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("touch player");


        if (other.gameObject.CompareTag("tutorial") == true)
        {
            other.gameObject.GetComponent<AudioSource>().Play();
            soundCount++;

            if (soundCount > 2)
                makeSound = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {



        //if (ps == null)
        //{
            //print("지정된 파티클이 없어요!");
        //}
        //else
        //{
           // ps.Play();
           // print("충돌! 파티클 플레이!");
        //}
            

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
