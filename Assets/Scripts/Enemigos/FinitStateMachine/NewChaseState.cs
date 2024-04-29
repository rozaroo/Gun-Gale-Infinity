using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewChaseState<T> : State<T>
{
    public Enemy _enemy;
    EnemyController _enemyController;
    Transform _player;
   // LineOfSight _los;

    public NewChaseState(EnemyController enemyController, Enemy enemy)
    {
        _enemy = enemy;
        _enemyController = enemyController;
       // _los = los;
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
        float distance = Vector3.Distance(_enemyController.player.position, _enemy.transform.position);
        Vector3 playerDirection = (_enemyController.player.position - _enemy.transform.position).normalized;
        playerDirection.y = 0;
        Move(playerDirection);
        _enemy.scriptalerta.Alert = true;
    }
    public void Move(Vector3 direction)
    {
        _enemy.transform.rotation = Quaternion.LookRotation(direction);
        _enemy.transform.position += direction * Time.deltaTime * 3;
    }
}
