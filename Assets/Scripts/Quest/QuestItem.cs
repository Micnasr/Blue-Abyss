using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    private Transform player;
    private Transform playerCam;

    private QuestController questController;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCam = player.GetComponentInChildren<PlayerCam>().transform;

        questController = FindObjectOfType<QuestController>();

        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        // Raycast forward to check if there's an item to collect
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 3f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("Collected Quest Item");
                questController.ItemCollected();
                FindAnyObjectByType<InteractUI>().InteractStop();
                gameObject.SetActive(false);
            }
        }
    }
}
