using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody target;
    public float timePrediction;
    public float angle;
    public float radius;
    public LayerMask maskObs;
    ISteering _steering;
    Enemy enemy;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        InitializeSteerings();
    }
    void Update()
    {
        //Obtener direccion del steering
        Vector3 dir = _steering.GetDir();
        enemy.Move(dir);
    }
    void InitializeSteerings()
    {
        _steering = new Pursuit(enemy.transform, target, timePrediction);  
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius);
    }
}
