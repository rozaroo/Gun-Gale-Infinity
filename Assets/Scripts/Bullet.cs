using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public int damageAmount = 20;
    public void OnCollisionEnter(Collision collision) 
    {
        var enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null) 
        {
            enemy.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        var enemy2 = collision.collider.GetComponent<EnemyTwo>();
        if (enemy2 != null) 
        {
            enemy2.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        var slime = collision.collider.GetComponent<SlimeController>();
        if (slime != null)
        {
            slime.TakeDamage(damageAmount);
            Destroy(gameObject);
        }

    }
    
}
