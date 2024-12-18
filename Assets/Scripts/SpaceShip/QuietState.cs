using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuietState<T> : State<T>
{
    SpaceShipController _shipController;
    T _inputMovement;
    T _destroyInput;
    public QuietState(SpaceShipController spaceshipcontroller, T inputMovement, T destroyInput)
    {
        _shipController = spaceshipcontroller;
        _inputMovement = inputMovement;
        _destroyInput = destroyInput;
    }
    public override void Execute()
    {
        // Transicion a Destroy sino hay vida
        if (_shipController.currentHealth <= 0) 
        { 
            _fsm.Transition(_destroyInput);
            return;
        }
        //MoveLogic
        base.Execute();
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //Disparar
        if (Input.GetMouseButtonDown(0)) _shipController.Shoot();
        //Transicion
        if (x != 0 || z != 0) _fsm.Transition(_inputMovement);
    }
}
