using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IdleState<T> : State<T>
{
    T _inputMovement;
    public IdleState(T inputMovement)
    {
        _inputMovement = inputMovement;
    }
    public override void Execute()
    {
        base.Execute();
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //Transicion
        if (x != 0 || z != 0) _fsm.Transition(_inputMovement);
    }
}
