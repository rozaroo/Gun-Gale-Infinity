using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class EnemyTwo : MonoBehaviour
{
    Quaternion targetRotation;
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;
    public Transform player;
    public float speed;
    public GameObject[] dropPrefabs;
    public float[] dropProbabilities;
    public Transform dropSpawnPoint;
    EnemyControllerTwo enemyController;
    //------------------------
    float timer;
    float chaseRange = 8;
    int currentWaypointIndex = 0;
    ISteering _steering;

    Rigidbody _rb;

    private void Awake()
    {
        enemyController = GetComponent<EnemyControllerTwo>();
        _rb = GetComponent<Rigidbody>();
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

    public void Move(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * speed;
    }
    public void Movetwo(Vector3 dir)
    {
        dir *= (speed * Time.deltaTime);
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }
    public void LookDir(Vector3 dir)
    {
        if (dir.x == 0 && dir.z == 0) return;
        transform.forward = dir;
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
