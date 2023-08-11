using System.Collections;
using UnityEngine;
using TMPro;

public class DepthMeter : MonoBehaviour
{
    private Transform player;
    private TextMeshProUGUI depthText;

    private float initialYPosition = -4.8f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        depthText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        // Calculate the depth based on the player's Y position
        float depth = Mathf.Abs((player.position.y - initialYPosition) * -2.0f);

        // Update the depth text
        depthText.text = depth.ToString("F1") + "m";
    }
}
