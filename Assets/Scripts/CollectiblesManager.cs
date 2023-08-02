using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectiblesManager : MonoBehaviour
{
    [HideInInspector] public string collectedStatus;
    private int totalCollectibles;

    public GameObject collectibleUI;
    private TextMeshProUGUI collectibleText;

    private BackpackUI backpackUI;

    [HideInInspector] public List<GameObject> collectibleItems = new List<GameObject>();

    private void Awake()
    {
        // Determine the number of collectibles based on the number of child GameObjects
        totalCollectibles = transform.childCount;

        collectibleText = collectibleUI.GetComponentInChildren<TextMeshProUGUI>();
        backpackUI = FindAnyObjectByType<BackpackUI>();

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

        LoadUI();
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

            LoadUI();

            if (!backpackUI.openUI)
                StartCoroutine(FadeOutText());
        }
        else
        {
            Debug.LogError("Index Out Of Bounds");
        }
    }

    private IEnumerator FadeOutText()
    {
        // Set the text gameobject to true
        collectibleUI.SetActive(true);

        // Fade out the text gradually
        float duration = 1f;
        float elapsedTime = 0f;
        Color startColor = collectibleText.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            collectibleText.color = Color.Lerp(startColor, targetColor, t);
            elapsedTime += Time.deltaTime;

            if (backpackUI.openUI)
            {
                collectibleText.color = startColor;
                yield break;
            }
                
            yield return null;
        }

        collectibleText.color = targetColor;
        collectibleUI.SetActive(false);

        collectibleText.color = startColor;
    }


    private void LoadUI()
    {
        int foundItems = 0;

        for (int i = 0; i < totalCollectibles; i++)
            if (collectedStatus[i] == '1')
                    foundItems++;

        collectibleText.text = (foundItems.ToString() + "/" + totalCollectibles + " Treasure Found");

    }
}

