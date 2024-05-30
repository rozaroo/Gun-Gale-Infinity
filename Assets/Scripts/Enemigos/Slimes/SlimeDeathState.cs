using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeathState<T> : State<T>
{
    public Slime _slime;
    public PlayerController _playerController;
    public SlimeDeathState(Slime slime, PlayerController playerController)
    {
        _slime = slime;
        _playerController = playerController;
    }
    public override void Enter()
    {
        _playerController.RecoveryHealth(1);
        _slime.DestroySlime();
    }
}
