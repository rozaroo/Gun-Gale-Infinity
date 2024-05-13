using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState<T> : State<T>
{
    public Enemy _enemy;
    public LevelManager _levelManager;
    public DeathState(Enemy enemy, LevelManager levelManager)
    {
        _enemy = enemy;
        _levelManager = levelManager;
    }
    public override void Enter()
    {
        _enemy.animator.SetTrigger("die"); 
        _enemy.GetComponent<Collider>().enabled = false;
        _enemy.SpawnRandomDrop();
        _levelManager.Enemies--;
        GameObject.Destroy(_enemy.gameObject,2f);
    }
}
