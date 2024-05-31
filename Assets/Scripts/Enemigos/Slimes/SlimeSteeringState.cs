using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSteeringState<T> : State<T>
{
    ISteering _steering;
    SlimeController _slimecontroller;
    ObstacleAvoidance _obs;
    public SlimeSteeringState(SlimeController slimecontroller, ISteering steering, ObstacleAvoidance obs)
    {
        _steering = steering;
        _slimecontroller = slimecontroller;
        _obs = obs;
    }
   
    public override void Sleep()
    {
        base.Sleep();
    }
    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir(), false);
        _slimecontroller.Move(dir);
        _slimecontroller.LookDir(dir);
    }
}
