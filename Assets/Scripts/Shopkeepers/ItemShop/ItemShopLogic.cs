using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemShopLogic : MonoBehaviour
{
    [Header("Price Of Items: Anchor -> Lifevest -> Light -> Jacket")]
    public int[] priceOfItems;

    public TextMeshProUGUI[] textPriceOfItems;
    public GameObject[] purchaseButtons;

    private MoneyManager moneyManager;

    public BackpackUI backpackUI;

    [Header("Colors")]
    public Color affordColor;
    public Color brokeColor;

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();

        PopulatePrices();

        UpdateUI();
    }

    private void Update()
    {
        UpdatePriceColor(); 
    }

    private void UpdateUI()
    {
        string[] names = { "Anchor", "Lifevest", "Light", "Jacket" };
        for (int i = 0; i < names.Length; i++)
        {
            if (backpackUI.itemsUnlockedStr.Contains(names[i]))
            {
                textPriceOfItems[i].text = "OWNED";
                purchaseButtons[i].SetActive(false);
            }
        }
    }

    private void UpdatePriceColor()
    {
        // Update the Color to be Red if we cannot afford
        for (int i = 0; i < textPriceOfItems.Length; i++)
        {
            if (moneyManager.currentMoney < priceOfItems[i] && textPriceOfItems[i].text != "OWNED")
                textPriceOfItems[i].color = brokeColor;
            else
                textPriceOfItems[i].color = affordColor;
        }
        
    }

    private void PopulatePrices()
    {
        for (int i = 0; i < priceOfItems.Length; i++)
            textPriceOfItems[i].text = "$" + priceOfItems[i].ToString();
    }

    public void BuyAnchor()
    {
        string name = "Anchor";
        int index = 0;

        if (backpackUI.itemsUnlockedStr.Contains(name))
        {
            return;
        }

        if (moneyManager.currentMoney >= priceOfItems[index])
        {
            moneyManager.RemoveMoney(priceOfItems[index]);

            //Give Item to Player
            backpackUI.BoughtItem(name);
            UpdateUI();
        }
        else
        {
            Debug.Log("No Money :(");
        }
    }

    public void BuyLifevest()
    {
        string name = "Lifevest";
        int index = 1;

        if (backpackUI.itemsUnlockedStr.Contains(name))
        {
            return;
        }

        if (moneyManager.currentMoney >= priceOfItems[index])
        {
            moneyManager.RemoveMoney(priceOfItems[index]);

            //Give Item to Player
            backpackUI.BoughtItem(name);
            UpdateUI();
        }
        else
        {
            Debug.Log("No Money :(");
        }
    }

    public void BuyLight()
    {
        string name = "Light";
        int index = 2;

        if (backpackUI.itemsUnlockedStr.Contains(name))
        {
            return;
        }

        if (moneyManager.currentMoney >= priceOfItems[index])
        {
            moneyManager.RemoveMoney(priceOfItems[index]);

            //Give Item to Player
            backpackUI.BoughtItem(name);
            UpdateUI();
        }
        else
        {
            Debug.Log("No Money :(");
        }
    }

    public void BuyJacket()
    {
        string name = "Jacket";
        int index = 3;

        if (backpackUI.itemsUnlockedStr.Contains(name))
        {
            return;
        }

        if (moneyManager.currentMoney >= priceOfItems[index])
        {
            moneyManager.RemoveMoney(priceOfItems[index]);

            //Give Item to Player
            backpackUI.BoughtItem(name);
            UpdateUI();
        }
        else
        {
            Debug.Log("No Money :(");
        }
    }
}
  
