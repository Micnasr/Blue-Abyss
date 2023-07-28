using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractUI : MonoBehaviour
{
    private TextMeshProUGUI interactText;

    public bool interactEnabled = false;
    void Start()
    {
        interactText = GetComponentInChildren<TextMeshProUGUI>();
        interactText.gameObject.SetActive(false);
    }

    private void Update()
    {
        interactText.gameObject.SetActive(interactEnabled);
    }
    
    public void InteractWith(string message)
    {
        interactText.text = message;
        interactEnabled = true;
    }

    public void InteractStop()
    {
        interactEnabled = false;
    }
}
