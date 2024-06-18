using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int Enemies;
    public GameObject Portal;
    public int Nivel;
    [SerializeField] GameObject defeatScreen;
    PlayerController pController;

    private void Awake()
    {
        pController = GameObject.FindObjectOfType<PlayerController>();
        if (pController == null) Debug.LogError("No ha sido encontrado");
    }

    void Start()
    {
        Portal.SetActive(false);
        defeatScreen.SetActive(false);
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
}
