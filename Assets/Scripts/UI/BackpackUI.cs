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

    private List<GameObject> instantiatedObjects = new List<GameObject>();

    private void Awake()
    {
        itemsUnlockedStr = PlayerPrefs.GetString("Backpack", "");
    }

    void Start()
    {
        ConvertStrArray();
        GenerateUI();
    }

    private void ConvertStrArray()
    {
        itemsUnlocked.Clear();
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
            else if (strArray[i] == "Light")
            {
                itemsUnlocked.Add(2);
            }
            else if (strArray[i] == "Jacket")
            {
                itemsUnlocked.Add(3);
            }
        }

    }

    private void GenerateUI()
    {
        float posX = 0f;
        float width = 0f;

        switch (itemsUnlocked.Count)
        {
            case 0:
                posX = -27.87619f;
                width = 39.9209f;
                break;
            case 1:
                posX = -11.36689f;
                width = 72.981f;
                break;
            case 2:
                posX = 4.6226f;
                width = 104.96f;
                break;
            case 3:
                posX = 21.2f;
                width = 138.11f;
                break;
            // Add more cases for other counts as needed
            default:
                posX = 37.5f;
                width = 170.82f;
                break;
        }

        // Destroy previously instantiated objects
        foreach (GameObject obj in instantiatedObjects)
        {
            Destroy(obj);
        }

        instantiatedObjects.Clear();

        RectTransform rectTransform = backgroundPanel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(posX, rectTransform.anchoredPosition.y);
        rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);

        for (int i = 0; i < itemsUnlocked.Count; i++)
        {
            GameObject instantiatedPrefab = Instantiate(prefabs[itemsUnlocked[i]], positions[i].position, positions[i].rotation);
            instantiatedPrefab.transform.SetParent(backgroundPanel.transform);
            instantiatedObjects.Add(instantiatedPrefab);
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

    public void BoughtItem(string item)
    {
        if (itemsUnlocked.Count == 0)
            itemsUnlockedStr = item;
        else
            itemsUnlockedStr += ("-" + item);

        PlayerPrefs.SetString("Backpack", itemsUnlockedStr);
        PlayerPrefs.Save();

        ConvertStrArray();
        GenerateUI();
    }
}
