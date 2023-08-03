using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public Quest quest;
    private GameObject player;

    public GameObject questPanel;
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI rewardText;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        questPanel.SetActive(false);
    }

    public void QuestWindowOpen()
    {
        questPanel.SetActive(true);

        questNameText.text = quest.title;
        descriptionText.text = quest.description;

        // progress text

        rewardText.text = "+$" + quest.reward.ToString();
    }
}
