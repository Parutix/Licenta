using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject controlsCanvas;
    private void Start()
    {
        mainMenuCanvas.SetActive(true);
        controlsCanvas.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void ShowControlsPage()
    {
        mainMenuCanvas.SetActive(false);
        controlsCanvas.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        controlsCanvas.SetActive(false);
    }
}
