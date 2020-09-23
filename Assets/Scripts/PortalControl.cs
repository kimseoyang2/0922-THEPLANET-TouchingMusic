using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalControl : MonoBehaviour
{
    public Transform land1, land2;
    public Transform playerRoot, playerCam;
    public Transform portalCam;

    public RenderTexture renderTexture;
    // Start is called before the first frame update
    void Start()
    {
        renderTexture.width = Screen.width;
        renderTexture.height = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerOffset = playerCam.position - land1.position;

        //portalCam.position = land2.position + playerOffset;
        portalCam.rotation = playerCam.rotation;
    }
}
