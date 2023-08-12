using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetalDetectorLogic : MonoBehaviour
{
    private GameObject player;

    private bool toggledON = false;
    private bool isBeeping = false;

    private CollectiblesManager collectiblesManager;

    [Header("Item Data")]
    public string BeepSound;

    [Header("Keybind")]
    public KeyCode toggleKey;

    [Header("State Colors")]
    public Color onColor;
    public Color offColor;

    private Image image;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        image = GetComponent<Image>();

        collectiblesManager = FindAnyObjectByType<CollectiblesManager>();
    }

    void Update()
    {
        // Toggle item if key is pressed
        if (Input.GetKeyDown(toggleKey))
        {
            toggledON = !toggledON;
        }

        if (toggledON)
        {
            ItemON();
            image.color = onColor;
        }
        else
        {
            image.color = offColor;
            if (isBeeping)
                StopBeeping();
        }
    }

    private void ItemON()
    {
        float maxDistance = 20f;
        bool shouldPlayBeep = false;

        // If a Collectible is Nearby, Play a Sound
        foreach (var collectible in collectiblesManager.collectibleItems)
        {
            if (collectible == null || !collectible.activeSelf)
                continue;

            float distance = Vector3.Distance(player.transform.position, collectible.transform.position);
            if (distance <= maxDistance)
            {
                shouldPlayBeep = true;
                break;
            }
        }

        if (shouldPlayBeep && !isBeeping)
        {
            PlayBeeping();
        }
        else if (!shouldPlayBeep && isBeeping)
        {
            StopBeeping();
        }
    }

    private void PlayBeeping()
    {
        isBeeping = true;
        FindObjectOfType<AudioManager>().Play(BeepSound, 1.5f);
    }

    private void StopBeeping()
    {
        isBeeping = false;
        FindObjectOfType<AudioManager>().StopPlaying(BeepSound);
    }
}
