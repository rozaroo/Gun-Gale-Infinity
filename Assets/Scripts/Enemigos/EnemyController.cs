using System;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public enum StatesEnum
{
    Patroll,
    Chase,
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
    Enemy enemy;
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

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        lvlManager = FindObjectOfType<LevelManager>();
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
        distance = Vector3.Distance(player.position, transform.position);
        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
    }
    void InitializeSteerings()
    {
        var pursuit = new Pursuit(enemy.transform, target, timePrediction);
        _steering = pursuit;
        _obstacleAvoidance = new ObstacleAvoidance(enemy.transform, angle, radius, maskObs,2.5f);
    }
    void InitializeFSM()
    {
        var dead = new DeathState<StatesEnum>(enemy,lvlManager);
        var attack = new NewAttackState<StatesEnum>(enemy,player);
        var chase = new NewChaseState<StatesEnum>(this,enemy);
        var patroll = new NewPatrolState<StatesEnum>(enemy, _obstacleAvoidance);
        var steering = new EnemyStateSteering<StatesEnum>(enemy, _steering, _obstacleAvoidance);

        _stateFollowPoints = new EnemyStateFollowPoints<StatesEnum>(enemy, enemy.animator);

        patroll.AddTransition(StatesEnum.Dead, dead);
        patroll.AddTransition(StatesEnum.Attack, attack);
        patroll.AddTransition(StatesEnum.Chase, chase);
        patroll.AddTransition(StatesEnum.Steering, steering);
        patroll.AddTransition(StatesEnum.Waypoints, _stateFollowPoints);

        dead.AddTransition(StatesEnum.Patroll, patroll);
        dead.AddTransition(StatesEnum.Attack, attack);
        dead.AddTransition(StatesEnum.Chase, chase);
        dead.AddTransition(StatesEnum.Steering, steering);
        dead.AddTransition(StatesEnum.Waypoints, _stateFollowPoints);

        attack.AddTransition(StatesEnum.Dead, dead);
        attack.AddTransition(StatesEnum.Chase, chase);
        attack.AddTransition(StatesEnum.Patroll, patroll);
        attack.AddTransition(StatesEnum.Steering, steering);
        attack.AddTransition(StatesEnum.Waypoints, _stateFollowPoints);

        chase.AddTransition(StatesEnum.Dead, dead);
        chase.AddTransition(StatesEnum.Attack, attack);
        chase.AddTransition(StatesEnum.Patroll, patroll);
        chase.AddTransition(StatesEnum.Steering, steering);
        chase.AddTransition(StatesEnum.Waypoints, _stateFollowPoints);

        steering.AddTransition(StatesEnum.Patroll, patroll);
        steering.AddTransition(StatesEnum.Dead, dead);
        steering.AddTransition(StatesEnum.Attack, attack);
        steering.AddTransition(StatesEnum.Chase, chase);
        steering.AddTransition(StatesEnum.Waypoints, _stateFollowPoints);

        _stateFollowPoints.AddTransition(StatesEnum.Dead, dead);
        _stateFollowPoints.AddTransition(StatesEnum.Attack, attack);
        _stateFollowPoints.AddTransition(StatesEnum.Patroll, patroll);
        _stateFollowPoints.AddTransition(StatesEnum.Steering, steering);

        _fsm = new FSM<StatesEnum>(_stateFollowPoints);
    }
    void InitializedTree()
    {
        var dead = new ActionNode(() => _fsm.Transition(StatesEnum.Dead));
        var attack = new ActionNode(() => _fsm.Transition(StatesEnum.Attack));
        var chase = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));
        var patrol = new ActionNode(() => _fsm.Transition(StatesEnum.Patroll));
        var pursuit = new ActionNode(() => _fsm.Transition(StatesEnum.Steering));
        var Astar = new ActionNode(() => _fsm.Transition(StatesEnum.Waypoints));
        //Preguntas 
        //auxiliarnode = new QuestionNode(QuestionAttackRange(), attack, chase);
        //QuestionRange = auxiliarnode._question;
        var qRange = new QuestionNode(QuestionAttackRange(), attack,pursuit);
        //var qRangeAttack = new QuestionNode(QuestionAttackRange(), attack,chase);
        var qLoS = new QuestionNode(QuestionLos(), Astar,qRange);
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
        return () => enemy.GetHP() <= 0;
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
}

