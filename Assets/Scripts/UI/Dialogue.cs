using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    private string[] lines;
    public float textSpeed;

    // Quest Related
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject questNamePanel;
    public GameObject rewardPanel;

    private NPCInteract currentNPC;

    private int index;

    void Start()
    {
        gameObject.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        questNamePanel.SetActive(false);
        rewardPanel.SetActive(false);

        textComponent.text = string.Empty;
    }

    void Update()
    {
        // Fast Forward Dialogue
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            } 
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue(string[] texts, NPCInteract npc)
    {
        currentNPC = npc;
        lines = texts;
        index = 0;
        gameObject.SetActive(true);
        textComponent.text = string.Empty;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index >= lines.Length - 1)
            return;

        if (index == lines.Length - 2)
        {
            // Show Quest Accept/Deny Buttons
            yesButton.SetActive(true);
            noButton.SetActive(true);
            questNamePanel.SetActive(true);
            rewardPanel.SetActive(true);
        }

        index++;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    public void CloseDialogue()
    {
        gameObject.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        questNamePanel.SetActive(false);
        rewardPanel.SetActive(false);
    }

    public void AcceptQuest()
    {
        if (currentNPC == null)
            Debug.LogError("CurrentNPC Null");

        currentNPC.AcceptQuest();
    }

    public void DeclineQuest()
    {
        if (currentNPC == null)
            Debug.LogError("CurrentNPC Null");

        currentNPC.DeclineQuest();
    }
}
