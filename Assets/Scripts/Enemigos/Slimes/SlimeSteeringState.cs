using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSteeringState<T> : State<T>
{
    ISteering _steering;
    Slime _slime;
    ObstacleAvoidance _obs;
    public SlimeSteeringState(Slime slime, ISteering steering, ObstacleAvoidance obs)
    {
        _steering = steering;
        _slime = slime;
        _obs = obs;
    }
   
    public override void Sleep()
    {
        base.Sleep();
    }
    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir(), false);
        _slime.Move(dir);
        _slime.LookDir(dir);
    }
}
