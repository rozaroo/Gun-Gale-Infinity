using System;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public enum StatesEnum
{
    Patroll,
    Chase,
    Attack,
    Dead
}

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float attackRange;
    LineOfSight _los;
    FSM<StatesEnum> _fsm;
    Enemy enemy;
    ITreeNode _root;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        _los = GetComponent<LineOfSight>();
    }
    private void Start()
    {
        InitializeFSM();
        InitializedTree();
    }
    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        _fsm.OnUpdate();
        _root.Execute();
    }
    void InitializeFSM()
    {
        var dead = new DeathState<StatesEnum>(enemy);
        var attack = new NewAttackState<StatesEnum>(enemy);
        var chase = new NewChaseState<StatesEnum>(enemy, player);
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
        var qAttackRange = new QuestionNode(QuestionRange = new QuestionNode(QuestionAttackRange, attack, chase));
        var qLoS = new QuestionNode(!QuestionLoS, patrol);
        var qHasLife = new QuestionNode(() => enemy.Life > 0, dead);
        _root = qHasLife;
    }

    bool QuestionAttackRange()
    {
        return distance < (_los.range - 5f);
    }
    bool QuestionLos()
    {
        return _los.CheckRange(player) && _los.CheckAngle(player) && _los.CheckView(player);
    }


}
