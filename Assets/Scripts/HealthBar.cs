using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerController thePlayer;
    public Image lifebarFill;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        float healthPercentage = thePlayer.currentHealth / thePlayer.maxHealth;
        rectTransform.localScale = new Vector3(healthPercentage, 1, 1);
        lifebarFill.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }
}
