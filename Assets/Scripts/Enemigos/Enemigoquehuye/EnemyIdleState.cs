using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    EnemyControllerTwo _enemycontrollertwo;

    public EnemyIdleState(EnemyControllerTwo enemycontrollertwo)
    {
        _enemycontrollertwo = enemycontrollertwo;
    }
    public override void Enter()
    {
        _enemycontrollertwo.animator.SetTrigger("Idle");
    }
    public override void Sleep()
    {
        base.Sleep();
    }
    public override void Execute()
    {
        _enemycontrollertwo.animator.SetTrigger("Idle");
    }
}
