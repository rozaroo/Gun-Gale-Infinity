using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSteering<T> : State<T>
{
    ISteering _steering;
    EnemyController _enemyController;
    ObstacleAvoidance _obs;
    public EnemyStateSteering(EnemyController enemyController, ISteering steering, ObstacleAvoidance obs)
    {
        _steering = steering;
        _enemyController = enemyController;
        _obs = obs;
    }
    public override void Enter()
    {
        _enemyController.animator.SetBool("isChasing", true);
    }
    public override void Sleep()
    {
        base.Sleep();
        _enemyController.animator.SetBool("isChasing", false);
    }
    public override void Execute()
    {
        _enemyController.UpdateHealthBarVisibility();
        var dir = _obs.GetDir(_steering.GetDir(), false);
        _enemyController.Move(dir);
        _enemyController.LookDir(dir);
    }
}
