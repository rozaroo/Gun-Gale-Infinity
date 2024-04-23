using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewChaseState<T> : State<T>
{
    Enemy _enemy;
    Transform _player;
    ILineOfSight _los;

    public NewChaseState(Enemy enemy, Transform player)
    {
        _enemy = enemy;
        _player = player;
        _los = enemy.Los;
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
        float distance = Vector3.Distance(_player.position, _enemy.transform.position);
        if (!_los.CheckAngle(_player) || !_los.CheckRange(_player) || !_los._CheckView(_player)) 
        {
            _enemy.scriptalerta.Alert = false;
            _enemy.TransitionToState((T)(object)new NewPatrolState<Enemy>(_enemy));
        }
        if (distance < _enemy.lineOfSight.range - 5f)
        {
            Vector3 playerDirection = (_player.position - _enemy.transform.position).normalized;
            playerDirection.y = 0;
            _enemy.Move(playerDirection);
            _enemy.scriptalerta.Alert = true;
        }
    }
}
