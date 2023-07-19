using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemShopLogic : MonoBehaviour
{
    [Header("Price Of Items: Anchor -> Lifevest -> Light -> Jacket")]
    public int[] priceOfItems;

    public TextMeshProUGUI[] textPriceOfItems;

    private Transform player;
    private MoneyManager moneyManager;

    public BackpackUI backpackUI;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        moneyManager = FindObjectOfType<MoneyManager>();

        PopulatePrices();
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

        }
        else
        {
            Debug.Log("No Money :(");
        }
    }
}
  
