using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class EnemyPatrol : MonoBehaviour
{
    public List<Transform> waypoints;
    private Transform currentWaypoint;
    private bool isCheckingWaypoint = false;

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
    public float chaseMovementSpeed;
    public LayerMask landLayer;

    [Header("Performance")]
    public float animatorDistance = 90f;
    private Animator animator;

    private Transform player;
    private OxygenController oxygenController;

    [Header("Sounds")]
    public string[] fishSounds;

    private void Start()
    {
        SetRandomWaypoint();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        oxygenController = player.GetComponent<OxygenController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isAggressive)
        {
            // Check if Player is In the Range and not on land
            if (PlayerInRange(aggressiveDistance) && !IsPlayerOnLand() && !oxygenController.inSubmarine)
            {
                // Chase Player Down
                MoveTowardsPlayer();
            }
            else
            {
                if (ReachedWaypoint())
                { 
                    PlayFishSound();
                    SetRandomWaypoint();
                }

                MoveTowardsWaypoint();
            }
        }
        else
        {
            if (ReachedWaypoint())
            {
                PlayFishSound();
                SetRandomWaypoint();
            }

            MoveTowardsWaypoint();
        }

        PerformanceAnimations();
    }

    private void SetRandomWaypoint()
    {
        int randomIndex = Random.Range(0, waypoints.Count);
        currentWaypoint = waypoints[randomIndex];

        if (isCheckingWaypoint)
        {
            StopCoroutine(CheckWaypointReached());
        }

        StartCoroutine(CheckWaypointReached());
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
            }
        }

        if (transform.position.y >= surfaceLevel)
        {
            targetPosition.y = Mathf.Clamp(targetPosition.y, -Mathf.Infinity, surfaceLevel);
        }

        // Rotate towards the target position
        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turningSpeed * 2f);

        if (PlayerInRange(stopDistance))
        {
            // Fish is close enough to the player, stop moving
            return;
        }

        // Move towards the target position
        transform.position += transform.forward * Time.deltaTime * chaseMovementSpeed;

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

    private bool IsPlayerOnLand()
    {
        return Physics.Raycast(player.transform.position, Vector3.down, 20f, landLayer);
    }

    private void PerformanceAnimations()
    {
        if (animator != null)
        {
            if (!PlayerInRange(animatorDistance))
                animator.enabled = false;
            else
                animator.enabled = true;
        }
    }

    private void PlayFishSound()
    {
        if (fishSounds.Length == 0)
            return;

        // % Chance to Play the Audio (To Avoid Spam)
        if (Random.Range(0, 5) != 1)
            return;
 
        int randomIndex = Random.Range(0, fishSounds.Length);

        float randomPitch = Random.Range(0.95f, 1.05f);
        FindObjectOfType<AudioManager>().Play(fishSounds[randomIndex], randomPitch, gameObject);
    }
    
    // Function Helps with Fish Not Getting Stuck on Waypoints
    private IEnumerator CheckWaypointReached()
    {
        isCheckingWaypoint = true;
        GameObject currentCheck = currentWaypoint.gameObject;
        yield return new WaitForSeconds(50f);

        if (currentWaypoint.gameObject == currentCheck)
        {
            SetRandomWaypoint();
        }
        else
        {
            isCheckingWaypoint = false;
        }
    }
}
