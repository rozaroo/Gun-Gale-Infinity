using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUpCapsule : MonoBehaviour
{
    public float cantidadVida = 25f;
    private void OnTriggerEnter(Collider other)
    {
        SpaceShipController ship = other.GetComponent<SpaceShipController>();
        if (ship != null)
        {
            ship.RecoveryHealth(cantidadVida);
            Destroy(gameObject);
        }
    }

}
