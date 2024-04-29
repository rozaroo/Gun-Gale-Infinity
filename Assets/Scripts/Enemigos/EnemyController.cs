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
    public float distance;
    public Transform player;
    public float attackRange;
    LineOfSight _los;
    FSM<StatesEnum> _fsm;
    Enemy enemy;
    ITreeNode _root;
    Func<bool> QuestionRange;
    QuestionNode auxiliarnode;
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
        distance = Vector3.Distance(player.position, transform.position);
        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
    }
    void InitializeFSM()
    {
        var dead = new DeathState<StatesEnum>(enemy);
        var attack = new NewAttackState<StatesEnum>(enemy,player);
        var chase = new NewChaseState<StatesEnum>(enemy);
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
        auxiliarnode = new QuestionNode(QuestionAttackRange(), attack, chase);
        QuestionRange = auxiliarnode._question;
        var qRange = new QuestionNode(QuestionLosPlayer(), chase);
        var qRangeAttack = new QuestionNode(QuestionAttackRange(), attack);
        var qLoS = new QuestionNode(QuestionLos(), patrol);
        var qHasLife = new QuestionNode(QuestionHP(), dead);
        _root = qHasLife;
    }

    Func<bool> QuestionAttackRange()
    {
        Func<bool> resu;
        resu = () => distance < (_los.range - 5f);
        return resu;
    }
    Func<bool> QuestionLos()
    {
        Func<bool> resu;
        resu = () => !(_los.CheckRange(enemy.player) && _los.CheckAngle(enemy.player) && _los.CheckView(enemy.player));
        return resu;
    }
    Func<bool> QuestionLosPlayer()
    {
        Func<bool> resu;
        resu = () => (_los.CheckRange(enemy.player) && _los.CheckAngle(enemy.player) && _los.CheckView(enemy.player));
        return resu;
    }
    Func<bool> QuestionHP()
    {
        Func<bool> resu;
        resu = () => enemy.GetHP() <= 0;
        return resu;
    }

}
