using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBarController : MonoBehaviour
{
    [SerializeField] private Image shieldBarFill;
    PlayerController Player;

    void Start()
    {
        Player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        shieldBarFill.fillAmount = Player.currentshield / 100;
    }
}
