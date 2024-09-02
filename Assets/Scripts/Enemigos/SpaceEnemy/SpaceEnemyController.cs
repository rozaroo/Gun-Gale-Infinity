using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;


public enum StatesEnumCinco 
{
    Move,
    Death
}
public class SpaceEnemyController : MonoBehaviour
{
    FSM<StatesEnumCinco> _fsm;
    ITreeNode _root;
    Func<bool> QuestionRange;
    QuestionNode auxiliarnode;
    private int HP = 100;
    public GameObject EnemyShootPrefab;
    public Transform ShootSpawnPoint;
    public Transform[] PuntosdeMovimiento;
    public float speed;
    public ShipLevelManager lvlManager;
    public GameObject explosionPrefab;
    public Transform player;
    private void Awake()
    {
        lvlManager = FindObjectOfType<ShipLevelManager>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Nave").transform;
        InitializeFSM();
        InitializedTree();
    }

    // Update is called once per frame
    void Update()
    {
        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
    }
    void InitializeFSM()
    {
        var death = new SpaceEnemyDeathState<StatesEnumCinco>(this);
        var move = new SpaceEnemyMovementState<StatesEnumCinco>(this);
        death.AddTransition(StatesEnumCinco.Move, move);
        move.AddTransition(StatesEnumCinco.Death, death);
        _fsm = new FSM<StatesEnumCinco>(move);
    }
    void InitializedTree()
    {
        var death = new ActionNode(() => _fsm.Transition(StatesEnumCinco.Death));
        var move = new ActionNode(() => _fsm.Transition(StatesEnumCinco.Move));
        //Preguntas 
        var qHasLife = new QuestionNode(QuestionHP(), death, move);
        _root = qHasLife;
    }
    Func<bool> QuestionHP()
    {
        return () => HP <= 0;
    }
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
    }
    public void Shoot()
    {
        GameObject shoot = Instantiate(EnemyShootPrefab, ShootSpawnPoint.position, Quaternion.identity);
        EnemyShoot ShootScript = shoot.GetComponent<EnemyShoot>();
        if (ShootScript != null && player != null) ShootScript.SetTarget(player);
    }
    public void Move(Vector3 dir)
    {
        transform.position += dir * Time.deltaTime * speed;
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void DestroyShip()
    {
        StartCoroutine(PlayDestructionAnimation(0.5f));
    }
    private IEnumerator PlayDestructionAnimation(float duration)
    {
        GameObject explosionObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
        Destroy(explosionObject, duration);
    }
}
