using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPowerUp : MonoBehaviour
{
    public int cantidadCartucuchos = 1;
    private void OnTriggerEnter(Collider other)
    {
        SpaceShipController ship = other.GetComponent<SpaceShipController>();
        if (ship != null)
        {
            ship.AddCartucho(cantidadCartucuchos);
            Destroy(gameObject);
        }
    }

}
