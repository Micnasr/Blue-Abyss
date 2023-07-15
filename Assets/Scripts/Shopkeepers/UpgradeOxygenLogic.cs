using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UpgradeOxygenLogic : MonoBehaviour
{
    [Header("Upgrade Data")]
    public float[] oxygenStages;
    public int[] prices;

    [Header("Reference Data")]
    public Image[] arrows;

    private int maxStage;

    public TextMeshProUGUI differenceText;
    public TextMeshProUGUI priceText;

    public GameObject purchaseButton;
    public GameObject moneyIcon;

    private GameObject player;
    private OxygenController oxygenController;
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
        oxygenController = player.GetComponent<OxygenController>();
        moneyManager = FindObjectOfType<MoneyManager>();

        maxStage = oxygenStages.Length;

        // Make sure both arrays are the same size
        if (oxygenStages.Length != prices.Length)
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
        float maxOxy = oxygenController.maxOxygen;

        for (int i = 0; i < oxygenStages.Length; i++)
        {
            if (maxOxy == oxygenStages[i])
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
            differenceText.text = oxygenController.maxOxygen.ToString() + " -> " + oxygenStages[nextStage].ToString();

            // Update Price
            priceText.text = "$" + prices[nextStage].ToString();
        }
        // Completed Screen
        else
        {
            differenceText.text = oxygenStages[maxStage-1].ToString();
            priceText.text = "MAX";
            purchaseButton.SetActive(false);
            moneyIcon.SetActive(false);
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
            // Update Max Oxygen
            oxygenController.NewMaxOxygen(oxygenStages[nextStage]);
            PlayerPrefs.SetFloat("MaxOxygen", oxygenStages[nextStage]);
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
