using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHealthManager : MonoBehaviour
{
    [Header("Fish Health Properties")]
    public float maxHealth;
    private float currentHealth;

    [Header("Dead Properties")]
    public Renderer fishRenderer;
    public Material deathMaterial;

    public float height = 0;

    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float duration = 1.0f;

    // Flag to track if the fish has died
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        fishRenderer = GetComponentInChildren<Renderer>();
        if (fishRenderer == null)
        {
            Debug.LogError("FishHealthManager: Renderer component not found!");
        }
    }

    void Update()
    {
        if (currentHealth <= 0f)
        {
            HandleDeath();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
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

        // Start Dissolving Animation
        StartCoroutine(DissolveHeightTransition(uniqueDeathMaterial));
    }

    private IEnumerator DissolveHeightTransition(Material uniqueDeathMaterial)
    {
        float startHeight = transform.position.y + 0.2f;
        float endHeight = startHeight - 0.5f;

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
        //Destroy(gameObject);
    }

    private void SetHeight(Material uniqueDeathMaterial, float height)
    {
        uniqueDeathMaterial.SetFloat("_CutoffHeight", height);
        uniqueDeathMaterial.SetFloat("_NoiseStrength", noiseStrength);
    }
}
