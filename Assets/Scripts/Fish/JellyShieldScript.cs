using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyShieldScript : MonoBehaviour
{
    private bool playerInsideTrigger = false;
    public float shieldDamage;

    // Player Health Script
    private HealthManager healthManager;

    private void Start()
    {
        healthManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.CompareTag("Player"))
        {
            playerInsideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.CompareTag("Player"))
        {
            playerInsideTrigger = false;
        }
    }

    private void Update()
    {
        // Inside Jelly Shield
        if (playerInsideTrigger)
        {
            float damage = shieldDamage * Time.deltaTime;
            healthManager.TakeDamage(damage, 0f);
        }
    }
}