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
    Dead
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

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InitializeFSM();
        InitializedTree();
    }
    private void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
    }
    void InitializeFSM()
    {
        var dead = new DeathState<StatesEnum>(enemy);
        var attack = new NewAttackState<StatesEnum>(enemy,player);
        var chase = new NewChaseState<StatesEnum>(this,enemy);
        var patroll = new NewPatrolState<StatesEnum>(enemy);

        patroll.AddTransition(StatesEnum.Dead, dead);
        patroll.AddTransition(StatesEnum.Attack, attack);
        patroll.AddTransition(StatesEnum.Chase, chase);
        
        dead.AddTransition(StatesEnum.Patroll, patroll);
        dead.AddTransition(StatesEnum.Attack, attack);
        dead.AddTransition(StatesEnum.Chase, chase);

        attack.AddTransition(StatesEnum.Dead, dead);
        attack.AddTransition(StatesEnum.Chase, chase);
        attack.AddTransition(StatesEnum.Patroll, patroll);

        chase.AddTransition(StatesEnum.Dead, dead);
        chase.AddTransition(StatesEnum.Attack, attack);
        chase.AddTransition(StatesEnum.Patroll, patroll);

        _fsm = new FSM<StatesEnum>(patroll);
    }
    void InitializedTree()
    {
        var dead = new ActionNode(() => _fsm.Transition(StatesEnum.Dead));
        var attack = new ActionNode(() => _fsm.Transition(StatesEnum.Attack));
        var chase = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));
        var patrol = new ActionNode(() => _fsm.Transition(StatesEnum.Patroll));

        //Preguntas 
        //auxiliarnode = new QuestionNode(QuestionAttackRange(), attack, chase);
        //QuestionRange = auxiliarnode._question;
        var qRange = new QuestionNode(QuestionAttackRange(), attack,chase);
        //var qRangeAttack = new QuestionNode(QuestionAttackRange(), attack,chase);
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
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, angle / 2, 0) * Forward * range);
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, -(angle / 2), 0) * Forward * range);
    }
#endregion
}
