using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishButcherLogic : MonoBehaviour
{
    private Transform player;
    public Transform npc;
    public float interactionDistance = 5f;
    public float rotationSpeed = 5f;

    private FishMeter fishMeter;
    private MoneyManager moneyManager;

    public int moneyMultiplier = 50;

    private Animator npcAnimator;

    private void Start()
    {
        fishMeter = FindObjectOfType<FishMeter>();
        moneyManager = FindObjectOfType<MoneyManager>();

        npcAnimator = npc.gameObject.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Check if the player is near the NPC
        if (Vector3.Distance(player.position, npc.position) <= interactionDistance)
        {
            RotateNPC();

            if (Input.GetKeyDown(KeyCode.E))
            {
                int money = fishMeter.currentCount * moneyMultiplier;

                if (money > 0)
                {
                    moneyManager.AddMoney(money);
                    npcAnimator.SetTrigger("WaveTrigger");
                }

                // Reset Bag
                fishMeter.ResetFishDeaths();
            }
        }
    }

    private void RotateNPC()
    {
        // Calculate the direction from NPC to player
        Vector3 directionToPlayer = player.position - npc.position;
        directionToPlayer.y = 0f;

        // Smoothly rotate the NPC towards the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        npc.rotation = Quaternion.Lerp(npc.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

