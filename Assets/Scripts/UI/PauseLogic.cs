using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseLogic : MonoBehaviour
{
    public bool pauseMenuOpen = false;
    public GameObject pauseMenuUI;

    public KeyCode pauseKey = KeyCode.N; //todo change to esc

    private CursorLockMode previousCursorLockMode;
    private bool previousCursorVisibility;
    //private bool previousPlayerMovementState;
    private bool previousPlayerShootState;
    private bool previousPlayerSwayState;
    private bool previousPlayerCam;

    private GameObject player;
    //private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    private PlayerCam playerCam;
    private WeaponSway weaponSway;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //playerMovement = player.GetComponent<PlayerMovement>();
        playerShoot = player.GetComponent<PlayerShoot>();
        weaponSway = player.GetComponentInChildren<WeaponSway>();
        playerCam = player.GetComponentInChildren<PlayerCam>();
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (!pauseMenuOpen)
                OpenPauseMenu();
            else
                ClosePauseMenu();
        }
    }

    private void OpenPauseMenu()
    {
        //previousPlayerMovementState = playerMovement.enabled;
        previousPlayerCam = playerCam.enabled;
        previousPlayerShootState = playerShoot.enabled;
        previousPlayerSwayState = weaponSway.enabled;

        PlayerScriptsState(false);

        // Store the previous states
        previousCursorLockMode = Cursor.lockState;
        previousCursorVisibility = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenuUI.SetActive(true);
        pauseMenuOpen = true;
    }

    public void ClosePauseMenu()
    {
        Debug.Log("CLOSE");

        //playerMovement.enabled = previousPlayerMovementState;
        playerShoot.enabled = previousPlayerShootState;
        weaponSway.enabled = previousPlayerSwayState;
        playerCam.enabled = previousPlayerCam;

        Cursor.lockState = previousCursorLockMode;
        Cursor.visible = previousCursorVisibility;

        pauseMenuUI.SetActive(false);
        pauseMenuOpen = false;
    }


    private void PlayerScriptsState(bool active)
    {
        //playerMovement.enabled = active;
        playerShoot.enabled = active;
        weaponSway.enabled = active;
        playerCam.enabled = active;
    }
}
