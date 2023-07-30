using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InteractObject : MonoBehaviour
{
    public string interactMessage;
    public float interactionDistance = 5f;

    private Transform player;
    private PlayerMovement playerMovement;
    private PlayerCam playerCam;

    private bool interactedOn = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        playerCam = player.gameObject.GetComponentInChildren<PlayerCam>();
    }

    void Update()
    {
        // Handle Interact UI Render
        if (!interactedOn && (Vector3.Distance(playerCam.transform.position, transform.position) <= interactionDistance) && LookingAtObject())
        {
            FindAnyObjectByType<InteractUI>().InteractWith(interactMessage);
            interactedOn = true;
        }
        else if (interactedOn && (Vector3.Distance(playerCam.transform.position, transform.position) >= interactionDistance || !LookingAtObject()))
        {
            FindAnyObjectByType<InteractUI>().InteractStop();
            interactedOn = false;
        }
    }

    private bool LookingAtObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 5f))
        {
            if (hit.collider != null && hit.collider.gameObject != null)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
