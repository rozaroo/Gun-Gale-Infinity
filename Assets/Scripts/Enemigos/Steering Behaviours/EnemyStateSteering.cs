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
        float distance = Vector3.Distance(_enemy.player.position, _enemy.transform.position);
        Vector3 playerDirection = (_enemy.player.position - _enemy.transform.position).normalized;
        playerDirection.y = 0;

        //var dir = _obs.GetDir(_steering.GetDir());
        _enemy.Movetwo(playerDirection);
        _enemy.LookDir(playerDirection);
        //Rotación al Jugador 
        Quaternion targetRotation = Quaternion.LookRotation(_enemy.player.position);
        _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
