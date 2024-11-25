using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
 [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab; 
    [SerializeField] private float damageArea; 
    [SerializeField] private float explosionForce; 
    [SerializeField] private float explosionDamage;
    [SerializeField] private float lifeTime;

    [Header("Debug Settings")]
    [SerializeField] private bool showDebugGizmos = true; 
    [SerializeField] private LayerMask hitboxMask; 

    private Rigidbody grenadeRb;
    private float timer = 0f;
    private Vector3 lastGrenadePos;

    void Start()
    {
        grenadeRb = GetComponent<Rigidbody>();
        lastGrenadePos = transform.position;

        if (hitboxMask == 0)
        hitboxMask = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            Explode();
            Destroy(gameObject); 
        }
        else
        {
            DetectCollision();
        }
    }

    private void Explode()
    {
        Vector3 explosionPosition = transform.position;

        Collider[] affectedColliders = Physics.OverlapSphere(explosionPosition, damageArea, hitboxMask);
        foreach (Collider collider in affectedColliders)
        {
            GameObject obj = collider.gameObject;

            // Aplicar daño a los enemigos
            EnemyController enemy = obj.GetComponent<EnemyController>();
            if (enemy != null)
            {
                float distance = Vector3.Distance(explosionPosition, collider.ClosestPoint(explosionPosition));
                float damageMultiplier = Mathf.Clamp01(1 - (distance / damageArea));
                float finalDamage = explosionDamage * damageMultiplier;
                enemy.TakeDamage((int)finalDamage);
            }

            // Aplicar fuerza a los objetos con Rigidbody
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, damageArea);
            }
        }

        // Instanciar el efecto visual de la explosión
        Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
    }

    private void DetectCollision()
    {
        Vector3 currentPos = transform.position;
        Vector3 direction = lastGrenadePos - currentPos;
        float distance = direction.magnitude;

        if (Physics.Raycast(currentPos, direction.normalized, out RaycastHit hit, distance, hitboxMask))
        {
            GameObject obj = hit.collider.gameObject;

            // Detectar enemigos al impactar
            EnemyController enemy = obj.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)explosionDamage);
                Debug.Log($"Impacto directo en {enemy.name}");
            }

            // Detonar inmediatamente si colisiona
            Explode();
            Destroy(gameObject);
        }

        lastGrenadePos = currentPos;
    }

    private void OnDrawGizmos()
    {
        if (showDebugGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, damageArea); // Radio de la explosión
        }
    }
}
