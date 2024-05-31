using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState<T> : State<T>
{
    SlimeController _slimecontroller;
    public SlimeIdleState(SlimeController slimecontroller)
    {
        _slimecontroller = slimecontroller;
    }
    public override void Sleep()
    {
        base.Sleep();
    }
}
