using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoatSellerLogic : MonoBehaviour
{
    // This code is a lot cleaner than the one in ItemShopLogic, I came a long way ~MN
    [Header("Price Of Items: Dinghy -> MiniSub")]
    string[] names = { "Dinghy", "MiniSub", "Titan" };
    public int[] priceOfItems;

    private string ownedBoatsStr;

    public GameObject[] boatPrefabs;
    public Transform[] boatSpawnLocation;
    private GameObject currentBoat;

    private MoneyManager moneyManager;

    private QuestController questController;
    private bool scientistQuestDone = false;

    [Header("Page Data")]  
    public GameObject[] fullNames;
    public GameObject[] fullPics;
    public GameObject[] description;
    public TextMeshProUGUI[] textPriceOfItems;
    public GameObject[] purchaseButtons;
    public GameObject[] spawnButtons;
    private int openPage = 0;

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
        questController = FindObjectOfType<QuestController>();

        UpdateUI();
    }

    private void Update()
    {
        UpdatePriceColor();

        // Check if ProfessorX's quest is completed
        if (questController.completedQuests[5] == '1')
            scientistQuestDone = true;
    }

    private void UpdateUI()
    { 
        for (int i = 0; i < names.Length; i++)
        {
            bool state = false;
            if (i == openPage)
            {
                state = true;
            }

            fullNames[i].SetActive(state);
            fullPics[i].SetActive(state);
            description[i].SetActive(state);
            textPriceOfItems[i].gameObject.SetActive(state);
            purchaseButtons[i].SetActive(state);

            if (!state)
            {
                purchaseButtons[i].SetActive(state);
                spawnButtons[i].SetActive(state);
            }
            else
            {
                if (ownedBoatsStr.Contains(names[i]))
                {
                    textPriceOfItems[i].text = "OWNED";
                    spawnButtons[i].SetActive(true);
                    purchaseButtons[i].SetActive(false);
                }
                else
                {
                    if (i != 2)
                    {
                        textPriceOfItems[i].text = "$" + priceOfItems[i].ToString();
                        spawnButtons[i].SetActive(false);
                    }
                    else
                    {
                        purchaseButtons[i].SetActive(scientistQuestDone);
                        textPriceOfItems[i].text = "$" + priceOfItems[i].ToString();
                        textPriceOfItems[i].gameObject.SetActive(scientistQuestDone);
                    }
                   
                }
            }
        }
    }

    public void UpdatePage(int page)
    {
        openPage = page;
        UpdateUI();
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
            FindObjectOfType<AudioManager>().Play("UIClick");
            moneyManager.RemoveMoney(priceOfItems[boatIndex]);

            ownedBoatsStr += (names[boatIndex] + "-");

            PlayerPrefs.SetString("OwnedBoats", ownedBoatsStr);
            PlayerPrefs.Save();

            UpdateUI();
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("UIError");
        }
    }

    public void SpawnBoat(int boatIndex)
    {
        if (currentBoat != null)
            Destroy(currentBoat);

        FindObjectOfType<AudioManager>().Play("UIClick");
        currentBoat = Instantiate(boatPrefabs[boatIndex], boatSpawnLocation[boatIndex].position, boatSpawnLocation[boatIndex].rotation);
    }
}