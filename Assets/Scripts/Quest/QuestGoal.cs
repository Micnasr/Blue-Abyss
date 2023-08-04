using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;

    public int requiredAmount;
    public int currentAmount;

    public GameObject killTarget;
    public GameObject[] itemTargets;

    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }
}

public enum GoalType
{
    Kill,
    Gathering
}
