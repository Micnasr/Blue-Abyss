using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    private Transform player;
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    private PlayerCam playerCam;
    private WeaponSway weaponSway;

    private QuestController questController;

    public bool isWaterNPC = false;

    [Header("NPC Quest")]
    public string questName;
    private Quest npcQuest;
    private bool gaveReward;

    public int levelRequirement = 0;

    private Dialogue dialogueManager;

    [Header("Interaction Stats")]
    public float interactionDistance = 5f;
    public float rotationSpeed = 5f;

    public KeyCode interactKey = KeyCode.E;

    // For The Hover Text
    public string interactMessage;
    private bool interactedOn = false;

    private bool talkingWithPlayer = false;

    [Header("Text Lines")]
    public string[] lines;

    [Header("Progress Lines")]
    public string[] progressLines;

    [Header("Completed Lines")]
    public string[] completedLines;

    private string[] underLevelLines = { "Hello, wanderer! While I'm glad to see you exploring our shores, you're still new here.",
                                        "Gain more experience and knowledge of our realm, then return, and I'll gladly offer you a suitable quest." };

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        playerShoot = player.gameObject.GetComponent<PlayerShoot>();
        playerCam = player.gameObject.GetComponentInChildren<PlayerCam>();
        weaponSway = player.gameObject.GetComponentInChildren<WeaponSway>();

        dialogueManager = FindAnyObjectByType<Dialogue>();
        questController = FindAnyObjectByType<QuestController>();

        npcQuest = questController.ReturnQuest(questName);
        gaveReward = false;
    }

    private void Start()
    {
        if (npcQuest.isCompleted)
            gaveReward = true;
    }

    private void Update()
    {
        // Check if the player is near the NPC
        if (Vector3.Distance(playerCam.transform.position, transform.position) <= interactionDistance)
        {
            RotateNPC();

            // Check if we can talk to NPC
            if (Input.GetKeyDown(interactKey) && (playerMovement.grounded || isWaterNPC) && LookingAtNPC())
            {
                if (!talkingWithPlayer)
                {
                    InteractWith();
                }
            }
        }
        
        // Handle Interact UI Render
        if (!interactedOn && (Vector3.Distance(playerCam.transform.position, transform.position) <= interactionDistance) && LookingAtNPC() && !talkingWithPlayer && (playerMovement.grounded || isWaterNPC))
        {
            FindAnyObjectByType<InteractUI>().InteractWith(interactMessage);
            interactedOn = true;
        }
        else if (interactedOn && (!(playerMovement.grounded || isWaterNPC) || talkingWithPlayer || Vector3.Distance(playerCam.transform.position, transform.position) >= interactionDistance || !LookingAtNPC()))
        {
            FindAnyObjectByType<InteractUI>().InteractStop();
            interactedOn = false;
        }
    }

    private bool LookingAtNPC()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 5f))
        {
            if (hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.transform.parent != null)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void CloseDialogue()
    {
        talkingWithPlayer = false;

        dialogueManager.CloseDialogue();
        
        playerMovement.enabled = true;
        playerShoot.enabled = true;
        playerCam.enabled = true;
        weaponSway.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void InteractWith()
    {
        talkingWithPlayer = true;

        if (questController.finishedQuests < levelRequirement)
            dialogueManager.StartDialogue(underLevelLines, this);

        else if (npcQuest.isCompleted)
        {
            dialogueManager.StartDialogue(completedLines, this);

            if (!gaveReward)
            {
                questController.QuestCompleted();
                gaveReward = true;
            }
        } 
        else if (questController.currentQuest != null && questController.currentQuest.title == questName)
        {
            dialogueManager.StartDialogue(progressLines, this);
        } 
        else
        {
            dialogueManager.StartDialogue(lines, this, true);
        }

        playerMovement.enabled = false;
        playerShoot.enabled = false;
        playerCam.enabled = false;
        weaponSway.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void RotateNPC()
    {
        // Calculate the direction from NPC to player
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        // Smoothly rotate the NPC towards the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void AcceptQuest()
    {
        questController.StartQuest(questName);
        CloseDialogue();
    }

    public void DeclineQuest()
    {
        CloseDialogue();
    }
}
