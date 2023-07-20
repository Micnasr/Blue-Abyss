using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifevestLogic : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private Rigidbody rb;

    private AnchorLogic anchorLogic;

    [HideInInspector] public bool toggledON = false;

    [Header("Item Data")]
    public float LifevestFloatStrength;

    [Header("Keybind")]
    public KeyCode toggleKey;

    [Header("State Colors")]
    public Color onColor;
    public Color offColor;

    private Image image;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject anchorUI = GameObject.FindGameObjectWithTag("Anchor");

        if (anchorUI != null)
        {
            anchorLogic = anchorUI.GetComponent<AnchorLogic>();
        }

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

            // If Vest is On we want Anchor to Be OFF
            if (toggledON && anchorLogic != null)
            {
                anchorLogic.toggledON = false;
            }
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
            Vector3 upForce = transform.up * LifevestFloatStrength;
            rb.AddForce(upForce, ForceMode.Acceleration);
        }
    }
}
