using System;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public enum StatesEnum
{
    Patroll,
    Attack,
    Dead,
    Steering,
    Waypoints
}

public class EnemyController : MonoBehaviour, ILineOfSight
{
    public float distance;
    public Transform player;
    public float attackRange;
    LineOfSight _los;
    FSM<StatesEnum> _fsm;
    ITreeNode _root;
    Func<bool> QuestionRange;
    QuestionNode auxiliarnode;
    //Line of Sight
    public float range;
    [Range(1, 360)]
    public float angle;
    public LayerMask maskObs;
    Vector3 posplayer;

    LevelManager lvlManager;

    //Steerings behaviours
    public Rigidbody target;
    public float timePrediction;
    public float radius;
    ISteering _steering;
    ObstacleAvoidance _obstacleAvoidance;

    //A-star
    EnemyStateFollowPoints<StatesEnum> _stateFollowPoints;
    #region Enemy
    Quaternion targetRotation;
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;
    public Transform[] PuntosdePatrullaje;
    public float speed;
    public DropPrefabs dropPrefabs;
    public DropProbabilities dropProbabilities;
    public Transform dropSpawnPoint;
    //------------------------
    float timer;
    Rigidbody _rb;
    #endregion

    private void Awake()
    {
        lvlManager = FindObjectOfType<LevelManager>();
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        healthBar.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InitializeSteerings();
        InitializeFSM();
        InitializedTree();
    }
    private void Update()
    {
        healthBar.value = HP;
        distance = Vector3.Distance(player.position, transform.position);
        UpdateHealthBarVisibility();
        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
    }
    void InitializeSteerings()
    {
        var evade = new Evade(this.transform, target, timePrediction);
        var pursuit = new Pursuit(this.transform, target, timePrediction);
        _steering = pursuit;
        //_steering = evade;
        _obstacleAvoidance = new ObstacleAvoidance(this.transform, angle, radius, maskObs,2.5f);
    }
    void InitializeFSM()
    {
        var dead = new DeathState<StatesEnum>(this,lvlManager);
        var attack = new NewAttackState<StatesEnum>(this,player);
        var patroll = new NewPatrolState<StatesEnum>(this, _obstacleAvoidance);
        var steering = new EnemyStateSteering<StatesEnum>(this, _steering, _obstacleAvoidance);

        _stateFollowPoints = new EnemyStateFollowPoints<StatesEnum>(this, this.animator);

        patroll.AddTransition(StatesEnum.Dead, dead);
        patroll.AddTransition(StatesEnum.Attack, attack);
        patroll.AddTransition(StatesEnum.Steering, steering);
        patroll.AddTransition(StatesEnum.Waypoints, _stateFollowPoints);

        dead.AddTransition(StatesEnum.Patroll, patroll);
        dead.AddTransition(StatesEnum.Attack, attack);
        dead.AddTransition(StatesEnum.Steering, steering);
        dead.AddTransition(StatesEnum.Waypoints, _stateFollowPoints);

        attack.AddTransition(StatesEnum.Dead, dead);
        attack.AddTransition(StatesEnum.Patroll, patroll);
        attack.AddTransition(StatesEnum.Steering, steering);
        attack.AddTransition(StatesEnum.Waypoints, _stateFollowPoints);

        steering.AddTransition(StatesEnum.Patroll, patroll);
        steering.AddTransition(StatesEnum.Dead, dead);
        steering.AddTransition(StatesEnum.Attack, attack);
        steering.AddTransition(StatesEnum.Waypoints, _stateFollowPoints);

        _stateFollowPoints.AddTransition(StatesEnum.Dead, dead);
        _stateFollowPoints.AddTransition(StatesEnum.Attack, attack);
        _stateFollowPoints.AddTransition(StatesEnum.Patroll, patroll);
        _stateFollowPoints.AddTransition(StatesEnum.Steering, steering);

        _fsm = new FSM<StatesEnum>(steering);
    }
    void InitializedTree()
    {
        var dead = new ActionNode(() => _fsm.Transition(StatesEnum.Dead));
        var attack = new ActionNode(() => _fsm.Transition(StatesEnum.Attack));
        var patrol = new ActionNode(() => _fsm.Transition(StatesEnum.Patroll));
        var steering = new ActionNode(() => _fsm.Transition(StatesEnum.Steering));
        var Astar = new ActionNode(() => _fsm.Transition(StatesEnum.Waypoints));
        //Preguntas 
        var qRange = new QuestionNode(QuestionAttackRange(), attack,steering);
        var qLoS = new QuestionNode(QuestionLos(), patrol,qRange);
        var qHasLife = new QuestionNode(QuestionHP(), dead,qLoS);
        _root = qHasLife;
    }
    #region Questions
    Func<bool> QuestionAttackRange()
    {
        return () => distance < (range - 5f);
    }
    Func<bool> QuestionLos()
    {
        return () => !(CheckRange(player) && CheckAngle(player) && CheckView(player));
    }
    Func<bool> QuestionLosPlayer()
    {
        return () => (CheckRange(player) && CheckAngle(player) && CheckView(player));
    }
    Func<bool> QuestionHP()
    {
        return () => HP <= 0;
    }
    
    #endregion
    #region LineOfSight
    //Line Of Sight

    public bool CheckRange(Transform target)
    {
        float distance = Vector3.Distance(target.position, Origin);
        return distance <= range;
    }
   
    public bool CheckAngle(Transform target)
    {
        Vector3 dirToTarget = target.position - Origin;
        float angleToTarget = Vector3.Angle(Forward, dirToTarget);
        return angleToTarget <= angle / 2;
    }

    public bool CheckView(Transform target)
    {
        posplayer = target.position;
        return !Physics.Linecast(Origin + new Vector3(0, 1, 0), target.position + new Vector3(0, 1, 0), maskObs);
    }
    Vector3 Origin => transform.position;
    Vector3 Forward => transform.forward;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Origin, range);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(Origin, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, angle / 2, 0) * Forward * range);
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, -(angle / 2), 0) * Forward * range);
    }
    #endregion

    //A-Star
    public IPoints GetStateWaypoints => _stateFollowPoints;
    #region Enemy
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        animator.SetTrigger("damage");
    }

    public void SpawnRandomDrop()
    {
        if (dropPrefabs.prefabs.Length == 0 || dropProbabilities.probabilities.Length == 0 || dropPrefabs.prefabs.Length != dropProbabilities.probabilities.Length) return;
        float randomValue = UnityEngine.Random.value;
        //Dtermino que prefab spawmear basado en las probabilidades 
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

    public void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
        FireBall fireballScript = fireball.GetComponent<FireBall>();
        if (fireballScript != null && player != null) fireballScript.SetTarget(player);
    }
    public void Move(Vector3 dir)
    {
        transform.position += dir * Time.deltaTime * speed;
        //dir *= speed;
        //dir.y = _rb.velocity.y;
        //_rb.velocity = dir;
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
    void UpdateHealthBarVisibility()
    {
        bool isVisible = CheckRange(player) && CheckAngle(player) && CheckView(player);
        healthBar.gameObject.SetActive(isVisible);
    }
    #endregion
}

