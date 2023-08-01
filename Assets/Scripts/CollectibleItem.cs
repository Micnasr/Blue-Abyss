using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    private Transform player;
    private Transform playerCam;
    private MoneyManager moneyManager;

    public int moneyReward = 300;

    private int collectibleIndex;
    private CollectiblesManager collectiblesManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCam = player.GetComponentInChildren<PlayerCam>().transform;
        moneyManager = FindAnyObjectByType<MoneyManager>();

        collectiblesManager = FindObjectOfType<CollectiblesManager>();

        collectibleIndex = -1;
        for (int i = 0; i < collectiblesManager.collectibleItems.Count; i++)
            if (collectiblesManager.collectibleItems[i] == gameObject)
                collectibleIndex = i;

        if (collectibleIndex == -1)
            Debug.LogError("Cant Find GameObject");
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
                collectiblesManager.UpdateCollectedStatus(collectibleIndex);
                FindObjectOfType<AudioManager>().Play("CollectibleFound");

                moneyManager.AddMoney(moneyReward);
                FindAnyObjectByType<InteractUI>().InteractStop();

                Destroy(gameObject);
            }
        }
    }
}
