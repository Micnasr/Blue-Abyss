using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    [Header("Land Movement")]
    public float moveSpeed;
    public float gravity;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Water Movement")]
    public float swimSpeed;
    public bool isSwimming;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode diveKey = KeyCode.LeftControl;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public Transform playerFeet;

    [HideInInspector] public bool grounded;
    private bool wasGrounded = false;

    public Transform orientation;

    float horizonInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    OxygenController oxygenController;

    [Header("ParticleEffects")]
    public GameObject jumpParticlePrefab;
    public GameObject walkParticlePrefab;

    private float walkParticleSpawnRate = 0.1f;
    private float walkParticleTimer = 0f;

    [Header("Sound Effects")]
    public string[] walking_sounds_sand;
    public string woodTag;
    public string[] walking_sounds_wood;
    public float walkSoundSpawnRate = 0.1f;
    private float walkSoundTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        isSwimming = false;

        oxygenController = GetComponent<OxygenController>();
    }

    private void FixedUpdate()
    {
        if (!isSwimming)
        {
            MoveGround();
        }
        else
        {
            MoveSwim();
        }
    }

    private void Update()
    {
        myInput();

        // Check if we are on the ground
        if (!isSwimming)
        {
            grounded = IsGrounded(); // New Jump
            //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // Old Jump

            // Check if we landed on the ground
            if (wasGrounded == false && grounded == true)
            {
                SpawnJumpParticle();
            }

            wasGrounded = grounded;
            SpeedControl();

            // Handle Drag
            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = groundDrag / 2;
        }
        
    }

    public bool IsGrounded()
    {
        float sphereRadius = 0.4f;
        Vector3 sphereCenter = playerFeet.position;
        Collider[] groundColliders = Physics.OverlapSphere(sphereCenter, sphereRadius, whatIsGround);

        return groundColliders.Length > 0;
    }

    private void myInput()
    {
        horizonInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Handle Jump Logic
        if (Input.GetKey(jumpKey) && readyToJump && grounded && !isSwimming)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    private void MoveGround()
    {
        ApplyCustomGravity();

        moveDirection = orientation.forward * verticalInput + orientation.right * horizonInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

            SpawnWalkParticle();

            if (rb.velocity.magnitude >= 0.001f)
            {
                // Sound Depending on Surface
                if (GetSurfaceType() == woodTag)
                    SpawnWalkSoundEffects(walking_sounds_wood);
                else
                    SpawnWalkSoundEffects(walking_sounds_sand);
            }
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpawnJumpParticle()
    {
        if (walkParticlePrefab != null)
        {
            Vector3 spawnPosition = new Vector3(playerFeet.position.x, playerFeet.position.y - 0.3f, playerFeet.position.z);
            GameObject jumpParticleInstance = Instantiate(jumpParticlePrefab, spawnPosition, Quaternion.identity);
            jumpParticleInstance.transform.parent = transform;
        }
    }

    private void SpawnWalkParticle()
    {
        // Check if Player is Moving
        if (Mathf.Abs(horizonInput) > 0f || Mathf.Abs(verticalInput) > 0f)
        {
            walkParticleTimer += Time.deltaTime;

            if (walkParticleTimer >= walkParticleSpawnRate)
            {
                if (walkParticlePrefab != null)
                {
                    Vector3 spawnPosition = playerFeet.position;
                    GameObject walkParticleInstance = Instantiate(walkParticlePrefab, spawnPosition, Quaternion.identity);
                    walkParticleInstance.transform.parent = transform;
                }

                walkParticleTimer = 0f;
            }
        }
    }

    private void SpawnWalkSoundEffects(string[] soundList)
    {
        // Check if Player is Moving
        if (Mathf.Abs(horizonInput) > 0f || Mathf.Abs(verticalInput) > 0f)
        {
            walkSoundTimer += Time.deltaTime;

            if (walkSoundTimer >= walkSoundSpawnRate)
            {
                if (soundList.Length == 0)
                    return;
               
                int randomIndex = Random.Range(0, soundList.Length);

                //float randomPitch = Random.Range(0.95f, 1.05f);
                FindObjectOfType<AudioManager>().Play(soundList[randomIndex]);

                walkSoundTimer = 0f;
            }
        }
    }

    public string GetSurfaceType()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight, whatIsGround))
        {
            return hit.collider.gameObject.tag;
        }
        else
        {
            return "";
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit Speed if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void MoveSwim()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizonInput;

        // Apply swim movement force
        rb.AddForce(moveDirection.normalized * swimSpeed * 10f, ForceMode.Force);

        // Check for swim up input to move towards the surface
        if (Input.GetKey(jumpKey))
        {
            SwimUp();
        }

        // Check for dive input to move downwards
        if (Input.GetKey(diveKey))
        {
            SwimDown();
        }
    }

    private void SwimUp()
    {
        // Move the player upwards
        Vector3 swimUpForce;

        // If the player's head is above the water, we want to jump further up
        if (oxygenController.isHeadAboveWater)
        {
            swimUpForce = transform.up * swimSpeed / 1.5f;
        }
        else
        {
            swimUpForce = transform.up * swimSpeed / 4f;
        }


        rb.AddForce(swimUpForce, ForceMode.Acceleration);

    }

    private void SwimDown()
    {
        // Move the player downwards
        Vector3 swimDownForce = -transform.up * swimSpeed/7f;
        rb.AddForce(swimDownForce, ForceMode.Acceleration);
    }

    private void ApplyCustomGravity()
    {
        Vector3 gravityForce = -transform.up * gravity / 2;
        rb.AddForce(gravityForce, ForceMode.Acceleration);
    }
}

