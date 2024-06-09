using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatesEnumTres
{
    Idle,
    Dead,
    Steering,
    Waypoints
}

public class SlimeController : MonoBehaviour, ILineOfSight, IBoid
{
    public float distance;
    public Transform player;
    LineOfSight _los;
    FSM<StatesEnumTres> _fsm;
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

    PlayerController playerController;
    AgentController agentController;

    //A-star
    SlimeStateFollowPoints<StatesEnumTres> _stateFollowPoints;
    #region Slime
    Quaternion targetRotation;
    private int HP = 20;
    public float speed;
    public float speedRoot;
    //------------------------
    float timer;
    float chaseRange = 8;
    Rigidbody _rb;

    public Vector3 Position => transform.position;
    public Vector3 Front => transform.forward;

    public GameObject particlePrefab;
    #endregion
    public bool EsLider = false;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (EsLider) agentController = FindObjectOfType<AgentController>(); //Para determinar cuales van a tener path finding y cuales no
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
        if (player == null && target == null) ReassignPlayerAndTarget();
        distance = Vector3.Distance(player.position, transform.position);
        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
    }
    void InitializeSteerings()
    {
        _steering = GetComponent<FlockingManager>();
        _obstacleAvoidance = new ObstacleAvoidance(this.transform, angle, radius, maskObs, 2.5f);
    }
    void InitializeFSM()
    {
        var idle = new SlimeIdleState<StatesEnumTres>(this);
        var dead = new SlimeDeathState<StatesEnumTres>(this, playerController);
        var steering = new SlimeSteeringState<StatesEnumTres>(this, _steering, _obstacleAvoidance);
        _stateFollowPoints = new SlimeStateFollowPoints<StatesEnumTres>(this, agentController);

        idle.AddTransition(StatesEnumTres.Steering, steering);
        idle.AddTransition(StatesEnumTres.Dead, dead);
        idle.AddTransition(StatesEnumTres.Waypoints, _stateFollowPoints);

        dead.AddTransition(StatesEnumTres.Steering, steering);
        dead.AddTransition(StatesEnumTres.Idle, idle);
        dead.AddTransition(StatesEnumTres.Waypoints, _stateFollowPoints);

        steering.AddTransition(StatesEnumTres.Dead, dead);
        steering.AddTransition(StatesEnumTres.Idle, idle);
        steering.AddTransition(StatesEnumTres.Waypoints, _stateFollowPoints);

        _stateFollowPoints.AddTransition(StatesEnumTres.Dead, dead);
        _stateFollowPoints.AddTransition(StatesEnumTres.Idle, idle);
        _stateFollowPoints.AddTransition(StatesEnumTres.Steering, steering);
        
        _fsm = new FSM<StatesEnumTres>(idle);
    }
    void InitializedTree()
    {
        var dead = new ActionNode(() => _fsm.Transition(StatesEnumTres.Dead));
        var steering = new ActionNode(() => _fsm.Transition(StatesEnumTres.Steering));
        var idle = new ActionNode(() => _fsm.Transition(StatesEnumTres.Idle));
        var astar = new ActionNode(() => _fsm.Transition(StatesEnumTres.Waypoints));

        var qFollowPoints = new QuestionNode(() => _stateFollowPoints.ejecutar, astar, idle);
        var qLoS = new QuestionNode(QuestionLosPlayer(), steering, qFollowPoints);
        var isFollower = new QuestionNode(QuestionFollower(), qLoS, steering);
        var qHasLife = new QuestionNode(QuestionHP(), dead, isFollower);
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
    Func<bool> QuestionFollower()
    {
        return () => checkIsFollower();
    }
    Func<bool> QuestionLosPlayer()
    {
        return () => (CheckRange(player) && CheckAngle(player) && CheckView(player));
    }
    Func<bool> QuestionHP()
    {
        return () => HP <= 0;
    }
    public bool checkIsFollower()
    {
        if (EsLider) return true;
        else return false;
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

    #region SlimeFunciones
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
    }

    public void Move(Vector3 dir)
    {
        transform.position += dir * Time.deltaTime * speed;
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
        dir.y = 0; //Añadir un smooth o lerp para los cambios bruscos de direcciones, haga suave el giro
        transform.forward = dir;
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void DestroySlime()
    {
        StartCoroutine(EmitParticlesForTime(1.0f));
    }
    private IEnumerator EmitParticlesForTime(float duration) 
    {
        GameObject particleObject = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        if (particleSystem != null ) 
        {
            particleSystem.Play();
            yield return new WaitForSeconds(duration);
            particleSystem.Stop();
            Destroy(gameObject);
            Destroy(particleObject, particleSystem.main.startLifetime.constantMax);
        }
    }
    public void ReassignPlayerAndTarget()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = player.GetComponent<Rigidbody>();
    }

    #endregion
    //A-Star
    public IPoints GetStateWaypoints => _stateFollowPoints;
}
