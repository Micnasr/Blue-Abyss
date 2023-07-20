using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnchorLogic : MonoBehaviour
{
    private GameObject player;
    private OxygenController oxygenController;
    private PlayerMovement playerMovement;
    private Rigidbody rb;

    private bool toggledON = false;

    [Header("Item Data")]
    public float AnchorDropStrength;

    [Header("Keybind")]
    public KeyCode toggleKey;

    [Header("State Colors")]
    public Color onColor;
    public Color offColor;

    private Image image;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        rb = player.GetComponent<Rigidbody>();

        image = GetComponent<Image>();
        
    }

    
    void Update()
    {
        // Toggle item if key is pressed
        if (Input.GetKeyDown(toggleKey))
        {
            toggledON = !toggledON;
        }

        // Toggles Color Status on Image
        if (toggledON)
        {
            ItemON();
            image.color = onColor;
        }
        else
            image.color = offColor;
    }

    private void ItemON()
    {
        if (playerMovement.isSwimming)
        {
            Vector3 downForce = -transform.up * AnchorDropStrength;
            rb.AddForce(downForce, ForceMode.Acceleration);
        }
    }
}
