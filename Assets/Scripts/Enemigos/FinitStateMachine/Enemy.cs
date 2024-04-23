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
    StateMachine<Enemy> stateMachine;

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
        stateMachine = new StateMachine<Enemy>(this);
        stateMachine.SetInitialState(new NewPatrolState<Enemy>(this));
        stateMachine.AddState(new NewChaseState<Enemy>(this,player));
        stateMachine.AddState(new DeathState<Enemy>(this));
        /*BodyPartHitCheck playerBodyPart = player.GetComponent<BodyPartHitCheck>();
        scriptalerta = GetComponent<SightModel>();
        ///----------------------------------------------------
        wayPoints.AddRange(Array.ConvertAll(PuntosdePatrullaje, item => item.transform));
        currentWaypointIndex = UnityEngine.Random.Range(0, wayPoints.Count);
        timer = 0;
        _steering = new Pursuit(transform, player.GetComponent<Rigidbody>(), 0.5f);*/
    }

    void Update()
    {
        stateMachine.ExecuteCurrentState();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP < 0) Die();
        else animator.SetTrigger("damage");
    }
    public void Die()
    {
        stateMachine.TransitionToState((Enemy)(object)new DeathState<Enemy>(this));
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
