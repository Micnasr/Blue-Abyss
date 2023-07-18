using System.Collections.Generic;
using UnityEngine;

public class LandEnemyPatrol : MonoBehaviour
{
    public List<Transform> waypoints;
    private Transform currentWaypoint;

    [Header("Movement")]
    public float turningSpeed;
    public float movementSpeed;
    public float threshold;

    [Header("Collision Detection")]
    public float raycastDistance;

    public LayerMask obstacleLayer;

    private void Start()
    {
        SetRandomWaypoint();
    }

    private void Update()
    {  
        if (ReachedWaypoint())
        {
            SetRandomWaypoint();
        }

        MoveTowardsWaypoint();
    }

    private void SetRandomWaypoint()
    {
        int randomIndex = Random.Range(0, waypoints.Count);
        currentWaypoint = waypoints[randomIndex];
    }

    private bool ReachedWaypoint()
    {
        // Calculate the distance between the fish and the current waypoint
        float distance = Vector3.Distance(transform.position, currentWaypoint.position);

        // Check if the distance is below a threshold value (indicating the fish has reached the waypoint)
        return distance < threshold;
    }

    private void MoveTowardsWaypoint()
    {
        Vector3 targetPosition = currentWaypoint.position;
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        // Perform a raycast downwards to detect the ground
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f; // Offset the raycast origin slightly above the character's position
        if (Physics.Raycast(raycastOrigin, -transform.up, out hit, raycastDistance, obstacleLayer))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y; // Set the target position to the height of the ground
            transform.position = pos;
        }

        // Rotate towards the target position
        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turningSpeed);

        transform.position += transform.forward * Time.deltaTime * movementSpeed;

        // Draw debug rays
        Debug.DrawRay(raycastOrigin, -transform.up * raycastDistance, Color.yellow);
    }
}