using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OxygenController : MonoBehaviour
{
    [Header("Oxygen Properties")]
    public float maxOxygen;
    public float oxygenDepletionRate;
    public float oxygenRegenRate;
    public float suffocateDamage;

    private float currentOxygen;

    public bool isHeadAboveWater;

    public Slider oxygenMeter;

    HealthManager healthManager;

    private void Awake()
    {
        maxOxygen = PlayerPrefs.GetFloat("MaxOxygen", maxOxygen);
    }

    private void Start()
    {
        currentOxygen = maxOxygen;
        oxygenMeter.maxValue = maxOxygen;

        // Player starts on land
        isHeadAboveWater = true;

        healthManager = GetComponent<HealthManager>();
    }

    private void Update()
    {
        // Update UI
        oxygenMeter.value = currentOxygen;

        // Head under the water
        if (isHeadAboveWater == false)
        {
            // Deplete oxygen over time
            currentOxygen -= oxygenDepletionRate * Time.deltaTime;

            if (currentOxygen <= 0f)
            {
                HandleOutOfOxygen();
            }
        }
        else
        {
            // Regenerate oxygen when not underwater
            currentOxygen += oxygenRegenRate * Time.deltaTime;
            currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
        }

        // Disable the UI if we are full :)
        if (currentOxygen >= maxOxygen)
            oxygenMeter.gameObject.SetActive(false);
        else
            oxygenMeter.gameObject.SetActive(true);

    }

    private void HandleOutOfOxygen()
    {
        SuffocateDamage();
    }

    private void SuffocateDamage()
    {
        Debug.Log("SUFFOCATING");
        if (healthManager != null)
        {
            float damage = suffocateDamage * Time.deltaTime;
            healthManager.TakeDamage(damage, 0f);
        } 
        else
        {
            Debug.LogError("Player Not Found - Health Manager");
        }
    }

    public void NewMaxOxygen(float newMaxOxygen)
    {
        maxOxygen = newMaxOxygen;
        oxygenMeter.maxValue = maxOxygen;
    }
}