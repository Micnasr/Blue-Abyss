using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FishMeter : MonoBehaviour
{
    private Slider fishMeter;
    public TextMeshProUGUI statusText;

    public int currentCount = 0;
    public int maxCount = 5;

    private void Start()
    {
        fishMeter = GetComponent<Slider>();

        if (fishMeter != null)
        {
            fishMeter.value = currentCount;
            fishMeter.maxValue = maxCount;
            statusText.text = "0/" + maxCount;
        }
        else
        {
            Debug.LogError("Fish Meter NULL");
        }
    }
    public void AddFishDeath(int amount)
    {
        currentCount += amount;
        currentCount = Mathf.Clamp(currentCount, 0, maxCount);
        UpdateSlider();
    }


    private void UpdateSlider()
    {
        if (fishMeter != null)
        {
            statusText.text = currentCount + "/" + maxCount;

            // Smoothly Update Slider
            StartCoroutine(UpdateSliderCoroutine());
        }
    }

    private IEnumerator UpdateSliderCoroutine()
    {
        float duration = 1f; 
        float elapsed = 0f;
        float startValue = fishMeter.value;
        float targetValue = currentCount;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float interpolatedValue = Mathf.Lerp(startValue, targetValue, t);
            fishMeter.value = interpolatedValue;

            yield return null;
        }

        // Ensure the final value is set accurately
        fishMeter.value = targetValue;
    }
}
