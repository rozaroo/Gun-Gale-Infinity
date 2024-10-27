using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Municion : MonoBehaviour
{
    SpaceShipController Player;
    [SerializeField] private TextMeshProUGUI municionYCartuchosTMP;
    void Start()
    {
        Player = FindObjectOfType<SpaceShipController>();
    }

    void Update()
    {
        municionYCartuchosTMP.text = $"{Player.MunicionActual()} / {Player.CartuchosDisponibles()}";
        if (Player == null) municionYCartuchosTMP.enabled = false;
    }
}
