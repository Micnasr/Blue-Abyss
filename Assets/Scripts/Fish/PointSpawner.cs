using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public GameObject fishPrefab;

    private GameObject currentFish;
    private bool isRespawning = false; // Flag to indicate if a fish is currently being respawned

    public float respawnDelay = 10f;
    public float spawnDistanceThreshold = 20f;

    private Camera playerCam;
    private GameObject player;

    public float despawnRange = 200f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerCam = player.GetComponentInChildren<Camera>();

        SpawnFish();
    }

    private void Update()
    {
        PerformanceRender();

        // Check if the current fish is dead and not already respawning
        if (currentFish == null && !isRespawning)
        {
            StartCoroutine(RespawnFish());
        }
    }

    private void PerformanceRender()
    {
        if (currentFish == null) return;

        Vector3 playerPosition = player.transform.position;
        Vector3 currentFishPosition = currentFish.transform.position;

        // Calculate the distance between the two positions
        float distance = Vector3.Distance(playerPosition, currentFishPosition);

        if (distance > despawnRange)
        {
            currentFish.SetActive(false);
        }
        else
        {
            currentFish.SetActive(true);
        }
    }

    void SpawnFish()
    {
        currentFish = Instantiate(fishPrefab, transform.position, transform.rotation);
    }

    private IEnumerator RespawnFish()
    {
        // Set the flag to prevent multiple respawns
        isRespawning = true;

        // Wait for the respawn delay
        yield return new WaitForSeconds(respawnDelay);

        // Only Spawn If Not Looking Directly Towards and x Away
        if (!IsPlayerLookingAtSpawnPoint(this.gameObject) && !PlayerInRange(spawnDistanceThreshold))
            SpawnFish();

        isRespawning = false;
    }

    private bool IsPlayerLookingAtSpawnPoint(GameObject spawnPoint)
    {
        // Get the player's forward direction
        Vector3 playerForward = playerCam.transform.forward;

        // Calculate the vector from the player to the spawn point
        Vector3 playerToSpawnPoint = spawnPoint.transform.position - playerCam.transform.position;

        // Normalize the vectors
        playerForward.Normalize();
        playerToSpawnPoint.Normalize();

        // Calculate the dot product between the vectors
        float dotProduct = Vector3.Dot(playerForward, playerToSpawnPoint);

        // Define a threshold value (adjust as needed)
        float angleThreshold = 0.8f;

        // Check if the dot product is greater than the threshold
        if (dotProduct > angleThreshold)
        {
            // Player is looking towards the spawn point
            return true;
        }

        // Player is not looking towards the spawn point
        return false;
    }

    public bool PlayerInRange(float range)
    {
        if (player == null)
        {
            return false;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= range;
    }
}
