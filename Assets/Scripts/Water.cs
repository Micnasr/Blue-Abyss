using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public ParticleSystem bubbleEffect;

    private ParticleSystem activeEffect;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;

        if (parent.CompareTag("Player") && other.GetComponentInParent<PlayerMovement>() != null && !other.CompareTag("PlayerHead"))
        {
            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement.isSwimming = true;

            SpawnBubbleEffect(other.transform.position);
        }
        else if (other.CompareTag("PlayerHead"))
        {
            OxygenController oxygenScript = other.GetComponentInParent<OxygenController>();
            oxygenScript.isHeadAboveWater = false;

            RenderSettings.fog = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform parent = other.transform.parent;

        // If Body leaves the water then we are NO longer swimming
        if (parent.CompareTag("Player") && other.GetComponentInParent<PlayerMovement>() != null && !other.CompareTag("PlayerHead"))
        {
            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement.isSwimming = false;
        }
        // If head leaves the water only then we can breathe
        else if (other.CompareTag("PlayerHead"))
        {
            OxygenController oxygenScript = other.GetComponentInParent<OxygenController>();
            oxygenScript.isHeadAboveWater = true;

            RenderSettings.fog = false;
        }
    }

    private void Update()
    {
        // Teleport Particles to Player while player is running (to make more visible)
        if (activeEffect != null && player != null)
        {
            activeEffect.transform.position = player.transform.position;
        }
    }

    private void SpawnBubbleEffect(Vector3 position)
    {
        if (bubbleEffect != null)
        {
            activeEffect = Instantiate(bubbleEffect, position, Quaternion.identity);
            activeEffect.Play();
        }
    }
}
