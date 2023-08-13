using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseLogic : MonoBehaviour
{
    public bool pauseMenuOpen = false;
    public GameObject pauseMenuUI;
    public GameObject backdrop;

    public KeyCode pauseKey = KeyCode.Escape;

    private CursorLockMode previousCursorLockMode;
    private bool previousCursorVisibility;
    private bool previousPlayerShootState;
    private bool previousPlayerSwayState;
    private bool previousPlayerCam;

    private GameObject player;
    private PlayerShoot playerShoot;
    private PlayerCam playerCam;
    private WeaponSway weaponSway;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        backdrop.SetActive(true);
        pauseMenuOpen = true;
    }

    public void ClosePauseMenu()
    {
        playerShoot.enabled = previousPlayerShootState;
        weaponSway.enabled = previousPlayerSwayState;
        playerCam.enabled = previousPlayerCam;

        Cursor.lockState = previousCursorLockMode;
        Cursor.visible = previousCursorVisibility;

        pauseMenuUI.SetActive(false);
        backdrop.SetActive(false);
        pauseMenuOpen = false;
    }


    private void PlayerScriptsState(bool active)
    {
        playerShoot.enabled = active;
        weaponSway.enabled = active;
        playerCam.enabled = active;
    }
}
