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

    private void Start()
    {
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

        // Call SpawnFish() again to respawn the fish
        SpawnFish();

        // Reset the flag
        isRespawning = false;
    }
}
