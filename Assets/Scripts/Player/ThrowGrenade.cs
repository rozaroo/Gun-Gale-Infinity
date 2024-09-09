using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform grenadePos;
    [SerializeField] private float throwForce;
    
    public void Throw()
    {
        Debug.Log($"<color=green>MUERANAANANAN</color>");
        GameObject grenade = Instantiate(grenadePrefab, grenadePos.position, Quaternion.identity);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        
        rb.AddForce(grenadePos.forward.normalized * throwForce, ForceMode.Impulse);
    }
}
