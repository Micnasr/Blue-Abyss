using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public List<Transform> waypoints;
    private Transform currentWaypoint;

    [Header("Movement")]
    public float turningSpeed;
    public float movementSpeed;
    public float threshold;

    [Header("Collision Detection")]
    public float angleCheck;
    public float raycastDistance;
    public float avoidanceDistance;

    public LayerMask obstacleLayer;

    [Header("Aggressive Behavior")]
    public bool isAggressive;
    public float aggressiveDistance;
    public float biteTimer;
    public float stopDistance;
    public float surfaceLevel;

    private float timer;
    private Transform player;

    private void Start()
    {
        SetRandomWaypoint();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isAggressive)
        {
            if (PlayerInRange(aggressiveDistance))
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    BitePlayer();
                    timer = biteTimer;
                }

                // Chase Player Down
                MoveTowardsPlayer();
            }
            else
            {
                // Check if the fish has reached the current waypoint
                if (ReachedWaypoint())
                {
                    SetRandomWaypoint();
                }

                // Move towards the current waypoint
                MoveTowardsWaypoint();
            }
        }
        else
        {
            // Check if the fish has reached the current waypoint
            if (ReachedWaypoint())
            {
                SetRandomWaypoint();
            }

            // Move towards the current waypoint
            MoveTowardsWaypoint();
        }
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

        RaycastHit[] hits = new RaycastHit[3];
        bool[] obstacleDetected = new bool[3];

        // Perform raycasts in the middle, left, and right directions
        Vector3[] raycastDirections = { transform.forward, Quaternion.AngleAxis(-angleCheck, transform.up) * transform.forward, Quaternion.AngleAxis(angleCheck, transform.up) * transform.forward };

        // Shoot a raycast in 3 directions
        for (int i = 0; i < raycastDirections.Length; i++)
        {
            obstacleDetected[i] = Physics.Raycast(transform.position, raycastDirections[i], out hits[i], raycastDistance, obstacleLayer);
        }

        // If one of the 3 directions are obstructed
        if (obstacleDetected[0] || obstacleDetected[1] || obstacleDetected[2])
        {
            // Perform a 360-degree rotation while scanning for an empty spot
            float angleStep = 10f;
            float maxAngle = 360f;
            float currentAngle = 0f;
            bool foundEmptySpot = false;

            while (currentAngle < maxAngle)
            {
                // Calculate the next rotation angle
                Vector3 rotatedDirection = Quaternion.AngleAxis(currentAngle, transform.up) * direction;

                // Perform 3 raycasts in the rotated direction
                if (!Physics.Raycast(transform.position, rotatedDirection, raycastDistance, obstacleLayer) &&
                    !Physics.Raycast(transform.position, Quaternion.AngleAxis(-angleCheck, transform.up) * rotatedDirection, raycastDistance, obstacleLayer) &&
                    !Physics.Raycast(transform.position, Quaternion.AngleAxis(angleCheck, transform.up) * rotatedDirection, raycastDistance, obstacleLayer))
                {
                    // Found an empty spot to navigate through
                    targetPosition = transform.position + rotatedDirection * raycastDistance;
                    foundEmptySpot = true;
                    break;
                }

                // Increment the current angle
                currentAngle += angleStep;
            }

            // If no empty spot was found throw an error
            if (!foundEmptySpot)
            {
                Vector3 avoidanceDirection = Vector3.Cross(transform.up, direction.normalized).normalized;
                targetPosition = transform.position + avoidanceDirection * avoidanceDistance;
                Debug.LogError("Stuck in all directions!!!!");
            }
        }

        // Rotate towards the target position
        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turningSpeed);

        // Move towards the target position
        transform.position += transform.forward * Time.deltaTime * movementSpeed;

        // Draw debug rays
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-angleCheck, transform.up) * transform.forward * raycastDistance, Color.green);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(angleCheck, transform.up) * transform.forward * raycastDistance, Color.blue);
    }

    private void MoveTowardsPlayer()
    {
        Vector3 targetPosition = player.transform.position;
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        RaycastHit[] hits = new RaycastHit[3];
        bool[] obstacleDetected = new bool[3];

        // Perform raycasts in the middle, left, and right directions
        Vector3[] raycastDirections = { transform.forward, Quaternion.AngleAxis(-angleCheck, transform.up) * transform.forward, Quaternion.AngleAxis(angleCheck, transform.up) * transform.forward };

        // Shoot a raycast in 3 directions
        for (int i = 0; i < raycastDirections.Length; i++)
        {
            obstacleDetected[i] = Physics.Raycast(transform.position, raycastDirections[i], out hits[i], raycastDistance, obstacleLayer);
        }

        // If one of the 3 directions are obstructed
        if (obstacleDetected[0] || obstacleDetected[1] || obstacleDetected[2])
        {
            // Perform a 360-degree rotation while scanning for an empty spot
            float angleStep = 10f;
            float maxAngle = 360f;
            float currentAngle = 0f;
            bool foundEmptySpot = false;

            while (currentAngle < maxAngle)
            {
                // Calculate the next rotation angle
                Vector3 rotatedDirection = Quaternion.AngleAxis(currentAngle, transform.up) * direction;

                // Perform 3 raycasts in the rotated direction
                if (!Physics.Raycast(transform.position, rotatedDirection, raycastDistance, obstacleLayer) &&
                    !Physics.Raycast(transform.position, Quaternion.AngleAxis(-angleCheck, transform.up) * rotatedDirection, raycastDistance, obstacleLayer) &&
                    !Physics.Raycast(transform.position, Quaternion.AngleAxis(angleCheck, transform.up) * rotatedDirection, raycastDistance, obstacleLayer))
                {
                    // Found an empty spot to navigate through
                    targetPosition = transform.position + rotatedDirection * raycastDistance;
                    foundEmptySpot = true;
                    break;
                }

                // Increment the current angle
                currentAngle += angleStep;
            }

            // If no empty spot was found throw an error
            if (!foundEmptySpot)
            {
                Vector3 avoidanceDirection = Vector3.Cross(transform.up, direction.normalized).normalized;
                targetPosition = transform.position + avoidanceDirection * avoidanceDistance;
                Debug.LogError("Stuck in all directions!!!!");
            }
        }

        if (transform.position.y >= surfaceLevel)
        {
            targetPosition.y = Mathf.Clamp(targetPosition.y, -Mathf.Infinity, surfaceLevel);
        }

        // Rotate towards the target position
        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turningSpeed);

        if (PlayerInRange(stopDistance))
        {
            // Fish is close enough to the player, stop moving
            return;
        }

        // Move towards the target position
        transform.position += transform.forward * Time.deltaTime * movementSpeed;

        // Draw debug rays
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-angleCheck, transform.up) * transform.forward * raycastDistance, Color.green);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(angleCheck, transform.up) * transform.forward * raycastDistance, Color.blue);
    }

    private bool PlayerInRange(float range)
    {
        if (player == null)
        {
            return false;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= range;
    }

    private void BitePlayer()
    {
        Debug.Log("Biting the player!");
        // Add your bite logic here
    }
}
