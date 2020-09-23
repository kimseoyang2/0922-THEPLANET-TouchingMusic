using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public GameObject particle;
    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
       ps= particle.GetComponent<ParticleSystem>();
        ps.Stop();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(ps == null)
        {
            print("지정된 파티클이 없어요!");
        }
        else
        {
            ps.Play();
            print("충돌! 파티클 플레이!");
        }
        
        
    }
}
