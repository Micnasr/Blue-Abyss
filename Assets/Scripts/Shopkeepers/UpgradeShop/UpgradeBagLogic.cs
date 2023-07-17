using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UpgradeBagLogic : MonoBehaviour
{
    [Header("Upgrade Data")]
    public int[] bagStages;
    public int[] prices;

    [Header("Reference Data")]
    public Image[] arrows;

    private int maxStage;

    public TextMeshProUGUI differenceText;
    public TextMeshProUGUI priceText;

    public GameObject purchaseButton;

    private GameObject player;

    public FishMeter fishMeter;
    private MoneyManager moneyManager;

    [Header("Colors")]
    public Color affordColor;
    public Color brokeColor;
    public Color CompletedArrow;

    // Current Upgrade Stage (0-4)
    private int nextStage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        moneyManager = FindObjectOfType<MoneyManager>();

        maxStage = bagStages.Length;

        // Make sure both arrays are the same size
        if (bagStages.Length != prices.Length)
            Debug.LogError("Price and Element Size UNEQUAL!");

        nextStage = FigureOutCurrentStage();
        UpdateUI();
    }

    private void Update()
    {
        UpdatePriceColor();
    }

    private void UpdatePriceColor()
    {
        if (nextStage < maxStage)
        {
            // Update the Color to be Red if we cannot afford
            if (moneyManager.currentMoney >= prices[nextStage])
                priceText.color = affordColor;
            else
                priceText.color = brokeColor;
        }
    }

    private int FigureOutCurrentStage()
    {
        int maxBag = fishMeter.maxBag;

        for (int i = 0; i < bagStages.Length; i++)
        {
            if (maxBag == bagStages[i])
            {
                return i + 1;
            }
        }

        return 0;
    }

    public void UpdateUI()
    {
        // Update the User Interface Accordingly
        if (nextStage < maxStage)
        {
            // Update Difference Text
            differenceText.text = fishMeter.maxBag.ToString() + " -> " + bagStages[nextStage].ToString();

            // Update Price
            priceText.text = "$" + prices[nextStage].ToString();
        }
        // Completed Screen
        else
        {
            differenceText.text = bagStages[maxStage-1].ToString();
            priceText.text = "MAX";
            purchaseButton.SetActive(false);
        }

        // Render The Arrows Depending on BUY STAGE
        for (int i = 0; i < nextStage; i++)
        {
            arrows[i].color = CompletedArrow;
        }
    }

    private void AdvanceStage()
    {
        // Make sure we do not go out of bounds
        if (nextStage < maxStage)
        {
            // Update Size of Max Bag
            fishMeter.NewMaxCount(bagStages[nextStage]);
            PlayerPrefs.SetInt("MaxBag", bagStages[nextStage]);
            PlayerPrefs.Save();
            
            nextStage++;
            UpdateUI();
        }
    }

    public void BuyUpgrade()
    {
        // Check if player can afford
        if (moneyManager.currentMoney >= prices[nextStage])
        {
            moneyManager.RemoveMoney(prices[nextStage]);
            AdvanceStage();
        } 
        else
        {
            Debug.Log("No Money :(");
        }
        
    }
}
