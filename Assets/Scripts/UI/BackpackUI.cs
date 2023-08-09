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

    [HideInInspector] public bool openUI = false;

    private List<GameObject> instantiatedObjects = new List<GameObject>();

    public GameObject itemHolder;
    public GameObject collectibleUI;

    private void Awake()
    {
        itemsUnlockedStr = PlayerPrefs.GetString("Backpack", "");
    }

    void Start()
    {
        backgroundPanel.SetActive(false);
        itemHolder.SetActive(false);
        collectibleUI.SetActive(false);

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
                posX = -10.1f;
                width = -0.46844f;
                break;
            case 1:
                posX = 0.5948f;
                width = 20.9216f;
                break;
            case 2:
                posX = 16.78f;
                width = 53.292f;
                break;
            case 3:
                posX = 32.66911f;
                width = 85.0702f;
                break;
            // Add more cases for other counts as needed
            default:
                posX = 49.23399f;
                width = 118.2f;
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
            instantiatedPrefab.transform.SetParent(itemHolder.transform);

            Vector3 desiredScale = new Vector3(0.4481792f, 0.4481792f, 0.4481792f);
            instantiatedPrefab.transform.localScale = desiredScale;

            instantiatedObjects.Add(instantiatedPrefab);
        }

    }


    void Update()
    {
        if (Input.GetKeyDown(openKey))
        {
            if (!openUI)
               OpenBackpack();
            else
                CloseBackpack();   
        }
    }

    private void CloseBackpack()
    {
        backgroundPanel.SetActive(false);
        itemHolder.SetActive(false);
        collectibleUI.SetActive(false);
        openUI = false;
    }

    public void OpenBackpack()
    {
        backgroundPanel.SetActive(true);
        itemHolder.SetActive(true);
        collectibleUI.SetActive(true);
        openUI = true;
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
