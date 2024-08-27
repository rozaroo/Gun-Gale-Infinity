using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public Button tutorialButton;
    public Button level1Button;
    public Button level2Button;
    public Button level4Button;
    public Button backButton;
    public Button instructionsbutton;
    public GameObject instructionsPanel;
    public Button backButton2;

    private void Start()
    {
        ShowCursor();
        InitializeMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadLevel0()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadLevel4()
    {
        SceneManager.LoadScene(4);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ShowGameOptions()
    {
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        instructionsbutton.gameObject.SetActive(false);
        tutorialButton.gameObject.SetActive(true);
        level1Button.gameObject.SetActive(true);
        level2Button.gameObject.SetActive(true);
        level4Button.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void ShowMainMenu()
    {
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        instructionsbutton.gameObject.SetActive(true);
        instructionsPanel.gameObject.SetActive(false);
        tutorialButton.gameObject.SetActive(false);
        level1Button.gameObject.SetActive(false);
        level2Button.gameObject.SetActive(false);
        level4Button.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
    private void InitializeMenu()
    {
        tutorialButton.gameObject.SetActive(false);
        level1Button.gameObject.SetActive(false);
        level2Button.gameObject.SetActive(false);
        level4Button.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        instructionsPanel.gameObject.SetActive(false);
        backButton2.gameObject.SetActive(false);
    }
    public void Instructionsbuton()
    {
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        instructionsbutton.gameObject.SetActive(false);
        instructionsPanel.gameObject.SetActive(true);
        backButton2.gameObject.SetActive(true);
    }
    public void Back() 
    {
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        instructionsbutton.gameObject.SetActive(true);
        instructionsPanel.gameObject.SetActive(false);
        backButton2.gameObject.SetActive(false);
    }
}
