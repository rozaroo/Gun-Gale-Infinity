using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject levelsButtons;

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ToggleCredits()
    {
        credits.SetActive(!credits.active);
    }
    public void LoadLevel0()
    {
        SceneManager.LoadScene(SceneManager.LoadScene(1));
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene(SceneManager.LoadScene(2));
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene(SceneManager.LoadScene(3));
    }
    public void ToggleLevels()
    { 
        levelsButtons.SetActive(!levelsButtons.active);
    }
}
