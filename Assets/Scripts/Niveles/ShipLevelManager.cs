using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipLevelManager : MonoBehaviour
{
    public int Enemies;
    public GameObject Oleada1;
    public GameObject Oleada2;
    public GameObject Oleada3;
    [SerializeField] private TextMeshProUGUI oleadaTMP;
    [SerializeField] private AudioSource backgroundMusic;
    private bool isMuted = false;
    [SerializeField] GameObject defeatScreen;
    SpaceShipController shipController;
    private void Awake()
    {
        shipController = GameObject.FindObjectOfType<SpaceShipController>();
        
    }
    void Start()
    {
        Oleada1.SetActive(true);
        oleadaTMP.text = "Oleada: 1";
        Oleada2.SetActive(false);
        Oleada3.SetActive(false);
        backgroundMusic.Play();
        defeatScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemies == 8) ActivarOleada2();
        if (Enemies == 4) ActivarOleada3();
        if (Enemies == 0) Win();
        if (Input.GetKeyDown(KeyCode.M)) ToggleMusic();
        if (Input.GetKeyDown(KeyCode.Escape)) MainMenu();
        if (shipController == null) Lose();
    }
    public void ActivarOleada2()
    {
        Oleada2.SetActive(true);
        oleadaTMP.text = "Oleada: 2";
    }
    public void ActivarOleada3()
    {
        Oleada3.SetActive(true);
        oleadaTMP.text = "Oleada: 3";
    }
    private void ToggleMusic()
    {
        isMuted = !isMuted;
        backgroundMusic.mute = isMuted;
    }
    public void Restart()
    {
        SceneManager.LoadScene(4);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Win()
    {
        SceneManager.LoadScene(5);
    }
    public void Lose()
    {
        defeatScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
