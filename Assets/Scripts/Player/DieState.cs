using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState<T> : State<T>
{
    PlayerController _playerController;
    public DieState(PlayerController playerController)
    {
        _playerController = playerController;
    }
    public override void Enter()
    {
        _playerController.Die();
    }
}
