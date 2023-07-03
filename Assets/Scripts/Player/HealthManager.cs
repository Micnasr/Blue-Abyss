using System.Collections;
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
    public float updateDuration = 0.5f;

    private float timeSinceLastDamage;

    private void Start()
    {
        currentHealth = maxHealth;

        healthMeter.maxValue = maxHealth;
        healthMeter.value = currentHealth;
        
        timeSinceLastDamage = 0f;
    }

    private void Update()
    {
        if (currentHealth <= 0f)
        {
            HandleDeath();
            return;
        }

        // Check if healing should start && CHECK FOR COMBAT STATE (FUTURE)
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

        // Start the coroutine to smoothly update the health meter
        StartCoroutine(UpdateHealthMeter(currentHealth));
    }

    private void StartHealingOverTime()
    {
        // Check if healing is needed
        if (currentHealth < maxHealth)
        {
            currentHealth += healingAmount * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            // Start the coroutine to smoothly update the health meter
            StartCoroutine(UpdateHealthMeter(currentHealth));
        }
    }

    private IEnumerator UpdateHealthMeter(float targetValue)
    {
        float initialValue = healthMeter.value;
        float timer = 0f;

        while (timer <= updateDuration)
        {
            timer += Time.deltaTime;
            float currentValue = Mathf.Lerp(initialValue, targetValue, timer / updateDuration);
            healthMeter.value = currentValue;
            yield return null;
        }

        // Ensure the final value is set accurately
        healthMeter.value = targetValue;
    }

    public void HandleDeath()
    {
        Debug.Log("Player DEAD!");
    }
}