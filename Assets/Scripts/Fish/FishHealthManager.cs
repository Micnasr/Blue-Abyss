using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHealthManager : MonoBehaviour
{
    [Header("Fish Health Properties")]
    public float maxHealth;
    private float currentHealth;

    [Header("Dead Properties")]
    public int deathPoints;
    private Renderer fishRenderer;
    public Material deathMaterial;
    public float shaderTopThreshold;
    public float shaderBottomThreshold;

    // Only for flocks
    private FlockUnit flockUnit;
    // Only for water animals
    private EnemyPatrol enemyPatrol;
    // Only for land animals
    private LandEnemyPatrol landEnemyPatrol;

    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float duration = 1.0f;

    public GameObject damageEffect;
    private bool playedEffect = false;

    private float runAwaySpeed;

    // Flag to track if the fish has died
    private bool isDead = false;

    private FishMeter fishMeter;

    void Start()
    {
        currentHealth = maxHealth;

        fishRenderer = GetComponentInChildren<Renderer>();
        if (fishRenderer == null)
        {
            Debug.LogError("FishHealthManager: Renderer component not found!");
        }

        flockUnit = GetComponent<FlockUnit>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        landEnemyPatrol = GetComponent<LandEnemyPatrol>();

        // Calculate Run Away Speed
        if (enemyPatrol != null)
            runAwaySpeed = enemyPatrol.movementSpeed * 2f;

        fishMeter = FindObjectOfType<FishMeter>();
    }

    void Update()
    {
        if (currentHealth <= 0f)
        {
            HandleDeath();
        } 
        else if (maxHealth > 6 && (currentHealth <= maxHealth * 0.3f) && !playedEffect)
        {
            PlayDamageEffect();
            playedEffect = true;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (enemyPatrol != null)
            StartCoroutine(RunAway());
    }

    private IEnumerator RunAway()
    {
        float originalSpeed = enemyPatrol.movementSpeed;
        float elapsedTime = 0f;
        float accelerationDuration = 0.7f;
        float decelerationDuration = 1f;

        while (elapsedTime < accelerationDuration)
        {
            // Calculate the t value for interpolation (acceleration)
            float t = elapsedTime / accelerationDuration;
            enemyPatrol.movementSpeed = Mathf.Lerp(originalSpeed, runAwaySpeed, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the fish reaches the runaway speed
        enemyPatrol.movementSpeed = runAwaySpeed;

        yield return new WaitForSeconds(5f);

        elapsedTime = 0f;

        while (elapsedTime < decelerationDuration)
        {
            // Calculate the t value for interpolation (deceleration)
            float t = elapsedTime / decelerationDuration;
            enemyPatrol.movementSpeed = Mathf.Lerp(runAwaySpeed, originalSpeed, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the fish reaches the original speed
        enemyPatrol.movementSpeed = originalSpeed;
    }

    public void HandleDeath()
    {
        // Exit the function if the fish is already dead
        if (isDead) return;

        // Set the flag to indicate the fish has died
        isDead = true;

        Debug.Log("DEAD: " + gameObject.transform.name);

        // Create a unique instance of the death material
        Material uniqueDeathMaterial = Instantiate(deathMaterial);

        // Replace all materials with the unique death material
        Material[] materials = new Material[fishRenderer.sharedMaterials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = uniqueDeathMaterial;
        }

        fishRenderer.sharedMaterials = materials;

        // Disable the respective movement script
        if (flockUnit != null)
        {
            flockUnit.enabled = false;
        }
        else if (enemyPatrol != null)
        {
            enemyPatrol.enabled = false;
        }
        else if (landEnemyPatrol != null)
        {
             landEnemyPatrol.enabled = false;
        }

        // Give Rewards When Dead
        fishMeter.AddFishDeath(deathPoints);

        // Start Dissolving Animation
        StartCoroutine(DissolveHeightTransition(uniqueDeathMaterial));
    }

    private IEnumerator DissolveHeightTransition(Material uniqueDeathMaterial)
    {
        float startHeight = transform.position.y + shaderTopThreshold;
        float endHeight = startHeight - shaderBottomThreshold;

        float height;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            height = Mathf.Lerp(startHeight, endHeight, elapsedTime / duration);
            SetHeight(uniqueDeathMaterial, height);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetHeight(uniqueDeathMaterial, endHeight);

        // Remove The Fish
        Destroy(gameObject);
    }

    private void SetHeight(Material uniqueDeathMaterial, float height)
    {
        uniqueDeathMaterial.SetFloat("_CutoffHeight", height);
        uniqueDeathMaterial.SetFloat("_NoiseStrength", noiseStrength);
    }

    private void PlayDamageEffect()
    {
        if (damageEffect != null)
        {
            // Instantiate the damage effect as a child of the fish
            damageEffect = Instantiate(damageEffect, transform);
            damageEffect.transform.localPosition = Vector3.zero;
            damageEffect.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

            // Play the particle system
            ParticleSystem particleSystem = damageEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }
    }
}