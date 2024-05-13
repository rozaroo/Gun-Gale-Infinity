using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int Enemies;
    public GameObject Portal;

    [SerializeField] GameObject defeatScreen;
    PlayerController pController;

    private void Awake()
    {
        pController = GetComponent<PlayerController>();
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
        if (pController.Active == false) defeatScreen.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene());
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.LoadScene(0));
    }
}
