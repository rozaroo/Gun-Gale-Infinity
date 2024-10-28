using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level4Button;
    [SerializeField] private Button backButton;
    [SerializeField] private Button instructionsButton;
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private Button instructionsBackButton;
    [SerializeField] private AudioSource backgroundMusic;
    private bool isMuted = false;
    private void Start()
    {
        ShowCursor();
        InitializeMenu();
        backgroundMusic.Play();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) ToggleMusic();
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
        SceneManager.LoadScene(5);
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
        instructionsButton.gameObject.SetActive(false);
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
        instructionsButton.gameObject.SetActive(true);
        instructionsPanel.SetActive(false);
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
        backButton.gameObject.SetActive(false);
    }
    public void Instructionsbuton()
    {
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        instructionsButton.gameObject.SetActive(false);
        instructionsPanel.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }
    public void Back() 
    {
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        instructionsButton.gameObject.SetActive(true);
        instructionsPanel.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
    private void ToggleMusic()
    {
        isMuted = !isMuted;
        backgroundMusic.mute = isMuted;
    }
}
