using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState<T> : State<T>
{
    Slime _slime;
    public SlimeIdleState(Slime slime)
    {
        _slime = slime;
    }
    public override void Sleep()
    {
        base.Sleep();
    }
}
