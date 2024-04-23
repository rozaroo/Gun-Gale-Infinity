using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    Quaternion targetRotation;
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;
    private Transform player;
    public GameObject[] PuntosdePatrullaje;
    public float speed;
    SightModel scriptalerta;
    ILineOfSight _los;
    LineOfSight lineOfSight;

    public GameObject[] dropPrefabs;
    public float[] dropProbabilities;
    public Transform dropSpawnPoint;

    //------------------------

    float timer;
    List<Transform> wayPoints = new List<Transform>();
    float chaseRange = 8;
    int currentWaypointIndex = 0;
    int patrolDirection = 1; // 1 = adelante y -1 = atras

    ISteering _steering;
    //--------------------------
    bool Estados_A;

    private void Awake()
    {
        _los = GetComponent<ILineOfSight>();
        lineOfSight = GetComponent<LineOfSight>();
    }
    public LineOfSight LineOfSight { get { return lineOfSight; } }
    public ILineOfSight LOS { get { return _los; } }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        BodyPartHitCheck playerBodyPart = player.GetComponent<BodyPartHitCheck>();
        scriptalerta = GetComponent<SightModel>();
        ///----------------------------------------------------
        wayPoints.AddRange(Array.ConvertAll(PuntosdePatrullaje, item => item.transform));
        currentWaypointIndex = UnityEngine.Random.Range(0, wayPoints.Count);
        timer = 0;
        _steering = new Pursuit(transform, player.GetComponent<Rigidbody>(), 0.5f);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        Estados_A = _los.CheckAngle(player) && _los.CheckRange(player) && _los.CheckView(player);
        healthBar.value = HP;
        //timer += Time.deltaTime;
        animator.SetBool("isPatrolling", !scriptalerta.Alert);
        animator.SetBool("isChasing", scriptalerta.Alert);
        animator.SetBool("isAttacking", scriptalerta.Alert && distance < lineOfSight.range - 5f);
        Vector3 direction = _steering.GetDir();
        if (wayPoints.Count == 0) return;
        if (currentWaypointIndex < 0 || currentWaypointIndex >= wayPoints.Count) return;
        if (wayPoints.Count > 0 && currentWaypointIndex >= 0 && currentWaypointIndex < wayPoints.Count)
        {

            if (!scriptalerta.Alert)///si no esta en alerta patrulla
            {
                Vector3 waypointdirection = wayPoints[currentWaypointIndex].position - transform.position;
                waypointdirection.y = 0;
                //Movimiento hacia el actual punto de patrullaje
                transform.Translate(waypointdirection.normalized * Time.deltaTime * 1.5f, Space.World);
                //Rotación hacia el siguiente punto de patrulla
                targetRotation = Quaternion.LookRotation(waypointdirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);

                if (Vector3.Distance(transform.position, wayPoints[currentWaypointIndex].position) < 0.5f)
                {
                    //cambiar de dirección
                    if (currentWaypointIndex == wayPoints.Count - 1 || currentWaypointIndex == 0)
                    {
                        patrolDirection *= -1;
                        //Pasar al siguiente punto
                        currentWaypointIndex += patrolDirection;
                        //Restringir el indice al rango valido
                        currentWaypointIndex = Mathf.Clamp(currentWaypointIndex, 0, wayPoints.Count - 1);
                    }
                }
            }
            else //sino persigue al player
            {
                Vector3 playerDirection = (player.position - transform.position).normalized;
                playerDirection.y = 0;
                if (Estados_A && distance >= lineOfSight.range - 5f) Move(playerDirection);//SteeringBehaviour..
                targetRotation = Quaternion.LookRotation(playerDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            }
            if (Estados_A) scriptalerta.Alert = true;///el enemigo esta en alerta
            else scriptalerta.Alert = false;///el enemigo no  esta en alerta
        }
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP < 0) Die();
        else animator.SetTrigger("damage");
    }
    public void Die()
    {
        animator.SetTrigger("die");
        GetComponent<Collider>().enabled = false;
        SpawnRandomDrop();
        Destroy(gameObject, 2f);
    }

    public void SpawnRandomDrop()
    {
        if (dropPrefabs.Length == 0 || dropProbabilities.Length == 0 || dropPrefabs.Length != dropProbabilities.Length) return;
        float randomValue = UnityEngine.Random.value;
        //Dtermino que prefab spawmear basado en las probabilidades 
        float cumulativeProbability = 0f;
        for (int i = 0; i < dropProbabilities.Length; i++)
        {
            cumulativeProbability += dropProbabilities[i];
            if (randomValue < cumulativeProbability)
            {
                Instantiate(dropPrefabs[i], dropSpawnPoint.position, Quaternion.identity);
                break;
            }
        }
    }

    public void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
        FireBall fireballScript = fireball.GetComponent<FireBall>();
        if (fireballScript != null && player != null) fireballScript.SetTarget(player);
    }
    public void Move(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * speed;
    }


}
