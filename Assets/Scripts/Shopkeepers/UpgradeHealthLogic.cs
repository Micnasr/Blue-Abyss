using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UpgradeHealthLogic : MonoBehaviour
{
    public float[] healthStages;
    public float[] prices;

    public Image[] arrows;

    private int maxStage;

    public float arrowTransparency;

    public TextMeshProUGUI differenceText;
    public TextMeshProUGUI priceText;

    private GameObject player;
    private HealthManager healthManager;

    public Color CompletedArrow;

    // Current Upgrade Stage (0-4)
    public int nextStage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        healthManager = player.GetComponent<HealthManager>();

        maxStage = healthStages.Length;

        // Make sure both arrays are the same size
        if (healthStages.Length != prices.Length)
            Debug.LogError("Price and Element Size UNEQUAL!");

        nextStage = FigureOutCurrentStage();
        UpdateUI();
    }

    private int FigureOutCurrentStage()
    {
        float maxHP = healthManager.maxHealth;

        for (int i = 0; i < healthStages.Length; i++)
        {
            if (maxHP == healthStages[i])
            {
                return i + 1;
            }
        }

        return 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AdvanceStage();
        }
    }

    public void UpdateUI()
    {
        // Update the User Interface Accordingly
        if (nextStage < maxStage)
        {
            // Update Difference Text
            differenceText.text = healthManager.maxHealth.ToString() + " -> " + healthStages[nextStage].ToString();

            // Update Price
            priceText.text = "$" + prices[nextStage].ToString();
        }
        // Completed Screen
        else
        {
            priceText.text = "COMPLETED";
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
            // Update Max Health
            healthManager.NewMaxHealth(healthStages[nextStage]);
            PlayerPrefs.SetFloat("MaxHealth", healthStages[nextStage]);
            PlayerPrefs.Save();
            
            nextStage++;
            UpdateUI();
        }
    }

    public void BuyUpgrade()
    {
        AdvanceStage();
    }
}
