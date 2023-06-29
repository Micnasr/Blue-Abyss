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

    void Start()
    {
        currentHealth = maxHealth;
        healthMeter.maxValue = maxHealth;
    }

    void Update()
    {
        // Update UI
        healthMeter.value = currentHealth;

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
        Debug.Log("DEAD!");
    }
}
