using System.Collections;
using UnityEngine;

public class OxygenController : MonoBehaviour
{
    public float maxOxygen = 100f;
    public float oxygenDepletionRate = 1f;
    public float oxygenRegenRate = 2f;

    private float currentOxygen;
    private bool isUnderwater;

    private PlayerMovement underwaterDetection;

    private void Start()
    {
        currentOxygen = maxOxygen;
        underwaterDetection = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        isUnderwater = underwaterDetection.isSwimming;

        if (isUnderwater)
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