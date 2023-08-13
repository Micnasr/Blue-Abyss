using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    // All Dependent Scripts
    public Transform playerSpawner;
    private PlayerMovement playerMovement;
    private OxygenController oxygenController;
    private Rigidbody playerRigidbody;
    private HealthManager healthManager;
    private PlayerShoot playerShoot;
    private WeaponSway weaponSway;
    private PlayerCam playerCam;
    private FishMeter fishMeter;
    private Dialogue dialogue;

    public GameObject crosshair;

    public Transform deathTitleSpawner;
    public GameObject deathPrefab;

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
        dialogue = FindAnyObjectByType<Dialogue>();
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
        // Stop Conversation If We Die While Talking
        if (dialogue.currentNPC != null)
            dialogue.currentNPC.CloseDialogue();

        PlayerScriptsState(false);

        RedUI();
        ShowDeathText();

        Time.timeScale = 0.2f;

        yield return new WaitForSecondsRealtime(delay);

        VehicleController vehicleController = FindAnyObjectByType<VehicleController>();
        if (vehicleController != null)
            vehicleController.PlayerDied();

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

    private void ShowDeathText()
    {
        GameObject deathUIInstance = Instantiate(deathPrefab, deathTitleSpawner.position, deathTitleSpawner.rotation);
        deathUIInstance.transform.SetParent(deathTitleSpawner);
        StartCoroutine(DestroyAfterAnimation(0.7f, deathUIInstance));
    }

    private IEnumerator DestroyAfterAnimation(float duration, GameObject objToDestroy)
    {
        yield return new WaitForSeconds(duration);
        Destroy(objToDestroy);
    }

    private void PlayerScriptsState(bool active)
    {
        playerMovement.enabled = active;
        oxygenController.enabled = active;
        healthManager.enabled = active;
        playerShoot.enabled = active;
        weaponSway.enabled = active;
        playerCam.enabled = active;

        crosshair.SetActive(active);
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
