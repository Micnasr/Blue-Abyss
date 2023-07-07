using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishButcherLogic : MonoBehaviour
{
    public Transform player;
    public Transform npc;
    public float interactionDistance = 5f;
    public float rotationSpeed = 5f;

    private void Update()
    {
        // Check if the player is near the NPC
        if (Vector3.Distance(player.position, npc.position) <= interactionDistance)
        {
            // Calculate the direction from NPC to player
            Vector3 directionToPlayer = player.position - npc.position;
            directionToPlayer.y = 0f;

            // Smoothly rotate the NPC towards the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            npc.rotation = Quaternion.Lerp(npc.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Debug.Log("PRESS E");

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Player interacted with the NPC!");
            }
        }
    }
}

