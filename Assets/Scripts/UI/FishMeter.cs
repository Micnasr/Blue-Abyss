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
            fishMeter.value = currentCount;
            statusText.text = currentCount + "/" + maxCount;
        } 
        else
        {
            Debug.LogError("Fish Meter NULL");
        }
    }
}
