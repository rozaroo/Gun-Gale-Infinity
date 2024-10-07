using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnemyTwoDeathState<T> : State<T>
{
    SpaceEnemyTwoController _spaceEnemyTwoController;
    ShipLevelManager _lvlManager;
    public SpaceEnemyTwoDeathState(SpaceEnemyTwoController spacenemytwocontroller, ShipLevelManager lvlmanager)
    {
        _spaceEnemyTwoController = spacenemytwocontroller;
        _lvlManager = lvlmanager;
    }
    public override void Enter()
    {
        _lvlManager.Enemies--;
        _spaceEnemyTwoController.DestroyShip();
    }
}
