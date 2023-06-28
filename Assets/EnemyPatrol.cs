using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public List<Transform> waypoints;
    private Transform currentWaypoint;

    public float turningSpeed;
    public float movementSpeed;
    public float threshold;

    public float raycastDistance;
    public float avoidanceDistance;

    public LayerMask obstacleLayer;

    private void Start()
    {
        SetRandomWaypoint();
    }

    private void Update()
    {
        // Check if the fish has reached the current waypoint
        if (ReachedWaypoint())
        {
            SetRandomWaypoint();
        }

        // Move towards the current waypoint
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

        RaycastHit hit;
        bool obstacleDetected = Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, obstacleLayer);

        if (obstacleDetected)
        {
            // Perform a 360-degree rotation while scanning for an empty spot ~MN PATENTED :)
            float angleStep = 10f;
            float maxAngle = 360f;
            float currentAngle = 0f;
            bool foundEmptySpot = false;

            while (currentAngle < maxAngle)
            {
                // Calculate the next rotation angle
                float angle = currentAngle * Mathf.Deg2Rad;
                Vector3 rotationVector = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
                Vector3 rotatedDirection = Quaternion.AngleAxis(currentAngle, transform.up) * direction;

                // Perform a raycast in the rotated direction
                if (!Physics.Raycast(transform.position, rotatedDirection, raycastDistance, obstacleLayer))
                {
                    // Found an empty spot to navigate through
                    targetPosition = transform.position + rotatedDirection * raycastDistance;
                    foundEmptySpot = true;
                    break;
                }

                // Increment the current angle
                currentAngle += angleStep;
            }

            // If no empty spot was found, fallback to avoidance behavior
            if (!foundEmptySpot)
            {
                Vector3 avoidanceDirection = Vector3.Cross(transform.up, direction.normalized).normalized;
                targetPosition = transform.position + avoidanceDirection * avoidanceDistance;
            }
        }

        // Rotate towards the target position
        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turningSpeed);

        // Move towards the target position
        transform.position += transform.forward * Time.deltaTime * movementSpeed;
    }
}