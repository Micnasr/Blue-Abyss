using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flocking : MonoBehaviour
{
    [Header("Spawn Setup")]
    [SerializeField] private FlockUnit flockUnitPrefab;
    [SerializeField] private int flockSize;
    [SerializeField] private Vector3 spawnBounds;

    [Header("Speed Setup")]
    [Range(0, 10)]
    [SerializeField] private float _minSpeed;
    public float minSpeed { get { return _minSpeed; } }

    [Range(0, 10)]
    [SerializeField] private float _maxSpeed;
    public float maxSpeed { get { return _maxSpeed; } }

    [Header("Detection Distances")]
    [Range(0, 10)]
    [SerializeField] private float _cohesionDistance;
    public float cohesionDistance { get { return _cohesionDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _avoidanceDistance;
    public float avoidanceDistance { get { return _avoidanceDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _alignmentDistance;
    public float alignmentDistance { get { return _alignmentDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _obstacleDistance;
    public float obstacleDistance { get { return _obstacleDistance; } }

    [Range(0, 100)]
    [SerializeField] private float _boundsDistance;
    public float boundsDistance { get { return _boundsDistance; } }


    [Header("Behaviour Weights")]
    [Range(0, 10)]
    [SerializeField] private float _cohesionWeight;
    public float cohesionWeight { get { return _cohesionWeight; } }

    // By Increasing avoidanceWeight it will disperse the Fish since they will keep more distance when avoiding
    [Range(0, 10)]
    [SerializeField] private float _avoidanceWeight;
    public float avoidanceWeight { get { return _avoidanceWeight; } }

    // By Increasing alignmentWeight it will make the fish go away. If its set to 0 theyll stay in the same area
    [Range(0, 10)]
    [SerializeField] private float _alignmentWeight;
    public float alignmentWeight { get { return _alignmentWeight; } }

    // Bound is how far the fish can swim to
    [Range(0, 10)]
    [SerializeField] private float _boundsWeight;
    public float boundsWeight { get { return _boundsWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _obstacleWeight;
    public float obstacleWeight { get { return _obstacleWeight; } }




    public FlockUnit[] allUnits { get; set; }
    private Camera playerCam;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerCam = player.GetComponentInChildren<Camera>();

        GenerateUnits();
    }

    private void Update()
    {
        CheckNULL();
        CheckInView();
    }

    private void CheckInView()
    {
        foreach (var unit in allUnits)
        {
            if (unit != null)
            {
                bool isVisible = unit.PlayerInRange(50f) && IsUnitVisible(unit);

                // Disable or enable the unit's GameObject based on visibility
                unit.gameObject.SetActive(isVisible);
            }
        }
    }

    private bool IsUnitVisible(FlockUnit unit)
    {
        SkinnedMeshRenderer skinnedMeshRenderer = unit.GetComponentInChildren<SkinnedMeshRenderer>();

        if (skinnedMeshRenderer != null)
        {
            // Check if any part of the SkinnedMeshRenderer is visibleby any camera
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(playerCam);
            Bounds bounds = skinnedMeshRenderer.bounds;
            return GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
        }

        // If the SkinnedMeshRenderer is not found, assume it's not visible
        return false;
    }

    private void CheckNULL()
    {
        List<FlockUnit> unitList = new List<FlockUnit>(allUnits);

        // Iterate over the list and remove null elements
        unitList.RemoveAll(unit => unit == null);

        // Convert the list back to an array
        allUnits = unitList.ToArray();
    }

    private void GenerateUnits()
    {
        allUnits = new FlockUnit[flockSize];
        for (int i = 0; i < flockSize; i++)
        {
            var randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector = new Vector3(randomVector.x * spawnBounds.x, randomVector.y * spawnBounds.y, randomVector.z * spawnBounds.z);
            var spawnPosition = transform.position + randomVector;
            var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

            allUnits[i] = Instantiate(flockUnitPrefab, spawnPosition, rotation);
            allUnits[i].AssignFlock(this);
            allUnits[i].InitializeSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));
        }
    }
}
