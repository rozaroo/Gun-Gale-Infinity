using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public enum StatesEnumDos
{
    Idle,
    Dead,
    Steering,
    Waypoints
}

public class EnemyControllerTwo : MonoBehaviour, ILineOfSight
{
    public float distance;
    public Transform player;
    LineOfSight _los;
    FSM<StatesEnumDos> _fsm;
    ITreeNode _root;
    Func<bool> QuestionRange;
    QuestionNode auxiliarnode;
    //Line of Sight
    public float range;
    [Range(1, 360)]
    public float angle;
    public LayerMask maskObs;
    Vector3 posplayer;

    //Steerings behaviours
    public Rigidbody target;
    public float timePrediction;
    public float radius;
    ISteering _steering;
    ObstacleAvoidance _obstacleAvoidance;

    LevelManager lvlManager;

    //A-star
    //EnemyStateFollowPoints<StatesEnum> _stateFollowPoints;
    #region EnemyTwo
    Quaternion targetRotation;
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;
    public float speed;
    public GameObject[] dropPrefabs;
    public float[] dropProbabilities;
    public Transform dropSpawnPoint;
    //------------------------
    float timer;
    float chaseRange = 8;
    int currentWaypointIndex = 0;
    ISteering _steering;

    Rigidbody _rb;
    #endregion


    private void Awake()
    {
        lvlManager = FindObjectOfType<LevelManager>();
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InitializeSteerings();
        InitializeFSM();
        InitializedTree();
    }
    private void Update()
    {
        healthBar.value = HP;
        distance = Vector3.Distance(player.position, transform.position);
        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
    }
    void InitializeSteerings()
    {
        var evade = new Evade(this.transform, target, timePrediction);
        _steering = evade;
        _obstacleAvoidance = new ObstacleAvoidance(this.transform, angle, radius, maskObs, 2.5f);
    }
    void InitializeFSM()
    {
        var idle = new EnemyIdleState<StatesEnumDos>(this);
        var dead = new DeathStateTwo<StatesEnumDos>(this, lvlManager);
        var steering = new EnemyStateSteeringTwo<StatesEnumDos>(this, _steering, _obstacleAvoidance);
        //_stateFollowPoints = new EnemyStateFollowPoints<StatesEnumDos>(enemy, enemy.animator);

        idle.AddTransition(StatesEnumDos.Steering, steering);
        //idle.AddTransition(StatesEnumDos.Waypoints, _stateFollowPoints);
        idle.AddTransition(StatesEnumDos.Dead, dead);

        dead.AddTransition(StatesEnumDos.Steering, steering);
        //dead.AddTransition(StatesEnumDos.Waypoints, _stateFollowPoints);
        dead.AddTransition(StatesEnumDos.Idle, idle);

        steering.AddTransition(StatesEnumDos.Dead, dead);
        //steering.AddTransition(StatesEnumDos.Waypoints, _stateFollowPoints);
        steering.AddTransition(StatesEnumDos.Idle, idle);

        //_stateFollowPoints.AddTransition(StatesEnumDos.Dead, dead);
        //_stateFollowPoints.AddTransition(StatesEnumDos.Steering, steering);
        //_stateFollowPoints.AddTransition(StatesEnumDos.Idle, idle);

        _fsm = new FSM<StatesEnumDos>(idle);
    }
    void InitializedTree()
    {
        var dead = new ActionNode(() => _fsm.Transition(StatesEnumDos.Dead));
        var steering = new ActionNode(() => _fsm.Transition(StatesEnumDos.Steering));
        var idle = new ActionNode(() => _fsm.Transition(StatesEnumDos.Idle));

        var qLoS = new QuestionNode(QuestionLosPlayer(), steering, idle);
        var qHasLife = new QuestionNode(QuestionHP(), dead, qLoS);
        _root = qHasLife;
    }
    #region Questions
    Func<bool> QuestionRango()
    {
        return () => distance < (range - 10f);
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
    //public IPoints GetStateWaypoints => _stateFollowPoints;
    #region EnemyTwo
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
    #endregion
}
