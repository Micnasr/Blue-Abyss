using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyShieldScript : MonoBehaviour
{
    private bool playerInsideTrigger = false;
    public float shieldDamage;
    public GameObject inShieldEffectPrefab;

    private GameObject inShieldEffect;
    private Transform playerTransform;
    private bool inShieldEffectSpawned = false;

    // Player Health Script
    private HealthManager healthManager;

    private void Start()
    {
        healthManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.CompareTag("Player"))
        {
            playerInsideTrigger = true;

            if (!inShieldEffectSpawned)
            {
                SpawnInShieldEffect();
                FindObjectOfType<AudioManager>().Play("JellyfishElectricity");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.CompareTag("Player"))
        {
            playerInsideTrigger = false;

            if (inShieldEffectSpawned)
            {
                DestroyInShieldEffect();
                FindObjectOfType<AudioManager>().StopPlaying("JellyfishElectricity");
            }
        }
    }

    private void Update()
    {
        // Inside Jelly Shield
        if (playerInsideTrigger)
        {
            float damage = shieldDamage * Time.deltaTime;
            healthManager.TakeDamage(damage, 0f);

            // Update the inShield effect position
            if (inShieldEffect != null)
                inShieldEffect.transform.position = playerTransform.position;
        }
    }

    private void SpawnInShieldEffect()
    {
        if (inShieldEffectPrefab != null)
        {
            inShieldEffect = Instantiate(inShieldEffectPrefab, playerTransform.position, Quaternion.identity);
            inShieldEffect.transform.SetParent(playerTransform);
            inShieldEffectSpawned = true;
        }
    }

    private void DestroyInShieldEffect()
    {
        if (inShieldEffect != null)
        {
            Destroy(inShieldEffect);
            inShieldEffectSpawned = false;
        }
    }

    public void DestroyShield()
    {
        if (inShieldEffect != null)
        {
            Destroy(inShieldEffect);
            inShieldEffectSpawned = false;
        }

        Destroy(transform.root.gameObject);
    }
}

