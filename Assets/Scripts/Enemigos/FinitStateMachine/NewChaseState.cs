using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewChaseState<T> : State<T>
{
    public Enemy _enemy;
    Transform _player;
    ILineOfSight _los;

    public NewChaseState(Enemy enemy)
    {
        _enemy = enemy;
        _player = _enemy.player;
        _los = _enemy._los;
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
        if (!_los.CheckAngle(_player) || !_los.CheckRange(_player) || !_los.CheckView(_player)) 
        {
            _enemy.scriptalerta.Alert = false;
            NewPatrolState<Enemy> nuevoestadopatrllar = new NewPatrolState<Enemy>(_enemy);
            _enemy.stateMachine.TransitionToState(nuevoestadopatrllar._enemy);
        }
        if (distance < _enemy.lineOfSight.range - 5f)
        {
            Vector3 playerDirection = (_player.position - _enemy.transform.position).normalized;
            playerDirection.y = 0;
            Move(playerDirection);
            _enemy.scriptalerta.Alert = true;
        }
    }
    public void Move(Vector3 direction)
    {
        _enemy.transform.position += direction * Time.deltaTime * 3;
    }
}
