using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney;
    public TextMeshProUGUI moneyText;

    private void Awake()
    {
        // Load the money data from player preferences
        currentMoney = PlayerPrefs.GetInt("Money", 0);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateUI();

        // Save the updated money data to player preferences
        PlayerPrefs.SetInt("Money", currentMoney);
        PlayerPrefs.Save();
    }

    public void RemoveMoney(int amount)
    {
        currentMoney -= amount;
        UpdateUI();

        // Save the updated money data to player preferences
        PlayerPrefs.SetInt("Money", currentMoney);
        PlayerPrefs.Save();
    }

    private void UpdateUI()
    {
        moneyText.text = "$" + currentMoney.ToString();
    }
}