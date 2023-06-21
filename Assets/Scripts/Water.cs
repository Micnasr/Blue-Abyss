using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;

        if (parent.CompareTag("Player") && other.GetComponentInParent<PlayerMovement>() != null)
        {
            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement.isSwimming = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform parent = other.transform.parent;

        if (parent.CompareTag("Player") && other.GetComponentInParent<PlayerMovement>() != null)
        {
            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement.isSwimming = false;
        }
    }
}
