using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OxygenController : MonoBehaviour
{
    public float maxOxygen;
    public float oxygenDepletionRate;
    public float oxygenRegenRate;

    private float currentOxygen;

    public bool isHeadAboveWater;

    public Slider oxygenMeter;

    private void Start()
    {
        currentOxygen = maxOxygen;
        oxygenMeter.maxValue = maxOxygen;

        // Player starts on land
        isHeadAboveWater = true;
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
    }

    private void HandleOutOfOxygen()
    {
        Debug.Log("DROWNING");
    }
}