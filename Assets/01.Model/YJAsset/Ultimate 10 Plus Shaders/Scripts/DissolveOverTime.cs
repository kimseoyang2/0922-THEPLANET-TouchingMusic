using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DissolveOverTime : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    public float speed = .5f;

    private void Start(){
        meshRenderer = this.GetComponent<MeshRenderer>();


    }

    private float t = 1.0f;
    private void Update(){
        Reveal();
    }

    void Reveal()
    {
        Material[] mats = meshRenderer.materials;


        mats[0].SetFloat("_Cutoff", t);

        t -= speed * Time.deltaTime;

        if (t <= 0)
            t = 0;

        // Unity does not allow meshRenderer.materials[0]...
        meshRenderer.materials = mats;
    }
}
