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
    public KeyCode diveKey = KeyCode.C;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;

    [HideInInspector] public bool grounded;

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
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
            SpeedControl();

            // Handle Drag
            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = groundDrag / 2;
        }
        
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
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
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
                    Vector3 spawnPosition = GetFeetPosition();
                    GameObject walkParticleInstance = Instantiate(walkParticlePrefab, spawnPosition, Quaternion.identity);
                    walkParticleInstance.transform.parent = transform;
                }

                walkParticleTimer = 0f;
            }
        }
    }

    private Vector3 GetFeetPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight, whatIsGround))
        {
            return hit.point;
        }
        else
        {
            return transform.position;
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

