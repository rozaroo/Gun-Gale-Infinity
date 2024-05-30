using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Slime : MonoBehaviour, IBoid
{
    Quaternion targetRotation;
    private int HP = 100;
    public Transform player;
    public float speed;
    public float speedRoot;
    SlimeController slimeController;
    //------------------------
    float timer;
    float chaseRange = 8;
    ISteering _steering;

    LevelManager lvlManager;

    Rigidbody _rb;

    public Vector3 Position => transform.position;
    public Vector3 Front => transform.forward;

    public GameObject particlePrefab;
    private void Awake()
    {
        slimeController = GetComponent<SlimeController>();
        lvlManager = GetComponent<LevelManager>();
        _rb = GetComponent<Rigidbody>();
    }
    public int GetHP() { return HP; }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
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
    public void DestroySlime()
    {
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
