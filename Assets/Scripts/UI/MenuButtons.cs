using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject startButton;
    public GameObject exitButton;

    public void StartGame()
    {
        SceneManager.LoadScene("MainMap");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void ShowButtons()
    {
        startButton.SetActive(true);
        exitButton.SetActive(true);
    }
}