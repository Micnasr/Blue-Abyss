using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public GameObject[] fishPrefabs;
    public List<Transform> Waypoints = new List<Transform>();

    private GameObject currentFish;
    private bool isRespawning = false; // Flag to indicate if a fish is currently being respawned

    public float respawnDelay = 10f;
    public float spawnDistanceThreshold = 20f;

    private Camera playerCam;
    private GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerCam = player.GetComponentInChildren<Camera>();

        GetWaypoints();
        SpawnFish();
    }

    private void Update()
    {
        // Check if the current fish is dead and not already respawning
        if (currentFish == null && !isRespawning)
        {
            StartCoroutine(RespawnFish());
        }
    }

    void GetWaypoints()
    {
        Transform[] wpList = this.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < wpList.Length; i++)
        {
            if (wpList[i].tag == "waypoint")
            {
                Waypoints.Add(wpList[i]);
            }
        }
    }

    void SpawnFish()
    {
        int randomIndex = Random.Range(0, fishPrefabs.Length);
        currentFish = Instantiate(fishPrefabs[randomIndex], transform.position, transform.rotation);

        // Set up fish patrol waypoints or other logic
        EnemyPatrol fishPatrol = currentFish.GetComponent<EnemyPatrol>();
        LandEnemyPatrol landFishPatrol = currentFish.GetComponent<LandEnemyPatrol>();

        if (fishPatrol != null)
            fishPatrol.waypoints = Waypoints;
        else
            landFishPatrol.waypoints = Waypoints;
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
