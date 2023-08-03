using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;
    public string description;
    public int reward;

    public QuestGoal goal;
    public bool isCompleted;
}
