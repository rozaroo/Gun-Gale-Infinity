using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public enum StatesEnumSeis 
{
    Move,
    Death,
    Attack
}
public class SpaceEnemyTwoController : MonoBehaviour
{
    FSM<StatesEnumSeis> _fsm;
    ITreeNode _root;
    Func<bool> QuestionRange;
    QuestionNode auxiliarnode;
    public int HP = 99;
    public DronSpeed dronspeed;
    public ShipLevelManager lvlManager;
    public GameObject explosionPrefab;
    public GameObject portalPrefab;
    public Transform player;
    public DropPrefabsthree dropPrefabs;
    public DropProbabilitiesTwo dropProbabilities;
    public Transform dropSpawnPoint;
    public bool contacto = false;
    private float animDuration = 0.5f;
    //Steerings
    public Rigidbody target;
    public float timePrediction;
    public float radius;
    public float angle;
    public LayerMask maskObs;
    ISteering _steering;
    ObstacleAvoidance _obstacleAvoidance;
    Rigidbody _rb;
    private void Awake()
    {
        lvlManager = FindObjectOfType<ShipLevelManager>();
        player = GameObject.FindGameObjectWithTag("Nave").transform;
        _rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Nave").GetComponent<Rigidbody>();
    }

    void Start()
    {
        InitializeSteerings();
        InitializeFSM();
        InitializedTree();
    }

    // Update is called once per frame
    void Update()
    {
        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
    }
    void InitializeSteerings()
    {
        var pursuit = new Pursuit(this.transform, target, timePrediction);
        _steering = pursuit;
        _obstacleAvoidance = new ObstacleAvoidance(this.transform, angle, radius, maskObs, 2.5f);
    }
    void InitializeFSM()
    {
        var death = new SpaceEnemyTwoDeathState<StatesEnumSeis>(this, lvlManager);
        var move = new SpaceEnemyTwoMovementState<StatesEnumSeis>(this, _steering, _obstacleAvoidance);
        var attack = new SpaceEnemyTwoAttackState<StatesEnumSeis>(this, lvlManager);
        death.AddTransition(StatesEnumSeis.Move, move);
        death.AddTransition(StatesEnumSeis.Attack, attack);
        move.AddTransition(StatesEnumSeis.Death, death);
        move.AddTransition(StatesEnumSeis.Attack, attack);
        attack.AddTransition(StatesEnumSeis.Move, move);
        attack.AddTransition(StatesEnumSeis.Death, death);
        _fsm = new FSM<StatesEnumSeis>(move);
    }
    void InitializedTree()
    {
        var death = new ActionNode(() => _fsm.Transition(StatesEnumSeis.Death));
        var move = new ActionNode(() => _fsm.Transition(StatesEnumSeis.Move));
        var attack = new ActionNode(() => _fsm.Transition(StatesEnumSeis.Attack));
        //Preguntas 
        var qAttack = new QuestionNode(QuestionAttack(), attack, move);
        var qHasLife = new QuestionNode(QuestionHP(), death, qAttack);
        _root = qHasLife;
    }
    Func<bool> QuestionHP()
    {
        return () => HP <= 0;
    }
    Func<bool> QuestionAttack()
    {
        return () => contacto;
    }
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
    }
    public void Move(Vector3 dir)
    {
        dir *= dronspeed.Speed[0];
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
    public void DestroyShip()
    {
        SpawnRandomDrop();
        StartCoroutine(PlayDestructionAnimation(animDuration));
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
        StartCoroutine(PlayPortalAnimation(animDuration));
    }
    private IEnumerator PlayPortalAnimation(float duration)
    {
        GameObject portalObject = Instantiate(portalPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        Destroy(portalObject, duration);
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
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<SpaceShipController>();
        if (player != null) contacto = true;
    }
    public void Attack()
    {
        var playerController = player.GetComponent<SpaceShipController>();
        playerController?.TakeDamage(20f);
        StartCoroutine(PlayDestructionAnimation(animDuration));
    }
}
