using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public ParticleSystem bubbleEffect;
    private ParticleSystem activeEffect;

    public string splashInWater;

    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;

        if (parent.CompareTag("Player") && other.GetComponentInParent<PlayerMovement>() != null && !other.CompareTag("PlayerHead"))
        {
            PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
            movement.isSwimming = true;

            if (splashInWater != "")
                FindObjectOfType<AudioManager>().Play(splashInWater);
            
            SpawnBubbleEffect(other.transform.position, parent);
        }
        else if (other.CompareTag("PlayerHead"))
        {
            OxygenController oxygenScript = other.GetComponentInParent<OxygenController>();
            oxygenScript.isHeadAboveWater = false;

            FindObjectOfType<AudioManager>().FadeTrack("DeepWaterMusic", "ShallowWaterMusic", 1f);

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

            FindObjectOfType<AudioManager>().FadeTrack("ShallowWaterMusic", "DeepWaterMusic", 1f);
            RenderSettings.fog = false;
        }
    }

    private void SpawnBubbleEffect(Vector3 position, Transform parent)
    {
        if (bubbleEffect != null)
        {
            activeEffect = Instantiate(bubbleEffect, position, Quaternion.identity, parent);
            activeEffect.Play();
        }
    }
}