using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private float LevelTime;
    [SerializeField] private TMP_Text countText;
    private int index;
    private float maxTime;

    void Start()
    {
        maxTime = LevelTime;
    }

    void Update()
    {
       if (maxTime > 0)
       {
            maxTime -= Time.deltaTime;
       }
       else if (maxTime < 0)
       {
            maxTime = 0;
       }

       if (maxTime <= 0)
       {
            levelManager.Lose();
       }

       int minutes = Mathf.FloorToInt(maxTime/60);
       int seconds = Mathf.FloorToInt(maxTime%60);

       countText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
