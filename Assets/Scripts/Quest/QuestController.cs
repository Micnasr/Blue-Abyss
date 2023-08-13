using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public Quest[] quests;

    public GameObject openQuestPanel;
    public GameObject closedQuestPanel;
    private bool openPanel = false;
    public KeyCode toggleKey = KeyCode.Tab;

    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI rewardText;

    private MoneyManager moneyManager;

    [HideInInspector] public int finishedQuests = 0;

    [HideInInspector] public string completedQuests;

    public Quest currentQuest;

    private void Awake()
    {
        LoadQuestCompleted();

        // Sets Each Quest as Complete or Not
        for (int i = 0; i < quests.Length; i++)
            quests[i].isCompleted = completedQuests[i] == '1';

        for (int i = 0; i < quests.Length; i++)
            if (quests[i].isCompleted)
                finishedQuests++;
    }

    private void Start()
    {
        closedQuestPanel.SetActive(true);
        openQuestPanel.SetActive(false);

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

    private void LoadQuestCompleted()
    {
        string initialStr = string.Empty;
        for (int i = 0; i < quests.Length; i++)
            initialStr += 0;

        completedQuests = PlayerPrefs.GetString("Quests", initialStr);
    }

    public void UpdateCompletedQuest(string name)
    {
        int index = -1;
        for (int i = 0; i < quests.Length; i++)
            if (quests[i].title == name)
                index = i;

        if (index == -1)
            Debug.LogError("Could Not Find Quest: " + name);

        char[] completedArray = completedQuests.ToCharArray();
        completedArray[index] = '1';
        completedQuests = new string(completedArray);

        // Save the updated collected status to PlayerPrefs
        PlayerPrefs.SetString("Quests", completedQuests);
        PlayerPrefs.Save();
    }

    private void OpenQuestPanel()
    {
        openQuestPanel.SetActive(true);
        closedQuestPanel.SetActive(false);
        openPanel = true;
    }

    private void CloseQuestPanel()
    {
        openQuestPanel.SetActive(false);
        closedQuestPanel.SetActive(true);
        openPanel = false;
    }

    public void StartQuest(string questName)
    {
        Quest quest = ReturnQuest(questName);

        // Reset Progress
        quest.goal.currentAmount = 0;

        if (quest.goal.goalType == GoalType.Gathering)
        {
            foreach(GameObject item in quest.goal.itemTargets)
            {
                item.SetActive(true);
            }
        }

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

        progressText.text = currentQuest.goal.currentAmount.ToString() + " / " + currentQuest.goal.requiredAmount.ToString();

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
        if (currentQuest == null || currentQuest.isCompleted || currentQuest.reward == 0 || (currentQuest.goal.goalType != GoalType.Kill))
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
        finishedQuests++;
        moneyManager.AddMoney(currentQuest.reward);
        UpdateCompletedQuest(currentQuest.title);
        EmptyQuest();
        CloseQuestPanel();
    }

    private void EmptyQuest()
    {
        EmptyQuestDisplay();
        currentQuest = null;
    }

    public void ItemCollected()
    {
        currentQuest.goal.currentAmount++;
        UpdateQuestDisplay();
        OpenQuestPanel();

        // Check if Quest Completed
        currentQuest.isCompleted = currentQuest.goal.IsReached();
    }
}
