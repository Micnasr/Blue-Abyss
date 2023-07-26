using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class BoatSellerLogic : MonoBehaviour
{
    // This code is a lot cleaner than the one in ItemShopLogic, I came a long way ~MN
    [Header("Price Of Items: Dinghy -> MiniSub")]
    string[] names = { "Dinghy", "MiniSub" };
    public int[] priceOfItems;

    private string ownedBoatsStr;

    public TextMeshProUGUI[] textPriceOfItems;
    public GameObject[] purchaseButtons;
    public GameObject[] spawnButtons;

    public GameObject[] boatPrefabs;
    public Transform[] boatSpawnLocation;
    private GameObject currentBoat;

    private MoneyManager moneyManager;

    [Header("Colors")]
    public Color affordColor;
    public Color brokeColor;

    private void Awake()
    {
        ownedBoatsStr = PlayerPrefs.GetString("OwnedBoats", "");
    }

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();

        UpdateUI();
    }

    private void Update()
    {
        UpdatePriceColor();
    }

    private void UpdateUI()
    { 
        for (int i = 0; i < names.Length; i++)
        {
            if (ownedBoatsStr.Contains(names[i]))
            {
                textPriceOfItems[i].text = "OWNED";
                spawnButtons[i].SetActive(true);
                purchaseButtons[i].SetActive(false);
            }
            else
            {
                textPriceOfItems[i].text = "$" + priceOfItems[i].ToString();
                spawnButtons[i].SetActive(false);
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

    public void BuyBoat(int boatIndex)
    {
        if (ownedBoatsStr.Contains(names[boatIndex]))
            return;

        if (moneyManager.currentMoney >= priceOfItems[boatIndex])
        {
            moneyManager.RemoveMoney(priceOfItems[boatIndex]);

            ownedBoatsStr += (names[boatIndex] + "-");

            PlayerPrefs.SetString("OwnedBoats", ownedBoatsStr);
            PlayerPrefs.Save();

            UpdateUI();
        }
        else
        {
            Debug.Log("No Money :(");
        }
    }

    public void SpawnBoat(int boatIndex)
    {
        if (currentBoat != null)
            Destroy(currentBoat);

        Debug.Log("SpawningBoat");
        currentBoat = Instantiate(boatPrefabs[boatIndex], boatSpawnLocation[boatIndex].position, boatSpawnLocation[boatIndex].rotation);
    }
}

