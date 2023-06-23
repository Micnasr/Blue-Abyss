using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;

        if (parent.CompareTag("Player") && other.GetComponentInParent<PlayerMovement>() != null && !other.CompareTag("PlayerHead"))
        {
            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement.isSwimming = true;
        } 
        else if (other.CompareTag("PlayerHead"))
        {
            OxygenController oxygenScript = other.GetComponentInParent<OxygenController>();
            oxygenScript.isHeadAboveWater = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform parent = other.transform.parent;

        // If Body leaves the water then we are NO longer swimming
        if (parent.CompareTag("Player") && other.GetComponentInParent<PlayerMovement>() != null && !other.CompareTag("PlayerHead"))
        {
            Debug.Log(other.gameObject.name);
            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement.isSwimming = false;
        }
        // If head leaves the water only then we can breath
        else if (other.CompareTag("PlayerHead"))
        {
            OxygenController oxygenScript = other.GetComponentInParent<OxygenController>();
            oxygenScript.isHeadAboveWater = true;
        }
    }
}
