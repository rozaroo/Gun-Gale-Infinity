using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Analytics;

public class ShipLevelManager : MonoBehaviour
{
    public int Enemies;
    public GameObject Oleada1;
    public GameObject Oleada2;
    public GameObject Oleada3;
    [SerializeField] private TextMeshProUGUI oleadaTMP;
    [SerializeField] private AudioSource backgroundMusic;
    private bool isMuted = false;
    private float killStartTime;  // Tiempo cuando matas al primer enemigo
    private bool hasStartedKillTimer = false;
    [SerializeField] GameObject defeatScreen;
    SpaceShipController shipController;
    private void Awake()
    {
        shipController = GameObject.FindObjectOfType<SpaceShipController>();
        
    }
    void Start()
    {
        if (PersistentGameData.Instance.accumulatedEnemyKillTime == 0f) PersistentGameData.Instance.accumulatedEnemyKillTime = Time.timeSinceLevelLoad;
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
        if (Input.GetKeyDown(KeyCode.R)) Restart();
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
        SceneManager.LoadScene(8);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Win()
    {
        float levelKillTime = PersistentGameData.Instance.accumulatedEnemyKillTime;
        float cardTime = PersistentGameData.Instance.accumulatedCardTime;
        PersistentGameData.Instance.RegisterLevelTime(SceneManager.GetActiveScene().name,levelKillTime);
        SceneManager.LoadScene(9);
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
        oleadaTMP.enabled = false;
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
                // Reseteamos la variable local para evitar errores si el nivel se reinicia
                hasStartedKillTimer = false;
            }
        }
    }
}
