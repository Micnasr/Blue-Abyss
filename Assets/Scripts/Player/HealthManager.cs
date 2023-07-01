using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Health Properties")]
    public float maxHealth;
    private float currentHealth;
    public Slider healthMeter;

    [Header("Healing Properties")]
    public float healingAmount;
    public float timeToStartHealing;

    private float timeSinceLastDamage;

    void Start()
    {
        currentHealth = maxHealth;
        healthMeter.maxValue = maxHealth;
        timeSinceLastDamage = 0f;
    }

    void Update()
    {
        // Update UI
        healthMeter.value = currentHealth;

        if (currentHealth <= 0f)
        {
            HandleDeath();
        }

        // Check if healing should start
        if (Time.time - timeSinceLastDamage >= timeToStartHealing)
        {
            StartHealingOverTime();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        timeSinceLastDamage = Time.time;
    }

    private void StartHealingOverTime()
    {
        // Check if healing is needed
        if (currentHealth < maxHealth)
        {
            currentHealth += healingAmount * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }
    }

    public void HandleDeath()
    {
        Debug.Log("DEAD!");
    }
}