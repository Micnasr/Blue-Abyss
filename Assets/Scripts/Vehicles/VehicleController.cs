using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public Transform driverSeat;
    public Transform exitLocation;

    public KeyCode interactKey = KeyCode.E;

    [Header("Vehicle Stats")]
    public float moveSpeed;
    public float rotationSpeed;
    private bool isDriving = false;
    
    public bool isSubmarine = false;
    public KeyCode diveKey = KeyCode.LeftControl;
    public KeyCode riseKey = KeyCode.Space;

    public float RiseDiveSpeed;
    public float LowestDepth;
    private float HighestDepth;

    private Transform player;
    private Rigidbody playerRigidbody;
    private Rigidbody boatRigidbody;
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    private PlayerCam playerCam;
    private WeaponSway weaponSway;
    private OxygenController oxygenController;

    private bool isTeleporting = false;

    private GameObject weaponHolster;
    private CapsuleCollider playerMainCollider;

    public ParticleSystem boatParticleSystem;

    [Header("AddOn")]
    public bool hasHeadlights;
    public GameObject leftHeadlight;
    public GameObject rightHeadlight;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRigidbody = player.GetComponent<Rigidbody>();
        oxygenController = player.GetComponent<OxygenController>();

        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        playerShoot = player.gameObject.GetComponent<PlayerShoot>();
        playerCam = player.gameObject.GetComponentInChildren<PlayerCam>();
        weaponSway = player.gameObject.GetComponentInChildren<WeaponSway>();
        weaponHolster = weaponSway.gameObject;
        playerMainCollider = player.gameObject.GetComponentInChildren<CapsuleCollider>();

        boatRigidbody = GetComponent<Rigidbody>();
        if (boatRigidbody == null)
        {
            boatRigidbody = gameObject.AddComponent<Rigidbody>();
            boatRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        if (isSubmarine)
            HighestDepth = this.transform.position.y;
    }

    private void FixedUpdate()
    {
        if (isDriving)
        {
            HandleDriving();

            // If It is a submarine :) ~ Michael
            if (isSubmarine)
                HandleSubmarine();
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
                return;
            }
        }

        FX_WaterParticles();
        ToggleHeadlights();
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
        weaponHolster.SetActive(false);
        playerMainCollider.enabled = false;

        // Move the player to the driver seat position and rotate them to face the boat's forward direction
        player.position = driverSeat.position;
        player.rotation = driverSeat.rotation;
        player.SetParent(driverSeat);

        isDriving = true;
        
        if (isSubmarine)
            oxygenController.inSubmarine = true;
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

    private bool atHighestDepth = false;
    private bool atLowestDepth = false;

    private void HandleSubmarine()
    {
        // Handle diving
        if (Input.GetKey(diveKey) && transform.position.y >= LowestDepth)
        {
            Vector3 forwardForce = -transform.up * RiseDiveSpeed;
            boatRigidbody.AddForce(forwardForce, ForceMode.Force);
        }

        // Handle rising
        if (Input.GetKey(riseKey) && transform.position.y <= HighestDepth)
        {
            Vector3 forwardForce = transform.up * RiseDiveSpeed;
            boatRigidbody.AddForce(forwardForce, ForceMode.Force);
        }

        // Check if the submarine is at its highest or lowest depth
        atHighestDepth = transform.position.y >= HighestDepth;
        atLowestDepth = transform.position.y <= LowestDepth;

        Vector3 newVelocity = boatRigidbody.velocity;

        // Prevent further movement if the submarine is at a boundary (either up or down)
        if (atHighestDepth && boatRigidbody.velocity.y > 0f)
        {
            newVelocity.y = 0f;
            boatRigidbody.velocity = newVelocity;
        }
        else if (atLowestDepth && boatRigidbody.velocity.y < 0f)
        {
            newVelocity.y = 0f;
            boatRigidbody.velocity = newVelocity;
        }
    }


    private void ExitVehicle()
    {
        if (isTeleporting)
            return;

        isTeleporting = true;
        StartCoroutine(DelayedExitVehicle());
        isTeleporting = false;
    }

    private IEnumerator DelayedExitVehicle()
    {
        // Wait for physics calculations to finish
        yield return new WaitForFixedUpdate();

        // Move the player to the exit location and reset their rotation
        player.position = exitLocation.position;
        player.rotation = exitLocation.rotation;
        player.SetParent(null);

        // Re-enable scripts and components
        playerMovement.enabled = true;
        playerShoot.enabled = true;
        weaponSway.enabled = true;
        playerRigidbody.isKinematic = false;
        playerMainCollider.enabled = true;
        weaponHolster.SetActive(true);

        isDriving = false;

        if (isSubmarine)
            oxygenController.inSubmarine = false;
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

    private void ToggleHeadlights()
    {
        if (!hasHeadlights)
            return;

        if (atHighestDepth || oxygenController.isHeadAboveWater)
        {
            leftHeadlight.SetActive(false);
            rightHeadlight.SetActive(false);
        }
        else
        {
            leftHeadlight.SetActive(true);
            rightHeadlight.SetActive(true);
        }
    }

}
