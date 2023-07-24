using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public Transform driverSeat;
    public Transform exitLocation;

    public KeyCode interactKey = KeyCode.E;

    [Header("Vehicle Stats")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private bool isDriving = false;

    private Transform player;
    private Rigidbody playerRigidbody;
    private Rigidbody boatRigidbody;
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    private PlayerCam playerCam;
    private WeaponSway weaponSway;

    public ParticleSystem boatParticleSystem;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRigidbody = player.GetComponent<Rigidbody>();

        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        playerShoot = player.gameObject.GetComponent<PlayerShoot>();
        playerCam = player.gameObject.GetComponentInChildren<PlayerCam>();
        weaponSway = player.gameObject.GetComponentInChildren<WeaponSway>();

        boatRigidbody = GetComponent<Rigidbody>();
        if (boatRigidbody == null)
        {
            boatRigidbody = gameObject.AddComponent<Rigidbody>();
            boatRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void FixedUpdate()
    {
        if (isDriving)
        {
            HandleDriving();
        }
    }

    private void Update()
    {
        if (!isDriving)
        {
            HandleLookingAtVehicle();
        }
        else
        {
            // Check if the player presses the interact key (E) to exit the vehicle
            if (Input.GetKeyDown(KeyCode.E))
            {
                ExitVehicle();
                Debug.Log("Exiting Vehicle");
                return;
            }
        }

        FX_WaterParticles();
    }

private void HandleLookingAtVehicle()
    {
        // Check if the player is looking at the vehicle and presses the interact key (E)
        if (Input.GetKeyDown(interactKey) && CanEnterVehicle())
        {
            Debug.Log("Entering Vehicle");
            EnterVehicle();
        }
    }

    private bool CanEnterVehicle()
    {
        RaycastHit hit;
        float maxInteractDistance = 5f;

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, maxInteractDistance))
        {
            // Check if the raycast hits the vehicle's collider
            if (hit.collider.gameObject.transform.parent.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private void EnterVehicle()
    {
        // Disable Player Scripts
        playerMovement.enabled = false;
        playerShoot.enabled = false;
        weaponSway.enabled = false;

        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.isKinematic = true;

        // Move the player to the driver seat position and rotate them to face the boat's forward direction
        player.position = driverSeat.position;
        player.rotation = driverSeat.rotation;
        player.SetParent(driverSeat);

        isDriving = true;
    }

    private void HandleDriving()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float rotationInput = Input.GetAxis("Horizontal");

        Vector3 rotationTorque = Vector3.up * rotationInput * rotationSpeed;
        boatRigidbody.AddTorque(rotationTorque, ForceMode.Force);

        // Move the boat forward or backward
        Vector3 forwardForce = transform.forward * forwardInput * moveSpeed;
        boatRigidbody.AddForce(forwardForce, ForceMode.Force);
    }


    private void ExitVehicle()
    {
        // Enable player scripts
        playerMovement.enabled = true;
        playerShoot.enabled = true;
        weaponSway.enabled = true;
        playerRigidbody.isKinematic = false;

        // Teleport the player to the exit location and reset their rotation
        player.position = exitLocation.position;
        player.rotation = exitLocation.rotation;
        player.SetParent(null);

        isDriving = false;
    }

    private void FX_WaterParticles()
    {
        float maxVelocity = 6.7f;
        float minRateOverTime = 0f;
        float maxRateOverTime = 400f;

        // Calculate the rate over time based on the boat's velocity
        float velocityPercent = Mathf.Clamp01(boatRigidbody.velocity.magnitude / maxVelocity);
        float rateOverTime = Mathf.Lerp(minRateOverTime, maxRateOverTime, velocityPercent);

        // Set the rate over time in the emission property of the particle system
        var emission = boatParticleSystem.emission;
        emission.rateOverTime = rateOverTime;

        // Start/stop the boat particle effect based on the boat's velocity
        if (boatRigidbody.velocity.magnitude > 0.2f) // Adjust the threshold as needed
        {
            if (!boatParticleSystem.isPlaying)
            {
                boatParticleSystem.Play();
            }
        }
        else
        {
            if (boatParticleSystem.isPlaying)
            {
                boatParticleSystem.Stop();
            }
        }
    }

}
