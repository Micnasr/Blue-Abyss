using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteDetector : MonoBehaviour
{
    public float biteCooldown = 1f;
    public float biteDamage;
    private bool canBite = true;
    private bool isPlayerInside = false;
    private float lastBiteTime;

    private HealthManager healthManager;

    private void Start()
    {
        healthManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
    }

    private void Update()
    {
        if (isPlayerInside && canBite && Time.time - lastBiteTime >= biteCooldown)
        {
            Debug.Log("Player bit!");
            healthManager.TakeDamage(biteDamage);

            // Set the last bite time
            lastBiteTime = Time.time;

            // Set canBite to false to prevent repeated biting
            canBite = false;

            // Start the bite cooldown timer
            StartCoroutine(BiteCooldown());
        }
    }

    private IEnumerator BiteCooldown()
    {
        yield return new WaitForSeconds(biteCooldown);

        // Reset canBite to allow biting again
        canBite = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}