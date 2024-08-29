using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnemyDeathState<T> : State<T>
{
    SpaceEnemyController _spaceEnemyController;
    public SpaceEnemyDeathState(SpaceEnemyController spacenemycontroller)
    {
        _spaceEnemyController = spacenemycontroller;
    }
    public override void Enter()
    {
        _spaceEnemyController.DestroyShip();
    }
}
