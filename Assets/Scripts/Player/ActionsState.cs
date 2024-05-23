using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsState<T> : State<T>
{
    private PlayerController _playerController;
    private T _idleState;
    public ActionsState(PlayerController playerController, T idleState)
    {
        _playerController = playerController;
        _idleState = idleState;
    }
    public override void Sleep()
    {
        base.Sleep();
    }
    public override void Execute()
    {
        ActionsLogic();
    }

    public void ActionsLogic()
    {
        //Inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _playerController.inventoryOpen = !_playerController.inventoryOpen;
            Cursor.lockState = _playerController.inventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _playerController.inventoryOpen;
        }
        _playerController.inventoryController.gameObject.SetActive(_playerController.inventoryOpen);
        if (Input.GetKeyDown(KeyCode.G)) _playerController.Drop();
        //Transicion
        if (Input.GetKeyDown(KeyCode.Tab) && !_playerController.inventoryOpen) _fsm.Transition(_idleState);
    }

}
