using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

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
    EnemyTwo enemytwo;
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

    private void Awake()
    {
        enemytwo = GetComponent<EnemyTwo>();
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
        var evade = new Evade(enemytwo.transform, target, timePrediction);
        _steering = evade;
        _obstacleAvoidance = new ObstacleAvoidance(enemytwo.transform, angle, radius, maskObs, 2.5f);
    }
    void InitializeFSM()
    {
        var idle = new EnemyIdleState<StatesEnumDos>(enemytwo);
        var dead = new DeathStateTwo<StatesEnumDos>(enemytwo, lvlManager);
        var steering = new EnemyStateSteeringTwo<StatesEnumDos>(enemytwo, _steering, _obstacleAvoidance);
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
        return () => enemytwo.GetHP() <= 0;
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

}
