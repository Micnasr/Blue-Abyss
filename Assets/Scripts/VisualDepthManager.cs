using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VisualDepthManager : MonoBehaviour
{
    private const byte k_MaxByteForOverexposedColor = 191;

    // G value of the Color (Makes it more balue)
    public float minGValue = 50f;
    public float maxGValue = 170f;

    // Fog Values
    public float minFog = 20f;
    public float maxFog = 70f;

    // Boundary of the map
    public float minDepth = -400f;
    public float maxDepth = -3f;

    public float intensity = 1f;

    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;

    private float currentBValue;
    private float currentFogEnd;

    private void Start()
    {
        // Initialize the current value to the initial value
        currentBValue = maxGValue;

        // Get the ColorAdjustments component from the Post Processing Volume
        postProcessingVolume.profile.TryGet(out colorAdjustments);

        // Set the default R and B values of the color filter
        if (colorAdjustments != null)
        {
            Color baseLinearColor = new Color32(35, 146, 191, 255);
            colorAdjustments.colorFilter.value = ComposeHdrColor(baseLinearColor, intensity);

        }
    }

    private void Update()
    {
        UpdateDepthColor();
        UpdateFog();
    }

    private void UpdateFog()
    {
        // Get the player's Y coordinate
        float playerY = transform.position.y;

        // Calculate the normalized depth
        float normalizedDepth = Mathf.InverseLerp(minDepth, maxDepth, playerY);

        RenderSettings.fogEndDistance = Mathf.Lerp(minFog, maxFog, normalizedDepth);
    }

    private void UpdateDepthColor()
    {
        // Get the player's Y coordinate
        float playerY = transform.position.y;

        // Calculate the normalized depth
        float normalizedDepth = Mathf.InverseLerp(minDepth, maxDepth, playerY);

        // Calculate the new value based on the normalized depth
        currentBValue = Mathf.Lerp(minGValue, maxGValue, normalizedDepth);

        // Set the G and B values of the Color Filter to the normalized depth
        if (colorAdjustments != null)
        {
            Color baseLinearColor = new Color32(35, (byte)Mathf.RoundToInt(currentBValue), 191, 255);
            colorAdjustments.colorFilter.value = ComposeHdrColor(baseLinearColor, intensity);
        }
    }

    public static Color ComposeHdrColor(Color32 baseLinearColor, float intensity)
    {
        float scaleFactor = k_MaxByteForOverexposedColor / intensity;
        float r = baseLinearColor.r / scaleFactor;
        float g = baseLinearColor.g / scaleFactor;
        float b = baseLinearColor.b / scaleFactor;
        float a = 255f;

        return new Color(r, g, b, a);
    }    
}
