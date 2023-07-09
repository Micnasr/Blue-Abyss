using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishAnimation : MonoBehaviour
{
    [Header("Animation Parameters")]
    public float animationDuration = 2f;
    public string blendShapeName = "Key 2";

    [Header("Movement Parameters")]
    public float bobbingAmount = 0.5f;
    public float bobbingSpeed = 1f;
    public float rotationSpeed = 50f;

    [Header("Attack Parameters")]
    public float attackDistance = 20f;
    public GameObject jellyStingPrefab;
    public float stingRadiusMin = 0.1f;
    public float stingRadiusMax = 20f;
    public float stingDuration = 3f;
    public float spawnDelay = 15f;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private int blendShapeIndex;
    private float timer;

    private bool playerInRange = false;
    private bool stingActive = false;
    private GameObject currentSting;
    private Transform playerTransform;

    private void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        // Find the index of the blend shape by name
        blendShapeIndex = FindBlendShapeIndex(blendShapeName);

        timer = 0f;

        // Find the player object by tag
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        AnimateJelly();
        MoveJelly();
        AttackPlayer();
    }

    private void MoveJelly()
    {
        // Calculate the vertical bobbing motion
        float verticalOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        Vector3 newPosition = transform.position;
        newPosition.y = newPosition.y + verticalOffset;
        transform.position = newPosition;

        // Rotate the jellyfish around its own axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }


    private void AttackPlayer()
    {
        if(playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            playerInRange = distance <= attackDistance;
        }

        if (playerInRange && !stingActive)
        {
            StartCoroutine(ActivateSting());
        }
    }

    private void AnimateJelly()
    {
        // Update the blend shape value based on the animation progress
        float blendShapeValue = Mathf.PingPong(timer / animationDuration, 1f) * 100f;
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeValue);

        // Update the animation timer
        timer += Time.deltaTime;
    }

    private IEnumerator ActivateSting()
    {
        stingActive = true;

        // Spawn jelly sting prefab
        currentSting = Instantiate(jellyStingPrefab, transform.position, Quaternion.identity);
        Transform scaleTransform = currentSting.transform;
        Vector3 initialScale = scaleTransform.localScale;
        Vector3 targetScale = Vector3.one * stingRadiusMax;

        float t = 0f;
        while (t < 1f)
        {
            // Scale the sphere gradually
            scaleTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            t += Time.deltaTime / stingDuration;
            yield return null;
        }

        yield return new WaitForSeconds(stingDuration);

        Destroy(currentSting);
        currentSting = null;

        // Wait for the spawn delay before allowing the sphere to spawn back
        yield return new WaitForSeconds(spawnDelay);

        stingActive = false;
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
