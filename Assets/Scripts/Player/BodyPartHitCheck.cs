using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartHitCheck : MonoBehaviour
{
    [HideInInspector]
    public PlayerController PLAYER;

    public string BodyName;
    public float Multiplier;
    public float LastDamage;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void TakeHit(float damage)
    {
        LastDamage = damage * Multiplier;
        this.PLAYER.TakeDamage(LastDamage);
        Debug.Log(damage + " * " + Multiplier + " = " + LastDamage);
    }

}
