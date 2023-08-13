using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteDetector : MonoBehaviour
{
    public float biteCooldown = 1f;
    public float biteDamage;
    private bool canBite = true;
    private bool isPlayerInside = false;
    private float lastBiteTime;

    private HealthManager healthManager;
    private DeathManager deathManager;

    private EnemyPatrol enemyPatrol;
    private float originalTurningSpeed;
    private bool isIncreasingTurningSpeedCoroutineRunning = false;
    private float maxTurningSpeedIncrease;
    private float turningSpeedIncreaseRate; 

    private void Start()
    {
        healthManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
        deathManager = GameObject.FindGameObjectWithTag("Player").GetComponent<DeathManager>();

        enemyPatrol = transform.parent.GetComponent<EnemyPatrol>();
        originalTurningSpeed = enemyPatrol.turningSpeed;
        maxTurningSpeedIncrease = originalTurningSpeed * 2f;
        turningSpeedIncreaseRate = originalTurningSpeed / 5f;
    }

    private void Update()
    {
        if (!deathManager.isPlayerDead && isPlayerInside && canBite && Time.time - lastBiteTime >= biteCooldown)
        {
            healthManager.TakeDamage(biteDamage);

            // Set the last bite time
            lastBiteTime = Time.time;

            // Reset canBite to prevent repeated biting
            canBite = false;

            // Reset turning speed and flag when bit
            enemyPatrol.turningSpeed = originalTurningSpeed;

            // Start the bite cooldown timer
            StartCoroutine(BiteCooldown());
        }
        else if (deathManager.isPlayerDead)
        {
            if (!canBite)
            {
                canBite = true;
                isPlayerInside = false;
            }
        }

        // Check if in combat and increase turning speed gradually
        if (enemyPatrol.inCombat)
        {
            StartCoroutine(IncreaseTurningSpeed());
        } 
        else if (!enemyPatrol.inCombat)
        {
            enemyPatrol.turningSpeed = originalTurningSpeed;
        }
    }

    private IEnumerator BiteCooldown()
    {
        yield return new WaitForSeconds(biteCooldown);

        // Reset canBite to allow biting again
        canBite = true;
    }

    private IEnumerator IncreaseTurningSpeed()
    {
        // If the coroutine is already running, exit the new coroutine
        if (isIncreasingTurningSpeedCoroutineRunning)
        {
            yield break; 
        }

        isIncreasingTurningSpeedCoroutineRunning = true;

        // Slowly Increase the Turning Speed When In Combat
        while (enemyPatrol.inCombat && enemyPatrol.turningSpeed < maxTurningSpeedIncrease)
        {
            enemyPatrol.turningSpeed += turningSpeedIncreaseRate;
            yield return new WaitForSeconds(2f);
        }

        isIncreasingTurningSpeedCoroutineRunning = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}
