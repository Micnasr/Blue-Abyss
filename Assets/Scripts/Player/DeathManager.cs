using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    public Transform playerSpawner;
    private PlayerMovement playerMovement;
    private OxygenController oxygenController;
    private Rigidbody playerRigidbody;
    private HealthManager healthManager;
    private PlayerShoot playerShoot;
    private WeaponSway weaponSway;
    private PlayerCam playerCam;
    private FishMeter fishMeter;

    public Image healthImage;

    public bool isPlayerDead = false;

    public void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        oxygenController = GetComponent<OxygenController>();
        playerRigidbody = GetComponent<Rigidbody>();
        healthManager = GetComponent<HealthManager>();
        playerShoot = GetComponent<PlayerShoot>();
        weaponSway = GetComponentInChildren<WeaponSway>();
        playerCam = GetComponentInChildren<PlayerCam>();
        fishMeter = FindAnyObjectByType<FishMeter>();
    }

    public void PlayerDied()
    {
        if (!isPlayerDead)
        {
            isPlayerDead = true;

            // Stop time for 2 seconds
            StartCoroutine(StopTimeForDelay(3f));
        }
    }

    private IEnumerator StopTimeForDelay(float delay)
    {
        PlayerScriptsState(false);

        RedUI();

        Time.timeScale = 0.2f;

        yield return new WaitForSecondsRealtime(delay);

        TeleportPlayer(playerSpawner.position);

        // Reset Bag
        fishMeter.ResetFishDeaths();

        PlayerScriptsState(true);

        // Player is On The Surface
        playerMovement.isSwimming = false;
        oxygenController.isHeadAboveWater = true;

        // Reset
        healthManager.ResetHealth();
        oxygenController.ResetOxygen();

        // Reset time scale even if there was an issue with the coroutine
        Time.timeScale = 1f;

        isPlayerDead = false;
    }

    private void PlayerScriptsState(bool active)
    {
        playerMovement.enabled = active;
        oxygenController.enabled = active;
        healthManager.enabled = active;
        playerShoot.enabled = active;
        weaponSway.enabled = active;
        playerCam.enabled = active;
    }

    private void RedUI()
    {
        Color newColor = healthImage.color;
        newColor.a = 0.3f;
        healthImage.color = newColor;
    }

    private void TeleportPlayer(Vector3 newPosition)
    {
        // Move the player using Rigidbody's MovePosition method
        playerRigidbody.MovePosition(newPosition);
    }
}
