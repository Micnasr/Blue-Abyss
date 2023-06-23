using System.Collections;
using UnityEngine;

public class OxygenController : MonoBehaviour
{
    public float maxOxygen = 100f;
    public float oxygenDepletionRate = 1f;
    public float oxygenRegenRate = 2f;

    private float currentOxygen;

    public bool isHeadAboveWater = false;

    private void Start()
    {
        currentOxygen = maxOxygen;
    }

    private void Update()
    {
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