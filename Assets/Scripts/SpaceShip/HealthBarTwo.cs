using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarTwo : MonoBehaviour
{
    [SerializeField] private Image lifeBarFIll;
    SpaceShipController Player;

    void Start()
    {
        Player = FindObjectOfType<SpaceShipController>();
    }

    void Update()
    {
        lifeBarFIll.fillAmount = Player.currentHealth / 100;
    }
}
