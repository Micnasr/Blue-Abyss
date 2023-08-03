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
    
    private GameObject player;

    public GameObject questPanel;
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI rewardText;

    //[HideInInspector]
    public Quest currentQuest;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        questPanel.SetActive(false);

        currentQuest = null;
    }

    public void StartQuest(string questName)
    {
        Quest quest = ReturnQuest(questName);

        if (quest == null)
            Debug.LogError("Could not find quest with name: " + questName);

        currentQuest = quest;
        questPanel.SetActive(true);

        UpdateQuestDisplay();
    }

    private void UpdateQuestDisplay()
    {  
        questNameText.text = currentQuest.title;
        descriptionText.text = currentQuest.description;

        progressText.text = currentQuest.goal.currentAmount.ToString() + '/' + currentQuest.goal.requiredAmount.ToString();

        rewardText.text = "+$" + currentQuest.reward.ToString();
    }

    public Quest ReturnQuest(string questName)
    {
        return Array.Find(quests, quest => quest.title == questName);
    }

    public void KillProgress(GameObject animal)
    {
        // Not on a Quest or Wrong Type
        if (currentQuest.reward == 0 || (currentQuest.goal.goalType != GoalType.Kill))
            return;

        // Get the name of the animal GameObject and the name of the kill target
        string animalName = animal.name.Replace("(Clone)", "").Trim();
        string killTargetName = currentQuest.goal.killTarget.name;

        if (animalName == killTargetName)
        {
            currentQuest.goal.currentAmount++;
            UpdateQuestDisplay();

            // Check if Quest Completed
            currentQuest.isCompleted = currentQuest.goal.IsReached();

        }
    }
}
