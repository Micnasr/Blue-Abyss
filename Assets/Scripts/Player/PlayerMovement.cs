using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    [Header("Land Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Water Movement")]
    public float swimSpeed;


    public Transform target;
    public bool isSwimming;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizonInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        isSwimming = false;
    }

    private void FixedUpdate()
    {
        if (!isSwimming)
        {
            MovePlayer();
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

    private void MovePlayer()
    {
        // If we are on the ground then gravity should be on
        if (rb.useGravity == false)
        {
            rb.useGravity = true;
        }

        moveDirection = orientation.forward * verticalInput + orientation.right * horizonInput;

        // On ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // If in the air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
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
        //Disable Gravity in Water
        if (rb.useGravity == true)
        {
            rb.useGravity = false;
        }

        moveDirection = orientation.forward * verticalInput + orientation.right * horizonInput;

        // Apply swim movement force
        rb.AddForce(moveDirection.normalized * swimSpeed * 10f, ForceMode.Force);

        // Check for swim up input to move towards the surface
        if (Input.GetKey(jumpKey))
        {
            SwimUp();
        }

        // Check for dive input to move downwards
        if (Input.GetKey(KeyCode.C))
        {
            SwimDown();
        }
    }

    private void SwimUp()
    {
        // Move the player upwards
        Vector3 swimUpForce = transform.up * swimSpeed;
        rb.AddForce(swimUpForce, ForceMode.Acceleration);
    }

    private void SwimDown()
    {
        // Move the player downwards
        Vector3 swimDownForce = -transform.up * swimSpeed;
        rb.AddForce(swimDownForce, ForceMode.Acceleration);
    }


}

