using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public int damageAmount = 20;
    public void OnCollisionEnter(Collision collision) 
    {
        var enemy = collision.collider.GetComponent<EnemyFSM>();
        if (enemy != null) enemy.TakeDamage(damageAmount);
        Destroy(gameObject);
    }
    
}
