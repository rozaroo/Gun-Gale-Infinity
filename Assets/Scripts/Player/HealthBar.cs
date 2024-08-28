using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image lifeBarFIll;
    PlayerController Player;

    void Start()
    {
        Player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        lifeBarFIll.fillAmount = Player.currentHealth / 100;
    }
}
