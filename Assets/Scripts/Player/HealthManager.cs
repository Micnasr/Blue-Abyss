using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Health Properties")]
    public float maxHealth;
    private float currentHealth;
    public Slider healthMeter;

    public Image healthImage;
    private float maxAlpha = 10f;
    private float minAlpha = 0f;

    [Header("Healing Properties")]
    public float healingAmount;
    public float timeToStartHealing;
    public float updateDuration = 0.5f;

    private float timeSinceLastDamage;

    [Header("Sound Effects")]
    public string damageSound = "PlayerDamage";
    public float damageSoundCooldown = 1f;
    private float lastDamageSoundTime;

    private DeathManager deathManager;

    private void Awake()
    {
        maxHealth = PlayerPrefs.GetFloat("MaxHealth", maxHealth);
    }

    private void Start()
    {
        deathManager = GetComponent<DeathManager>();

        currentHealth = maxHealth;

        healthMeter.maxValue = maxHealth;
        healthMeter.value = currentHealth;
        
        timeSinceLastDamage = 0f;
        lastDamageSoundTime = Time.time;
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

        // Handle the Red UI (How Red the Screen Becomes)
        VisualBloodUI();
    }

    public void TakeDamage(float damage, float duration = 0.5f)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        timeSinceLastDamage = Time.time;

        // Either we want to smoothly update or not
        if (duration >= 0f)
        {
            StartCoroutine(UpdateHealthMeter(currentHealth, duration));
        }
        else
        {
            healthMeter.value = currentHealth;
        }

        // Play Player Damage Sound
        if (Time.time - lastDamageSoundTime >= damageSoundCooldown)
        {
            float randomPitch = Random.Range(0.95f, 1.05f);
            FindObjectOfType<AudioManager>().Play(damageSound, randomPitch);
            lastDamageSoundTime = Time.time;
        }
    }

    private void StartHealingOverTime()
    {
        // Check if healing is needed
        if (currentHealth < maxHealth)
        {
            currentHealth += healingAmount * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            healthMeter.value = currentHealth;
        }
    }

    private IEnumerator UpdateHealthMeter(float targetValue, float duration)
    {
        float initialValue = healthMeter.value;
        float timer = 0f;

        while (timer <= duration)
        {
            timer += Time.deltaTime;
            float currentValue = Mathf.Lerp(initialValue, targetValue, timer / duration);
            healthMeter.value = currentValue;
            yield return null;
        }

        // Ensure the final value is set accurately
        healthMeter.value = targetValue;
    }

    public void HandleDeath()
    {
        deathManager.PlayerDied();
    }

    public void NewMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        healthMeter.maxValue = maxHealth;

        PlayerPrefs.SetFloat("MaxHealth", newMaxHealth);
        PlayerPrefs.Save();
    }

    private void VisualBloodUI()
    {
        float healthPercentage = currentHealth / maxHealth;
        float targetAlpha = Mathf.Lerp(maxAlpha, minAlpha, healthPercentage);
        Color newColor = healthImage.color;
        newColor.a = targetAlpha / 100f;
        healthImage.color = newColor;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        StopAllCoroutines();
        healthMeter.value = currentHealth;
    }
}
