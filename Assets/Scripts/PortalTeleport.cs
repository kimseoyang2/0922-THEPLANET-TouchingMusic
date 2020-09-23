using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public GameObject teleSpot;
    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = teleSpot.transform.position;
    }
}
