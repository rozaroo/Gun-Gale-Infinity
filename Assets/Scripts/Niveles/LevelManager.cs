using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public int Enemies;
    [SerializeField] private GameObject Portal;
    [SerializeField] GameObject defeatScreen;
    [SerializeField] TMP_Text enemiesCountText;
    PlayerController pController;
    [SerializeField] private AudioSource backgroundMusic;
    private bool isMuted = false;
    private float killStartTime;  // Tiempo cuando matas al primer enemigo
    private bool hasStartedKillTimer = false;
    [SerializeField] private bool hasCardInLevel = false; // Indica si hay una tarjeta en el nivel
    private float cardStartTime; // Tiempo en que empieza la búsqueda de la tarjeta
    private bool isCardTimerRunning = false; // Verifica si el cronómetro está activo

    private void Awake()
    {
        pController = GameObject.FindObjectOfType<PlayerController>();
        if (pController == null) Debug.LogError("No ha sido encontrado");
    }

    void Start()
    {
        if (PersistentGameData.Instance.accumulatedEnemyKillTime == 0f) PersistentGameData.Instance.accumulatedEnemyKillTime = Time.timeSinceLevelLoad;
        if (hasCardInLevel && PersistentGameData.Instance.accumulatedCardTime == 0f)
        {
            PersistentGameData.Instance.accumulatedCardTime = Time.timeSinceLevelLoad;
            Debug.Log("Cronómetro de tarjeta iniciado.");
        }
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
        if (Input.GetKeyDown(KeyCode.R)) Restart();
    }
        
    public void Restart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Lose()
    {
        // Enviar evento de derrota
        AnalyticsService.Instance.CustomData("PlayerDeaths", new Dictionary<string, object>
        {
            { "level", SceneManager.GetActiveScene().name }, // Nombre del nivel actual
            { "time_played", Time.timeSinceLevelLoad }      // Tiempo jugado en el nivel
        });
        Debug.Log("Evento PlayerDeaths enviado");
        defeatScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void DecreaseEnemyCount()
    {
        if (Enemies > 0)
        {
            // Inicia el cronómetro si es la primera muerte
            if (!hasStartedKillTimer)
            {
                killStartTime = Time.timeSinceLevelLoad;
                hasStartedKillTimer = true;
            }

            Enemies--;
            UpdateEnemiesText();

            // Envía el tiempo al eliminar al último enemigo
            if (Enemies == 0)
            {
                float killEndTime = Time.timeSinceLevelLoad;
                float levelKillTime = killEndTime - killStartTime; // Tiempo solo para este nivel

                // Registrar el evento en Analytics
                AnalyticsService.Instance.CustomData("PlayerKillTime", new Dictionary<string, object>
            {
                { "level", SceneManager.GetActiveScene().name }, // Nivel actual
                { "kill_time", levelKillTime }                  // Tiempo solo para este nivel
            });
                Debug.Log($"Evento PlayerKillTime enviado: {levelKillTime} segundos");

                // Acumular el tiempo total en la clase persistente
                PersistentGameData.Instance.accumulatedEnemyKillTime += levelKillTime;
                PersistentGameData.Instance.RegisterLevelTime(SceneManager.GetActiveScene().name,levelKillTime,PersistentGameData.Instance.accumulatedCardTime);
                // Reseteamos la variable local para evitar errores si el nivel se reinicia
                hasStartedKillTimer = false;
            }
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
