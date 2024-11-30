using UnityEngine;

public class PersistentGameData : MonoBehaviour
{
    public static PersistentGameData Instance;
    // Variables persistentes
    public float accumulatedEnemyKillTime = 0f; // Tiempo acumulado para matar enemigos
    public float accumulatedCardTime = 0f;     // Tiempo acumulado para recoger la tarjeta

    private void Awake()
    {
        // Asegurarse de que solo exista una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir este objeto al cargar nuevas escenas
        }
        else Destroy(gameObject);
    }
    public void RegisterLevelTime(string levelName, float enemyKillTime, float cardTime = 0f)
    {
        float maxTime = Mathf.Max(enemyKillTime, cardTime); // Escoge el tiempo mayor
        if (levelMaxTimes.ContainsKey(levelName)) levelMaxTimes[levelName] = Mathf.Max(levelMaxTimes[levelName], maxTime); // Actualiza si es mayor
        else levelMaxTimes[levelName] = maxTime;
    }
    public float GetTotalPlayTime()
    {
        float totalPlayTime = 0f;
        foreach (var time in levelMaxTimes.Values)
            totalPlayTime += time;
        return totalPlayTime;
    }
}
