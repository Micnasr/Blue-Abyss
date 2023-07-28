using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishButcherLogic : MonoBehaviour
{
    private Transform player;
    private PlayerCam playerCam;
    public Transform npc;
    public float interactionDistance = 5f;
    public float rotationSpeed = 5f;

    public KeyCode interactKey = KeyCode.E;

    private FishMeter fishMeter;
    private MoneyManager moneyManager;
    private PlayerMovement playerMovement;

    public string interactMessage;
    private bool interactedOn = false;

    public int moneyMultiplier = 50;

    private Animator npcAnimator;

    private void Start()
    {
        fishMeter = FindObjectOfType<FishMeter>();
        moneyManager = FindObjectOfType<MoneyManager>();

        npcAnimator = npc.gameObject.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        playerCam = player.gameObject.GetComponentInChildren<PlayerCam>();
    }

    private void Update()
    {
        // Check if the player is near the NPC
        if (Vector3.Distance(playerCam.transform.position, npc.position) <= interactionDistance)
        {
            RotateNPC();

            if (Input.GetKeyDown(interactKey) && playerMovement.grounded && LookingAtNPC())
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

        // Handle Interact UI Render
        if (!interactedOn && (Vector3.Distance(playerCam.transform.position, npc.position) <= interactionDistance) && LookingAtNPC() && playerMovement.grounded)
        {
            FindAnyObjectByType<InteractUI>().InteractWith(interactMessage);
            interactedOn = true;

        }
        else if (interactedOn && (!playerMovement.grounded || Vector3.Distance(playerCam.transform.position, npc.position) >= interactionDistance || !LookingAtNPC()))
        {
            FindAnyObjectByType<InteractUI>().InteractStop();
            interactedOn = false;
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

    private bool LookingAtNPC()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 20f))
        {
            if (hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.transform.parent != null)
            {
                if (hit.collider.gameObject.transform.parent.gameObject == gameObject)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

