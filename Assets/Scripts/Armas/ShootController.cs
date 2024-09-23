using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    Transform shootTr;
    Rigidbody shootRb;
    public float shootPower = 0f;
    public float lifeTime = 4f;
    private float time = 0f;

    private int shootDamage = 33;
    Vector3 lastBulletPos;

    void Start()
    {
        shootTr = GetComponent<Transform>();
        shootRb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= lifeTime) Destroy(this.gameObject);
    }
    public void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.collider.GetComponent<SpaceEnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(shootDamage);
            Destroy(gameObject);
        }
    }
}

