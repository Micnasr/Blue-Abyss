using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    [HideInInspector] public string collectedStatus;
    private int totalCollectibles;

    [HideInInspector] public List<GameObject> collectibleItems = new List<GameObject>();

    private void Awake()
    {
        // Determine the number of collectibles based on the number of child GameObjects
        totalCollectibles = transform.childCount;

        // Load the collected status from PlayerPrefs
        LoadCollectedStatus();

        // Spawn the collectibles based on the collected status
        for (int i = 0; i < totalCollectibles; i++)
        {
            GameObject collectible = transform.GetChild(i).gameObject;
            bool isCollected = collectedStatus[i] == '1';
            collectible.SetActive(!isCollected);

            collectibleItems.Add(collectible);
        }
    }

    private void LoadCollectedStatus()
    {
        string initialStr = string.Empty;
        for (int i = 0; i < totalCollectibles; i++)
            initialStr += 0;
            
        collectedStatus = PlayerPrefs.GetString("CollectedStatus", initialStr);
    }

    // Method to save the collected status to PlayerPrefs
    public void UpdateCollectedStatus(int index)
    {
        if (index >= 0 && index < collectedStatus.Length)
        {
            char[] collectedArray = collectedStatus.ToCharArray();
            collectedArray[index] = '1';
            collectedStatus = new string(collectedArray);

            // Save the updated collected status to PlayerPrefs
            PlayerPrefs.SetString("CollectedStatus", collectedStatus);
            PlayerPrefs.Save();

        }
        else
        {
            Debug.LogError("Index Out Of Bounds");
        }
    }
}

