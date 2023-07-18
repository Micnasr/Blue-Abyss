using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackUI : MonoBehaviour
{
    public GameObject backgroundPanel;
    public KeyCode openKey = KeyCode.B;

    // Seperate Items by "-"
    public string itemsUnlockedStr;

    // Store Prefab Order
    private List<int> itemsUnlocked = new List<int>();

    public Transform[] positions;
    public GameObject[] prefabs;

    private bool openUI = false;

    void Start()
    {
        ConvertStrArray();
        GenerateUI();
    }

    private void ConvertStrArray()
    {
        string[] strArray = itemsUnlockedStr.Split('-');

        for (int i = 0; i < strArray.Length; i++)
        {
            if (strArray[i] == "Anchor")
            {
                itemsUnlocked.Add(0);
            } 
            else if (strArray[i] == "Lifevest")
            {
                itemsUnlocked.Add(1);
            }
        }

    }

    private void GenerateUI()
    {
        for (int i = 0; i < itemsUnlocked.Count; i++)
        {
            GameObject instantiatedPrefab = Instantiate(prefabs[itemsUnlocked[i]], positions[i].position, positions[i].rotation);
            instantiatedPrefab.transform.SetParent(backgroundPanel.transform);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(openKey))
        {
            openUI = !openUI;

            if (openUI)
               OpenUpgradeUI();
            else
                CloseUpgradeUI();
            
        }
    }

    private void CloseUpgradeUI()
    {
        backgroundPanel.SetActive(false);
    }

    private void OpenUpgradeUI()
    {
        backgroundPanel.SetActive(true);
    }
}
