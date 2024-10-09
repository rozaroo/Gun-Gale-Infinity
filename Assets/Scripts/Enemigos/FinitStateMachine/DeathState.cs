using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState<T> : State<T>
{
    EnemyController _enemyController;
    public LevelManager _levelManager;
    public DeathState(EnemyController enemyController, LevelManager levelManager)
    {
        _enemyController = enemyController;
        _levelManager = levelManager;
    }
    public override void Enter()
    {
        _enemyController.healthBar.gameObject.SetActive(false);
        _enemyController.animator.SetTrigger("die");
        _enemyController.GetComponent<Collider>().enabled = false;
        _enemyController.SpawnRandomDrop();
        _levelManager.Enemies--;
        GameObject.Destroy(_enemyController.gameObject,2f);
    }
}
