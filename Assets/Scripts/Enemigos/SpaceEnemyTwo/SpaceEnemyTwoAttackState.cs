using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnemyTwoAttackState<T> : State<T>
{
    SpaceEnemyTwoController _spaceEnemyController;
    ShipLevelManager _lvlManager;
    public SpaceEnemyAttackState(SpaceEnemyTwoController spacenemycontroller, ShipLevelManager lvlmanager)
    {
        _spaceEnemyController = spacenemycontroller;
        _lvlManager = lvlmanager;
    }
    public override void Enter()
    {
        _lvlManager.Enemies--;
        _spaceEnemyController.Attack();
    }
}
