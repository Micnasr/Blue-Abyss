using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishAnimation : MonoBehaviour
{
    public float animationDuration = 2f;
    public string blendShapeName = "Key 2";

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private int blendShapeIndex;
    private float timer;
    private bool increasing;

    private void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        // Find the index of the blend shape by name
        blendShapeIndex = FindBlendShapeIndex(blendShapeName);

        timer = 0f;
        increasing = true;
    }

    private void Update()
    {
        // Update the blend shape value based on the animation progress
        float blendShapeValue = Mathf.PingPong(timer / animationDuration, 1f) * 100f;
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeValue);

        // Update the animation timer
        timer += Time.deltaTime;
    }

    private int FindBlendShapeIndex(string blendShapeName)
    {
        int blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;
        for (int i = 0; i < blendShapeCount; i++)
        {
            if (skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i) == blendShapeName)
            {
                return i;
            }
        }
        Debug.LogError("Blend shape not found: " + blendShapeName);
        return -1;
    }
}
