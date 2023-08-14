using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDepthManager : MonoBehaviour
{
    private bool reachedDeepZone = false;
    private bool reachedAbyssZone = false;

    public float deepZoneY = -60;
    public float abyssZoneY = -400;

    private string shallowMusic = "ShallowWaterMusic";
    private string deepMusic = "DeepWaterMusic";
    private string abyssMusic = "AbyssMusic";


    private Transform player;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        // Handle Music Depending On Player Depth >:)

        if (player != null)
        {
            float playerY = player.position.y;

            if (playerY <= deepZoneY && !reachedDeepZone && !reachedAbyssZone)
            {
                FindObjectOfType<AudioManager>().FadeTrack(deepMusic, 1f);
                reachedDeepZone = true;
            }
            else if (playerY <= abyssZoneY && !reachedAbyssZone && reachedDeepZone)
            {
                FindObjectOfType<AudioManager>().FadeTrack(abyssMusic, 1f);
                reachedAbyssZone = true;
            }
            else if (playerY > abyssZoneY && reachedAbyssZone)
            {
                FindObjectOfType<AudioManager>().FadeTrack(deepMusic, 1f);
                reachedAbyssZone = false;
            }
            else if (playerY > deepZoneY && reachedDeepZone)
            {
                FindObjectOfType<AudioManager>().FadeTrack(shallowMusic, 1f);
                reachedDeepZone = false;
            }
        }
    }
}
