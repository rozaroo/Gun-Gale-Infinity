using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSteeringTwo<T> : State<T>
{
    ISteering _steering;
    EnemyControllerTwo _enemycontrollertwo;
    ObstacleAvoidance _obs;
    public EnemyStateSteeringTwo(EnemyControllerTwo enemycontrollertwo, ISteering steering, ObstacleAvoidance obs)
    {
        _steering = steering;
        _enemycontrollertwo = enemycontrollertwo;
        _obs = obs;
    }
    public override void Enter()
    {
        _enemycontrollertwo.animator.SetBool("IsRunning", true);
    }
    public override void Sleep()
    {
        base.Sleep();
        _enemycontrollertwo.animator.SetBool("IsRunning", false);
    }
    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir(), false);
        _enemycontrollertwo.Move(dir);
        _enemycontrollertwo.LookDir(dir);
    }
}
