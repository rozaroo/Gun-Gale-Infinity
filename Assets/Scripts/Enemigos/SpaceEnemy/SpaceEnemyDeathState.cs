using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnemyDeathState<T> : State<T>
{
    SpaceEnemyController _spaceEnemyController;
    ShipLevelManager _lvlManager;
    public SpaceEnemyDeathState(SpaceEnemyController spacenemycontroller, ShipLevelManager lvlmanager)
    {
        _spaceEnemyController = spacenemycontroller;
        _lvlManager = lvlmanager;
    }
    public override void Enter()
    {
        _lvlManager.DecreaseEnemyCount();
        _spaceEnemyController.DestroyShip();
    }
}
