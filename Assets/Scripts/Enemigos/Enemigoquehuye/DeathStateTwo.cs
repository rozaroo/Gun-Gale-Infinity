using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStateTwo<T> : State<T>
{
    public EnemyTwo _enemytwo;
    public LevelManager _levelManager;
    public DeathStateTwo(EnemyTwo enemytwo, LevelManager levelManager)
    {
        _enemytwo = enemytwo;
        _levelManager = levelManager;
    }
    public override void Enter()
    {
        _enemytwo.animator.SetTrigger("die");
        _enemytwo.GetComponent<Collider>().enabled = false;
        _enemytwo.SpawnRandomDrop();
        _levelManager.Enemies--;
        GameObject.Destroy(_enemytwo.gameObject, 2f);
    }
}
