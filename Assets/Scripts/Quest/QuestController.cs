using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public Quest[] quests;

    public GameObject questPanel;
    private bool openPanel = false;
    public KeyCode toggleKey = KeyCode.Tab;

    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI rewardText;

    private MoneyManager moneyManager;

    //[HideInInspector]
    public Quest currentQuest;

    private void Start()
    {
        moneyManager = FindAnyObjectByType<MoneyManager>();
        EmptyQuest();
    }

    private void Update()
    {
        // Handle Opening Quest Panel
        if (Input.GetKeyDown(toggleKey))
        {
            if (!openPanel)
                OpenQuestPanel();
            else
                CloseQuestPanel();
        }
    }

    private void OpenQuestPanel()
    {
        questPanel.SetActive(true);
        openPanel = true;
    }

    private void CloseQuestPanel()
    {
        questPanel.SetActive(false);
        openPanel = false;
    }

    public void StartQuest(string questName)
    {
        Quest quest = ReturnQuest(questName);

        // Reset Progress
        quest.goal.currentAmount = 0;

        if (quest == null)
            Debug.LogError("Could not find quest with name: " + questName);

        currentQuest = quest;

        OpenQuestPanel();

        UpdateQuestDisplay();
    }

    private void UpdateQuestDisplay()
    {
        questNameText.text = currentQuest.title;
        descriptionText.text = currentQuest.description;

        progressText.text = currentQuest.goal.currentAmount.ToString() + '/' + currentQuest.goal.requiredAmount.ToString();

        rewardText.text = "+$" + currentQuest.reward.ToString();
    }

    private void EmptyQuestDisplay()
    {
        questNameText.text = "No Current Quests";
        descriptionText.text = "";
        progressText.text = "";
        rewardText.text = "";
    }

    public Quest ReturnQuest(string questName)
    {
        return Array.Find(quests, quest => quest.title == questName);
    }

    public void KillProgress(GameObject animal)
    {
        // Not on a Quest or Wrong Type
        if (currentQuest == null || currentQuest.reward == 0 || (currentQuest.goal.goalType != GoalType.Kill))
            return;

        // Get the name of the animal GameObject and the name of the kill target
        string animalName = animal.name.Replace("(Clone)", "").Trim();
        string killTargetName = currentQuest.goal.killTarget.name;

        if (animalName == killTargetName)
        {
            currentQuest.goal.currentAmount++;
            UpdateQuestDisplay();
            OpenQuestPanel();

            // Check if Quest Completed
            currentQuest.isCompleted = currentQuest.goal.IsReached();
        }
    }

    public void QuestCompleted()
    {
        Debug.Log("quest completed");
        moneyManager.AddMoney(currentQuest.reward);;
        EmptyQuest();
        CloseQuestPanel();
        
    }

    private void EmptyQuest()
    {
        EmptyQuestDisplay();
        currentQuest = null;
    }
}
