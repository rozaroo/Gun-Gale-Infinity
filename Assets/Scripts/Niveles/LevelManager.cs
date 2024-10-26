using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public int Enemies;
    [SerializeField] private GameObject Portal;
    [SerializeField] private int Nivel;
    [SerializeField] GameObject defeatScreen;
    [SerializeField] TMP_Text enemiesCountText;
    PlayerController pController;
    [SerializeField] private AudioSource backgroundMusic;
    private bool isMuted = false;
    private void Awake()
    {
        pController = GameObject.FindObjectOfType<PlayerController>();
        if (pController == null) Debug.LogError("No ha sido encontrado");
    }

    void Start()
    {
        Portal.SetActive(false);
        defeatScreen.SetActive(false);
        Enemies = FindObjectsOfType<EnemyController>().Length + FindObjectsOfType<EnemyControllerTwo>().Length ;
        UpdateEnemiesText();
        //backgroundMusic.Play();
        ToggleMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemies == 0) Portal.SetActive(true);
        if (pController != null) 
        {
            if (pController.Active != null)
            {
                if (pController.Active == false) Lose();
                //else Debug.Log("es null");
            }
        }
        UpdateEnemiesText();
        if (Input.GetKeyDown(KeyCode.M)) ToggleMusic();
        if (Input.GetKeyDown(KeyCode.Escape)) MainMenu();
    }
        
    public void Restart()
    {
        SceneManager.LoadScene(Nivel);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Lose()
    {
        defeatScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void DecreaseEnemyCount()
    {
        if (Enemies  > 0)
        {
            Enemies--;
            UpdateEnemiesText();
        }
    }
    void UpdateEnemiesText()
    {
        enemiesCountText.text = "Enemigos Restantes: " + Enemies;
    }
    private void ToggleMusic()
    {
        isMuted = !isMuted;
        backgroundMusic.mute = isMuted;
    }
}
