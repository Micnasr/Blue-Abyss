using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterBiome : MonoBehaviour
{
    public float freezeDamage = 15f;
    private GameObject player;
    private HealthManager healthManager;
    private PlayerMovement playerMovement;

    public GameObject snowParticlePrefab;
    private GameObject currentSnowParticle;
    private BackpackUI backpackUI;

    public GameObject[] winterNPCS;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        healthManager = player.gameObject.GetComponent<HealthManager>();

        backpackUI = FindAnyObjectByType<BackpackUI>();
    }

    void Update()
    {
        // Check if Player Owns Jacket
        bool hasJacket = backpackUI.itemsUnlockedStr.Contains("Jacket");

        if (playerMovement.GetSurfaceType() == "ice-surface")
        {
            // If Player Owns Jacket, Do Not Take Damage
            if (!hasJacket && healthManager != null)
            {
                float damage = freezeDamage * Time.deltaTime;
                healthManager.TakeDamage(damage, 0f);
            }

            // Check if the particle effect is not already active
            if (currentSnowParticle == null)
            {
                // Instantiate the snow particle effect above the player
                Vector3 particlePosition = playerMovement.transform.position + Vector3.up * -3f;
                currentSnowParticle = Instantiate(snowParticlePrefab, particlePosition, Quaternion.identity, player.transform);
            }
        }
        else
        {
            // If the player is not on ice and the particle effect is active, destroy it
            if (currentSnowParticle != null)
            {
                Destroy(currentSnowParticle);
                currentSnowParticle = null;
            }
        }

        // Enable / Disable Npcs Depending On If Player Owns Jacket
        foreach (GameObject NPC in winterNPCS)
        {
            NPCInteract npcInteract = NPC.GetComponent<NPCInteract>();

            if (npcInteract.enabled == !hasJacket)
                npcInteract.enabled = hasJacket;
        }
    }
}
