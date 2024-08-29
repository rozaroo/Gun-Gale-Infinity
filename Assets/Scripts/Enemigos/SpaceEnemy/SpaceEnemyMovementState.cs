using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnemyMovementState<T> : State<T>
{
    SpaceEnemyController _spaceEnemyController;
    public SpaceEnemyMovementState(SpaceEnemyController spacenemycontroller)
    {
        _spaceEnemyController = spacenemycontroller;
    }
    public override void Execute()
    {
        
    }
}
