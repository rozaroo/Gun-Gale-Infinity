using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarTwo : MonoBehaviour
{
    [SerializeField] private Image lifeBarFill;
    [SerializeField] private Image Borde;
    SpaceShipController Player;
    [SerializeField] private Color fullHealth = Color.white;
    [SerializeField] private Color midHealth = Color.yellow;
    [SerializeField] private Color lowHealth = Color.red;
    void Start()
    {
        Player = FindObjectOfType<SpaceShipController>();
    }

    void Update()
    {
        float healthPercentage = Player.currentHealth / 100f;
        lifeBarFill.fillAmount = healthPercentage;
        if (healthPercentage > 0.5f)
        {
            lifeBarFill.color = fullHealth;
            Borde.color = fullHealth;
        }
        else if (healthPercentage > 0.25f && healthPercentage <= 0.5f)
        {
            lifeBarFill.color = midHealth;
            Borde.color = midHealth;
        }
        else if (healthPercentage <= 0.25f)
        {
            lifeBarFill.color = lowHealth;
            Borde.color = lowHealth;
        }
        if (Player == null) 
        {
            lifeBarFill.enabled = false;
            Borde.enabled = false;
        }
    }
}
