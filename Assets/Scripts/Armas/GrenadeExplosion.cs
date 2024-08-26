using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    Transform grenadeTr;
    Rigidbody grenadeRb;
    public bool explode = false;
    public float damageArea = 0f;
    public float throwForce = 0f;
    public float explodePower = 0f;
    public float lifeTime = 0f;
    public float explodeDamage = 0f;
    private float time = 0f;
    public LayerMask hitboxMask;
    Vector3 lastGrenadePos;
    public bool showDebugGizmos = true;

    void Start()
    {
        grenadeTr = GetComponent<Transform>();
        grenadeRb = GetComponent<Rigidbody>();
        hitboxMask = LayerMask.NameToLayer("Hitbox");
        grenadeRb.velocity = grenadeTr.forward * throwForce;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (explode)
        {
            if (time >= lifeTime)
            {
                ExplodeNow();
                Destroy(this.gameObject);
            }
        }
        else 
        { 
            DetectCollision();
            if (time >= lifeTime)
            {
                ExplodeNow();
                Destroy(this.gameObject);
            }
        }
    }
    public void ExplodeNow()
    {
        Vector3 explodePos = grenadeTr.position;
        Collider[] checking = Physics.OverlapSphere(explodePos, this.damageArea, ~hitboxMask);
        if (checking.Length > 0 ) 
        {
            foreach (Collider c in checking) 
            {
                GameObject go = c.gameObject;
                if (go.layer == hitboxMask)
                {
                    BodyPartHitCheck playerBodyPart = go.GetComponent<BodyPartHitCheck>();
                    if (playerBodyPart != null)
                    {
                        Vector3 collisionPos = c.ClosestPoint(explodePos);
                        float distance = Vector3.Distance(explodePos, collisionPos);
                        float damageDisminution = distance / damageArea;
                        float finalDamage = explodeDamage - explodeDamage * damageDisminution;
                        playerBodyPart.TakeHit(finalDamage);
                    }
                }
            }
        }
    }
    public void DetectCollision()
    {
        Vector3 grenadeNewPos = grenadeTr.position;
        Vector3 grenadeDirection = lastGrenadePos - grenadeNewPos;
        RaycastHit hit;
        if (Physics.Raycast(grenadeNewPos, grenadeDirection.normalized, out hit, grenadeDirection.magnitude))
        {
            GameObject go = hit.collider.gameObject;
            if (go.layer == hitboxMask)
            {
                BodyPartHitCheck playerBodyPart = go.GetComponent<BodyPartHitCheck>();
                if (playerBodyPart != null)
                {
                    playerBodyPart.TakeHit(explodeDamage);
                    Debug.Log("Impacto en " + playerBodyPart.BodyName);
                }
            }
        }
        lastGrenadePos = grenadeNewPos;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(grenadeTr.position, damageArea);
    }
}
