using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    private Transform player;
    private Transform playerCam;

    private string questFor;

    private QuestController questController;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCam = player.GetComponentInChildren<PlayerCam>().transform;

        questController = FindObjectOfType<QuestController>();

        gameObject.SetActive(false);

        questFor = "";

        // Find the Quests Its for
        for (int i = 0; i < questController.quests.Length; i++)
        {
            for (int j = 0; j < questController.quests[i].goal.itemTargets.Length; j++)
            {
                if (gameObject == questController.quests[i].goal.itemTargets[j])
                    questFor = questController.quests[i].title;
            }
        }

        if (questFor == "")
            Debug.LogError("Cannot Find Item's Quest");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CollectItem();
        }

        // Despawn if Quest is Changed
        if (questController.currentQuest == null || questController.currentQuest.title != questFor)
            gameObject.SetActive(false);
    }

    private void CollectItem()
    {
        // Raycast forward to check if there's an item to collect
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 3f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                FindObjectOfType<AudioManager>().Play("CollectibleFound");
                questController.ItemCollected();
                FindAnyObjectByType<InteractUI>().InteractStop();
                gameObject.SetActive(false);
            }
        }
    }
}
