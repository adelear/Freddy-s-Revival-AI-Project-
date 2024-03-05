using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpots : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) other.GetComponent<Player>().isHidden = true; 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) other.GetComponent<Player>().isHidden = false;
    }
}
