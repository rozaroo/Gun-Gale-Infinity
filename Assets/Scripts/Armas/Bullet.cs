using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public int damageAmount = 20;
    public void OnTriggerEnter(Collider other) 
    {
        var enemy = other.GetComponent<EnemyController>();
        if (enemy != null) 
        {
            enemy.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        var enemy2 = other.GetComponent<EnemyControllerTwo>();
        if (enemy2 != null) 
        {
            enemy2.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        var slime = other.GetComponent<SlimeController>();
        if (slime != null)
        {
            slime.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}
