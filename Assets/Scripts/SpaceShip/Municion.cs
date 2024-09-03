using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Municion : MonoBehaviour
{
    SpaceShipController Player;
    [SerializeField] private TextMeshProUGUI municionTMP;
    [SerializeField] private TextMeshProUGUI cartuchosTMP;
    void Start()
    {
        Player = FindObjectOfType<SpaceShipController>();
    }

    void Update()
    {
        municionTMP.text = "" + Player.MunicionActual();
        cartuchosTMP.text = "" + Player.CartuchosDisponibles();
    }
}
