using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraState<T> : State<T>
{
    /*
    private PlayerController _playerController;
    private T _idleState;

    public CameraState(PlayerController playerController, T idleState)
    {
        _playerController = playerController;
        _idleState = idleState;
    }

    public override void Execute()
    {
        CameraLogic();
    } 
    public void CameraLogic()
    {
        
        if (_playerController.inventoryOpen == true) return;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float theTime = Time.deltaTime;
        _playerController.rotY += mouseY * theTime * 200f;
        _playerController.rotX = mouseX * theTime * 200f;
        _playerController.playerTr.Rotate(0, _playerController.rotX, 0); //Para que rote con la camara
        _playerController.rotY = Mathf.Clamp(_playerController.rotY, _playerController.minAngle, _playerController.maxAngle);
        Quaternion localRotation = Quaternion.Euler(-_playerController.rotY, 0, 0);
        _playerController.cameraAxis.localRotation = localRotation;
        if (_playerController.hasPistol || _playerController.hasRiffle || _playerController.hasGrenade)
        {
            _playerController.cameraTrack.gameObject.SetActive(false);
            _playerController.cameraWeaponTrack.gameObject.SetActive(true);
            _playerController.crosshair.gameObject.SetActive(true);
            _playerController.theCamera.position = Vector3.Lerp(_playerController.theCamera.position, _playerController.cameraWeaponTrack.position, _playerController.cameraSpeed * theTime);
            _playerController.theCamera.rotation = Quaternion.Lerp(theCamera.rotation, cameraWeaponTrack.rotation, _playerController.cameraSpeed * theTime);
        }
        else
        {
            _playerController.cameraTrack.gameObject.SetActive(true);
            _playerController.cameraWeaponTrack.gameObject.SetActive(false);
            _playerController.theCamera.position = Vector3.Lerp(_playerController.theCamera.position, _playerController.cameraTrack.position, _playerController.cameraSpeed * theTime);
            _playerController.theCamera.rotation = Quaternion.Lerp(_playerController.theCamera.rotation, _playerController.cameraTrack.rotation, _playerController.cameraSpeed * theTime);
        }
        if (!_playerController.inventoryOpen) 
        {
            _playerController._fsm.Transition(_idleState);
        }
    }*/
}
