using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject keyCardIndicator;
    [SerializeField] private GameObject CardUISprite;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            player.HasKeyCard = true;
            keyCardIndicator.SetActive(true);
            CardUISprite.SetActive(false);
            // Envía el evento si el cronómetro estaba activo
            if (FindObjectOfType<LevelManager>() != null)
            {
                LevelManager levelManager = FindObjectOfType<LevelManager>();
                if (levelManager.isCardTimerRunning)
                {
                    float cardEndTime = Time.timeSinceLevelLoad;
                    float totalCardTime = cardEndTime - PersistentGameData.Instance.accumulatedCardTime;
                    // Enviar datos a Analytics
                    Unity.Services.Analytics.AnalyticsService.Instance.CustomEvent("CardTime", new Dictionary<string, object>
                    {
                        { "level", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name },
                        { "time_to_collect_card", totalCardTime }
                    });

                    Debug.Log($"Tiempo acumulado para recoger tarjeta: {totalCardTime} segundos");

                    // Actualizar el tiempo acumulado
                    PersistentGameData.Instance.accumulatedCardTime += totalCardTime;

                    // Detener el cronómetro
                    levelManager.isCardTimerRunning = false;
                }
            }
            Destroy(gameObject,0.1f);
        }
    }
}