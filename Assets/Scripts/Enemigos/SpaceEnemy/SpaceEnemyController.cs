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
    public int HP = 99;
    public GameObject EnemyShootPrefab;
    public Transform ShootSpawnPoint;
    public Transform[] PuntosdeMovimiento;
    public DronSpeed dronspeed;
    public ShipLevelManager lvlManager;
    public GameObject explosionPrefab;
    public GameObject portalPrefab;
    public GameObject DamagePrefab;
    public Transform player;
    public DropPrefabsthree dropPrefabs;
    public DropProbabilitiesTwo dropProbabilities;
    public Transform dropSpawnPoint;
    
    private void Awake()
    {
        lvlManager = FindObjectOfType<ShipLevelManager>();
        player = GameObject.FindGameObjectWithTag("Nave").transform;
    }

    void Start()
    {
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
        var death = new SpaceEnemyDeathState<StatesEnumCinco>(this, lvlManager);
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
        if (HP > 34) StartCoroutine(Damage(0.5f));
    }
    public void Shoot()
    {
        GameObject shoot = Instantiate(EnemyShootPrefab, ShootSpawnPoint.position, Quaternion.identity);
        EnemyShoot ShootScript = shoot.GetComponent<EnemyShoot>();
        if (ShootScript != null && player != null) ShootScript.SetTarget(player);
    }
    public void Move(Vector3 dir)
    {
        transform.position += dir * Time.deltaTime * dronspeed.Speed[0];
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void DestroyShip()
    {
        SpawnRandomDrop();
        StartCoroutine(PlayDestructionAnimation(0.5f));
    }
    private IEnumerator PlayDestructionAnimation(float duration)
    {
        GameObject explosionObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
        Destroy(explosionObject, duration);
    }
    public void PortalShip()
    {
        StartCoroutine(PlayPortalAnimation(0.5f));
    }
    private IEnumerator PlayPortalAnimation(float duration)
    {
        GameObject portalObject = Instantiate(portalPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        Destroy(portalObject, duration);
    }
    private IEnumerator Damage(float duration)
    {
        GameObject damageObject = Instantiate(DamagePrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        Destroy(damageObject, duration);
    }
    public void SpawnRandomDrop()
    {
        if (dropPrefabs.prefabs.Length == 0 || dropProbabilities.probabilities.Length == 0 || dropPrefabs.prefabs.Length != dropProbabilities.probabilities.Length) return;
        float randomValue = UnityEngine.Random.value;
        float cumulativeProbability = 0f;
        for (int i = 0; i < dropProbabilities.probabilities.Length; i++)
        {
            cumulativeProbability += dropProbabilities.probabilities[i];
            if (randomValue < cumulativeProbability)
            {
                Instantiate(dropPrefabs.prefabs[i], dropSpawnPoint.position, Quaternion.identity);
                break;
            }
        }
    }
}
