using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState<T> : State<T>
{
    PlayerController _playerController;
    T _idleInput;
    public WalkState(PlayerController playerController, T idleInput)
    {
        _playerController = playerController;
        _idleInput = idleInput;
    }
    public override void Enter()
    {
    
    }
    public override void Sleep()
    {
        base.Sleep();
    }
    public override void Execute()
    {
        if (_playerController.inventoryOpen == true) return;
        Vector3 direction = _playerController.playerRb.velocity;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float theTime = Time.deltaTime;
        _playerController.newDirection = new Vector2(moveX, moveZ);

        //Vector3 movementDirection = new Vector3(moveX, 0, moveZ).normalized;
        //Vector3 velocity = movementDirection * playerSpeed * Time.deltaTime;

        Vector3 side = _playerController.playerSpeed * moveX * theTime * _playerController.playerTr.right;
        Vector3 forward = _playerController.playerSpeed * moveZ * theTime * _playerController.playerTr.forward;
        Vector3 endDirection = side + forward;
        _playerController.playerRb.velocity = endDirection;
        //Transicion
        if (moveX == 0 && moveZ == 0) _fsm.Transition(_idleInput);
    }
}
