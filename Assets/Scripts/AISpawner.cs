using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AISpawner : MonoBehaviour
{
    public GameObject fishPrefab;
    public List<Transform> Waypoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        GetWaypoints();
        SpawnFish();
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
        GameObject fish = Instantiate(fishPrefab, transform.position, transform.rotation);

        EnemyPatrol fishPatrol = fish.GetComponent<EnemyPatrol>();
        LandEnemyPatrol landFishPatrol = fish.GetComponent<LandEnemyPatrol>();
        
        if (fishPatrol != null )
            fishPatrol.waypoints = Waypoints;
        else
            landFishPatrol.waypoints = Waypoints;
    }
}
