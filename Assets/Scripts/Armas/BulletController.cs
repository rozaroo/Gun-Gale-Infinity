using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Transform bulletTr;
    Rigidbody bulletRb;
    public float bulletPower = 0f;
    public float lifeTime = 4f;
    private float time = 0f;

    public float bulletDamage = 1;
    Vector3 lastBulletPos;
    public LayerMask hitboxMask;

    void Start()
    {
        bulletTr = GetComponent<Transform>();
        bulletRb = GetComponent<Rigidbody>();
        bulletRb.velocity = this.transform.forward * bulletPower;
        hitboxMask = LayerMask.NameToLayer("Hitbox");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= lifeTime) Destroy(this.gameObject);
    }
    
}
