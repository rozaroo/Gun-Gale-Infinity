using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    private float attackRange;
    LineOfSight _los;
    FSM<StatesEnum> _fsm;
    FakeCrash _model;
    ITreeNode _root;
    private void Awake()
    {
        _model = GetComponent<FakeCrash>();
        _los = GetComponent<LineOfSight>();
    }
    private void Start()
    {
        InitializeFSM();
        InitializedTree();
    }
    void InitializeFSM()
    {
        var idle = new FakeIdleState<StatesEnum>();
        var dead = new FakeDeadState<StatesEnum>(_model);
        var attack = new FakeAttackState<StatesEnum>(_model);
        var chase = new FakeChaseState<StatesEnum>(_model, player);

        idle.AddTransition(StatesEnum.Dead, dead);
        idle.AddTransition(StatesEnum.Attack, attack);
        idle.AddTransition(StatesEnum.Chase, chase);

        dead.AddTransition(StatesEnum.Idle, idle);
        dead.AddTransition(StatesEnum.Attack, attack);
        dead.AddTransition(StatesEnum.Chase, chase);

        attack.AddTransition(StatesEnum.Idle, idle);
        attack.AddTransition(StatesEnum.Dead, dead);
        attack.AddTransition(StatesEnum.Chase, chase);

        chase.AddTransition(StatesEnum.Idle, idle);
        chase.AddTransition(StatesEnum.Dead, dead);
        chase.AddTransition(StatesEnum.Attack, attack);

        _fsm = new FSM<StatesEnum>(idle);
    }
    void InitializedTree()
    {
        //Actions
        var idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        var dead = new ActionNode(() => _fsm.Transition(StatesEnum.Dead));
        var attack = new ActionNode(() => _fsm.Transition(StatesEnum.Attack));
        var chase = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));

        //Question
        var qIsCooldown = new QuestionNode(() => _model.IsCooldown, idle, attack);
        var qIsCooldownOutOfRange = new QuestionNode(() => _model.IsCooldown, idle, chase);
        var qAttackRange = new QuestionNode(QuestionAttackRange, qIsCooldown, qIsCooldownOutOfRange);
        var qLoS = new QuestionNode(QuestionLoS, qAttackRange, idle);
        var qHasLife = new QuestionNode(() => _model.Life > 0, qLoS, dead);

        _root = qHasLife;
    }
    bool QuestionAttackRange()
    {
        return _los.CheckRange(player, attackRange);
    }
    bool QuestionLoS()
    {
        return _los.CheckRange(player)
            && _los.CheckAngle(player)
            && _los.CheckView(player);
    }
    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }
}
