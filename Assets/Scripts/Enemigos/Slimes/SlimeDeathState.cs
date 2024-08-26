using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeathState<T> : State<T>
{
    SlimeController _slimecontroller;
    public PlayerController _playerController;
    public SlimeDeathState(SlimeController slimecontroller, PlayerController playerController)
    {
        _slimecontroller = slimecontroller;
        _playerController = playerController;
    }
    public override void Enter()
    {
        _playerController.RecoveryHealth(20);
        _slimecontroller.DestroySlime();
    }
}
