using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public GameObject fishPrefab;

    private GameObject fish;

    void Start()
    {
        fish = Instantiate(fishPrefab, transform.position, transform.rotation);
    }
}
