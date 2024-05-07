using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttackState<T> : State<T>
{
    public Enemy _enemy;
    Transform _player;
    public NewAttackState(Enemy enemy, Transform player)
    {
        _enemy = enemy;
        _player = player;
    }
    public override void Enter()
    {
        _enemy.animator.SetBool("isAttacking", true);
    }
    public override void Sleep() 
    {
        base.Sleep();
        _enemy.animator.SetBool("isAttacking", false);
    }
}
