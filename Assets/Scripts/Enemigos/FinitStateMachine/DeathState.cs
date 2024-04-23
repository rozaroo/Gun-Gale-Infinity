using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState<T> : State<T>
{
    Enemy _enemy;
    public DeathState(Enemy enemy)
    {
        _enemy = enemy;
    }
    public override void Enter()
    {
        _enemy.animator.SetTrigger("die"); 
        _enemy.GetComponent<Collider>().enabled = false;
        _enemy.SpawnRandomDrop();
        GameObject.Destroy(_enemy.gameObject,2f);
    }
}
