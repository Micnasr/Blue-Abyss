using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHealthManager : MonoBehaviour
{
    [Header("Fish Health Properties")]
    public float maxHealth;
    [HideInInspector] public float currentHealth;

    [Header("Dead Properties")]
    public int deathPoints;
    private Renderer fishRenderer;
    public Material deathMaterial;
    public float shaderTopThreshold;
    public float shaderBottomThreshold;

    public bool doesBleed = false;
    public float bloodHeightOffset = 0;

    // Only for flocks
    private FlockUnit flockUnit;
    // Only for water animals
    private EnemyPatrol enemyPatrol;
    // Only for land animals
    private LandEnemyPatrol landEnemyPatrol;

    private QuestController questController;

    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float duration = 1.0f;

    public GameObject damageEffect;
    private bool playedEffect = false;

    public float runAwayMultiplier = 2f;
    private float runAwaySpeed;
    private bool runningAway = false;

    // Flag to track if the fish has died
    [HideInInspector] public bool isDead = false;

    private FishMeter fishMeter;

    [Header("Sound Effects")]
    public string criticalSound;

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

        questController = FindAnyObjectByType<QuestController>();

        // Calculate Run Away Speed
        if (enemyPatrol != null || landEnemyPatrol != null)
            if (enemyPatrol != null)
                runAwaySpeed = enemyPatrol.movementSpeed * runAwayMultiplier;
            else if (landEnemyPatrol != null)
                runAwaySpeed = landEnemyPatrol.movementSpeed * runAwayMultiplier;
            
        fishMeter = FindObjectOfType<FishMeter>();
    }

    void Update()
    {
        if (currentHealth <= 0f)
        {
            HandleDeath();
        } 
        else if (doesBleed && (currentHealth <= maxHealth * 0.3f) && !playedEffect)
        {
            PlayDamageEffect();

            if (criticalSound != "")
                FindObjectOfType<AudioManager>().Play(criticalSound, 1f, gameObject);
            
            playedEffect = true;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (enemyPatrol != null || landEnemyPatrol)
        {
            if (!runningAway)
            {
                runningAway = true;
                StartCoroutine(RunAway());
            }
        }
    }

    private IEnumerator RunAway()
    {
        float originalSpeed = 0f;
        if (enemyPatrol != null)
            originalSpeed = enemyPatrol.movementSpeed;
        else if (landEnemyPatrol != null)
            originalSpeed = landEnemyPatrol.movementSpeed;

        float elapsedTime = 0f;
        float accelerationDuration = 0.7f;
        float decelerationDuration = 1f;

        while (elapsedTime < accelerationDuration)
        {
            // Calculate the t value for interpolation (acceleration)
            float t = elapsedTime / accelerationDuration;

            if (enemyPatrol != null)
                enemyPatrol.movementSpeed = Mathf.Lerp(originalSpeed, runAwaySpeed, t);
            else if (landEnemyPatrol != null)
                landEnemyPatrol.movementSpeed = Mathf.Lerp(originalSpeed, runAwaySpeed, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the fish reaches the runaway speed
        if (enemyPatrol != null)
            enemyPatrol.movementSpeed = runAwaySpeed;
        else if (landEnemyPatrol != null)
            landEnemyPatrol.movementSpeed = runAwaySpeed;

        yield return new WaitForSeconds(5f);

        elapsedTime = 0f;

        while (elapsedTime < decelerationDuration)
        {
            // Calculate the t value for interpolation (deceleration)
            float t = elapsedTime / decelerationDuration;
            
            if (enemyPatrol != null)
                enemyPatrol.movementSpeed = Mathf.Lerp(runAwaySpeed, originalSpeed, t);
            else if (landEnemyPatrol != null)
                landEnemyPatrol.movementSpeed = Mathf.Lerp(runAwaySpeed, originalSpeed, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the fish reaches the original speed
        if (enemyPatrol != null)
            enemyPatrol.movementSpeed = originalSpeed;
        else if (landEnemyPatrol != null)
            landEnemyPatrol.movementSpeed = originalSpeed;

        runningAway = false;
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

        // Handle If In Quest
        questController.KillProgress(gameObject);

        // Remove Bleed Effect
        if (doesBleed && playedEffect)
            Destroy(damageEffect);

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
            // Calculate the spawn position with height offset
            Vector3 spawnPosition = transform.position + (Vector3.up * bloodHeightOffset);

            // Instantiate the damage effect at the spawn position as a child of the fish
            damageEffect = Instantiate(damageEffect, spawnPosition, Quaternion.Euler(0f, 90f, 0f), transform);

            // Play the particle system
            ParticleSystem particleSystem = damageEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }
    }
}