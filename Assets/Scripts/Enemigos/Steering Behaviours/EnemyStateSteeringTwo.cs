using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSteeringTwo<T> : State<T>
{
    ISteering _steering;
    EnemyTwo _enemytwo;
    ObstacleAvoidance _obs;
    public EnemyStateSteeringTwo(EnemyTwo enemytwo, ISteering steering, ObstacleAvoidance obs)
    {
        _steering = steering;
        _enemytwo = enemytwo;
        _obs = obs;
    }
    public override void Enter()
    {
        _enemytwo.animator.SetBool("IsRunning", true);
    }
    public override void Sleep()
    {
        base.Sleep();
        _enemytwo.animator.SetBool("IsRunning", false);
    }
    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir(), false);
        _enemytwo.Move(dir);
        _enemytwo.LookDir(dir);
        Debug.Log("Huyendo");
    }
}
