using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSteering<T> : State<T>
{
    ISteering _steering;
    Enemy _enemy;
    ObstacleAvoidance _obs;
    public EnemyStateSteering(Enemy enemy, ISteering steering, ObstacleAvoidance obs)
    {
        _steering = steering;
        _enemy = enemy;
        _obs = obs;
    }
    public override void Enter()
    {
        _enemy.animator.SetBool("isChasing", true);
    }
    public override void Sleep()
    {
        base.Sleep();
        _enemy.animator.SetBool("isChasing", false);
    }
    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir(), false);
        _enemy.Movetwo(dir);
        _enemy.LookDir(dir);
        //Rotación al Jugador 
        Quaternion targetRotation = Quaternion.LookRotation(_enemy.player.position);
        _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
