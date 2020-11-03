using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class InverseNormalBlack : MonoBehaviour
{
    Material mat;
    Mesh mesh;

    public static bool getWhite;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        int[] triangles = mesh.triangles;
        int temp;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            temp = triangles[i];
            triangles[i] = triangles[i + 1];
            triangles[i + 1] = temp;
        }
        mesh.triangles = triangles;

        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] *= -1;
        }
        mesh.normals = normals;

        mat = GetComponent<MeshRenderer>().material;
        getWhite = false;
    }

    private void Update()
    {
        
        Color color = mat.color;
        //만약 0.5보다 숫자가 작으면 여기들어가기
        if (color.a <= 1f)
        {
            color.a += Time.deltaTime * 0.5f;
            mat.color = color;

            if (color.a >= 0.9f)
                getWhite = true;
        }
    }
}