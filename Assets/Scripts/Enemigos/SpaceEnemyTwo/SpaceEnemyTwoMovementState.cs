using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnemyTwoMovementState<T> : State<T>
{
    SpaceEnemyTwoController _spaceEnemyController;
    ISteering _steering;
    ObstacleAvoidance _obs;
    public SpaceEnemyTwoMovementState(SpaceEnemyTwoController spacenemycontroller, ISteering steering, ObstacleAvoidance obs)
    {
        _spaceEnemyController = spacenemycontroller;
        _steering = steering;
        _obs = obs;
    }
    public override void Sleep()
    {
        base.Sleep();
    }
    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir(), false);
        _spaceEnemyController.Move(dir);
        _spaceEnemyController.LookDir(dir);
    }
}
