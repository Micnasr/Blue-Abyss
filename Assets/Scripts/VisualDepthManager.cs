using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VisualDepthManager : MonoBehaviour
{
    private const byte k_MaxByteForOverexposedColor = 191;

    public float minValue = 50f;
    public float maxValue = 170f;
    public float minDepth = -400f;
    public float maxDepth = -3f;

    public float intensity = 1f;

    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;

    private float currentValue;

    private void Start()
    {
        // Initialize the current value to the initial value
        currentValue = maxValue;

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
        // Get the player's Y coordinate
        float playerY = transform.position.y;

        // Calculate the normalized depth
        float normalizedDepth = Mathf.InverseLerp(minDepth, maxDepth, playerY);

        // Calculate the new value based on the normalized depth
        currentValue = Mathf.Lerp(minValue, maxValue, normalizedDepth);

        // Set the G and B values of the Color Filter to the normalized depth
        if (colorAdjustments != null)
        {
            Color baseLinearColor = new Color32(35, (byte)Mathf.RoundToInt(currentValue), 191, 255);
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
