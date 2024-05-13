using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject[] Enemies;
    public GameObject Portal;
    
    void Start()
    {
        Portal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemies.Length == 0) Portal.SetActive(true);
    }
}
