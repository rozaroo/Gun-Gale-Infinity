using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;
    private float maxTime = 10.0f;

    void Update()
    {
       if (maxTime > 0) 
       {
            maxTime -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(maxTime % 60);
            countText.text = string.Format("{1:00}", seconds);
        }
       else SceneManager.LoadScene(5);
    }
}