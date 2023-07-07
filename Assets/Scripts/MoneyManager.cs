using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney;
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        // READ FROM FILE AFTER
        currentMoney = 0;
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateUI();

    }

    public void RemoveMoney(int amount)
    {
        currentMoney -= amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        moneyText.text = "$" + currentMoney.ToString();
    }
}
