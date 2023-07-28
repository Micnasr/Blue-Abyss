using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShopLogic : MonoBehaviour
{
    private Transform player;
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    private PlayerCam playerCam;
    private WeaponSway weaponSway;

    public Transform npc;
    public float interactionDistance = 5f;
    public float rotationSpeed = 5f;

    public KeyCode interactKey = KeyCode.E;
    
    public string interactMessage;
    private bool interactedOn = false;

    private bool UIOpen = true;

    public GameObject UIPanel;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        playerShoot = player.gameObject.GetComponent<PlayerShoot>();
        playerCam = player.gameObject.GetComponentInChildren<PlayerCam>();
        weaponSway = player.gameObject.GetComponentInChildren<WeaponSway>();

        UIPanel.SetActive(false);
    }

    private void Update()
    {
        // Check if the player is near the NPC
        if (Vector3.Distance(playerCam.transform.position, npc.position) <= interactionDistance)
        {
            RotateNPC();

            if (Input.GetKeyDown(interactKey) && playerMovement.grounded && LookingAtNPC())
            {
                if (UIOpen)
                {
                    OpenUpgradeUI();
                }
                else
                {
                    CloseUpgradeUI();
                }
            }
            else
            {
                if (Input.GetKeyDown(interactKey) && !UIOpen)
                {
                    CloseUpgradeUI();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(interactKey) && !UIOpen)
            {
                CloseUpgradeUI();
            }
        }

        // Handle Interact UI Render
        if (!interactedOn && (Vector3.Distance(playerCam.transform.position, npc.position) <= interactionDistance) && LookingAtNPC() && UIOpen && playerMovement.grounded)
        {
            FindAnyObjectByType<InteractUI>().InteractWith(interactMessage);
            interactedOn = true;

        }
        else if (interactedOn && (!playerMovement.grounded || !UIOpen || Vector3.Distance(playerCam.transform.position, npc.position) >= interactionDistance || !LookingAtNPC()))
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
                if (hit.collider.gameObject.transform.parent.gameObject == gameObject)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void CloseUpgradeUI()
    {
        UIOpen = !UIOpen;

        UIPanel.SetActive(false);

        playerMovement.enabled = true;
        playerShoot.enabled = true;
        playerCam.enabled = true;
        weaponSway.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OpenUpgradeUI()
    {
        UIOpen = !UIOpen;

        UIPanel.SetActive(true);

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
        Vector3 directionToPlayer = player.position - npc.position;
        directionToPlayer.y = 0f;

        // Smoothly rotate the NPC towards the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        npc.rotation = Quaternion.Lerp(npc.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
