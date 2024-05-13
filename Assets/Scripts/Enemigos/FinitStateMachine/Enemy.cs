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
    public Transform player;
    public Transform[] PuntosdePatrullaje;
    public float speed;
    public GameObject[] dropPrefabs;
    public float[] dropProbabilities;
    public Transform dropSpawnPoint;
    EnemyController enemyController;
    //------------------------
    float timer;
    float chaseRange = 8;
    int currentWaypointIndex = 0;
    int patrolDirection = 1; // 1 = adelante y -1 = atras
    ISteering _steering;
    
    LevelManager lvlManager;
    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        lvlManager = GetComponent<LevelManager>();
    }
    public int GetHP() { return HP; }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    void Update()
    {
        healthBar.value = HP;
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        animator.SetTrigger("damage");
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
