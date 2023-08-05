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

    public bool inSubmarine = false;

    public bool isHeadAboveWater;

    public Slider oxygenMeter;

    HealthManager healthManager;

    [Header("Sound Effects")]
    public string lowOxygen = "OxygenLowBeep";
    private bool lowOxygenSoundPlayed = false;

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

        float lowOxygenThreshold = maxOxygen * 0.25f;

        // Head under the water
        if (isHeadAboveWater == false && !inSubmarine)
        {
            // Deplete oxygen over time
            currentOxygen -= oxygenDepletionRate * Time.deltaTime;

            if (currentOxygen <= 0f)
            {
                HandleOutOfOxygen();
            }

            // Play Sound If Low Oxygen
            if (currentOxygen <= lowOxygenThreshold && !lowOxygenSoundPlayed)
            {
                FindObjectOfType<AudioManager>().Play("OxygenLowBeep");
                lowOxygenSoundPlayed = true;
            }
        }
        else
        {
            // Regenerate oxygen when not underwater
            currentOxygen += oxygenRegenRate * Time.deltaTime;
            currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);

            // Stop Low Oxygen Sound
            if (lowOxygenSoundPlayed)
            {
                FindObjectOfType<AudioManager>().StopPlaying("OxygenLowBeep");
                lowOxygenSoundPlayed = false;
            }
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