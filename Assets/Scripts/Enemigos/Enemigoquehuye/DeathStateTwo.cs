using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStateTwo<T> : State<T>
{
    EnemyControllerTwo _enemycontrollertwo;
    public LevelManager _levelManager;
    public DeathStateTwo(EnemyControllerTwo enemycontrollertwo, LevelManager levelManager)
    {
        _enemycontrollertwo = enemycontrollertwo;
        _levelManager = levelManager;
    }
    public override void Enter()
    {
        _enemycontrollertwo.animator.SetTrigger("die");
        _enemycontrollertwo.GetComponent<Collider>().enabled = false;
        _enemycontrollertwo.SpawnRandomDrop();
        _levelManager.Enemies--;
        GameObject.Destroy(_enemycontrollertwo.gameObject, 2f);
    }
}
