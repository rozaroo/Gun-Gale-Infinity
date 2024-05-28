using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    EnemyTwo _enemytwo;

    public EnemyIdleState(EnemyTwo enemytwo)
    {
        _enemytwo = enemytwo;
    }
    public override void Enter()
    {
        _enemytwo.animator.SetTrigger("Idle");
    }
    public override void Sleep()
    {
        base.Sleep();
    }
    public override void Execute()
    {
        _enemytwo.animator.SetTrigger("Idle");
        Debug.Log("Idle");
    }
}
