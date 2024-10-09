using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttackState<T> : State<T>
{
    EnemyController _enemyController;
    Transform _player;
    public NewAttackState(EnemyController enemyController, Transform player)
    {
        _enemyController = enemyController;
        _player = player;
    }
    public override void Enter()
    {
        _enemyController.UpdateHealthBarVisibility();
        _enemyController.animator.SetBool("isAttacking", true);
    }
    public override void Sleep() 
    {
        base.Sleep();
        _enemyController.animator.SetBool("isAttacking", false);
    }
}
