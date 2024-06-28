using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : State<T>
{
    PlayerController _playerController;
    T _inputMovement;
    public IdleState(PlayerController playerController, T inputMovement)
    {
        _playerController = playerController;
        _inputMovement = inputMovement;
    }
    public override void Execute()
    {
        //MoveLogic
        base.Execute();
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //Transicion
        if (x != 0 || z != 0) _fsm.Transition(_inputMovement);

        //CameraLogic
        if (_playerController.Player)
        {
            if (_playerController.inventoryOpen == true) return;
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            float theTime = Time.deltaTime;
            _playerController.rotY += mouseY * theTime * _playerController.playervalues.CamRotSpeed[0];
            _playerController.rotX = mouseX * theTime * _playerController.playervalues.CamRotSpeed[0];
            _playerController.playerTr.Rotate(0, _playerController.rotX, 0); //Para que rote con la camara
            _playerController.rotY = Mathf.Clamp(_playerController.rotY, _playerController.playervalues.MinAngle[0], _playerController.playervalues.MaxAngle[0]);
            Quaternion localRotation = Quaternion.Euler(-_playerController.rotY, 0, 0);
            _playerController.cameraAxis.localRotation = localRotation;
            if (_playerController.hasPistol || _playerController.hasRiffle || _playerController.hasGrenade)
            {
                _playerController.cameraTrack.gameObject.SetActive(false);
                _playerController.cameraWeaponTrack.gameObject.SetActive(true);
                _playerController.crosshair.gameObject.SetActive(true);
                _playerController.theCamera.position = Vector3.Lerp(_playerController.theCamera.position, _playerController.cameraWeaponTrack.position, _playerController.playervalues.CameraSpeed[0] * theTime);
                _playerController.theCamera.rotation = Quaternion.Lerp(_playerController.theCamera.rotation, _playerController.cameraWeaponTrack.rotation, _playerController.playervalues.CameraSpeed[0] * theTime);
            }
            else
            {
                _playerController.cameraTrack.gameObject.SetActive(true);
                _playerController.cameraWeaponTrack.gameObject.SetActive(false);
                _playerController.theCamera.position = Vector3.Lerp(_playerController.theCamera.position, _playerController.cameraTrack.position, _playerController.playervalues.CameraSpeed[0] * theTime);
                _playerController.theCamera.rotation = Quaternion.Lerp(_playerController.theCamera.rotation, _playerController.cameraTrack.rotation, _playerController.playervalues.CameraSpeed[0] * theTime);
            }
        }
        
        //ItemLogic
        if (_playerController.nearItem != null && Input.GetKeyDown(KeyCode.E)) 
        {
            GameObject instantiatedItem = null;
            int countWeapons = 0;
            foreach (GameObject itemPrefab in _playerController.itemPrefab)
            {
                if (itemPrefab.CompareTag("PW") && _playerController.nearItem.CompareTag("PW"))
                {
                    instantiatedItem = GameObject.Instantiate(itemPrefab, _playerController.itemSlot.position, _playerController.itemSlot.rotation);
                    _playerController.primaryWeapon = instantiatedItem.gameObject;
                    countWeapons++;
                    _playerController.weapons++;
                    GameObject.Destroy(_playerController.nearItem.gameObject);
                    instantiatedItem.transform.parent = _playerController.primarySlot;
                    _playerController.nearItem = null;
                    WeaponController pwIcon = instantiatedItem.GetComponentInChildren<WeaponController>();
                    _playerController.primaryWeaponIcon.sprite = pwIcon.weaponIcon;
                    _playerController.primaryWeaponIcon.gameObject.SetActive(true);
                    break;
                }
                else if (itemPrefab.CompareTag("SW") && _playerController.nearItem.CompareTag("SW"))
                {
                    instantiatedItem = GameObject.Instantiate(itemPrefab, _playerController.itemSlot.position, _playerController.itemSlot.rotation);
                    _playerController.secondaryWeapon = instantiatedItem.gameObject;
                    countWeapons++;
                    _playerController.weapons++;
                    GameObject.Destroy(_playerController.nearItem.gameObject);
                    instantiatedItem.transform.parent = _playerController.secondarySlot;
                    _playerController.nearItem = null;
                    WeaponController swIcon = instantiatedItem.GetComponentInChildren<WeaponController>();
                    _playerController.secondaryWeaponIcon.sprite = swIcon.weaponIcon;
                    _playerController.secondaryWeaponIcon.gameObject.SetActive(true);
                    break;
                }
                else if (itemPrefab.CompareTag("TW") && _playerController.nearItem.CompareTag("TW"))
                {
                    instantiatedItem = GameObject.Instantiate(itemPrefab, _playerController.itemSlot.position, _playerController.itemSlot.rotation);
                    _playerController.throwableWeapon = instantiatedItem.gameObject;
                    countWeapons++;
                    _playerController.weapons++;
                    GameObject.Destroy(_playerController.nearItem.gameObject);
                    instantiatedItem.transform.parent = _playerController.throwableSlot;
                    _playerController.nearItem = null;
                    GrenadeController twIcon = instantiatedItem.GetComponentInChildren<GrenadeController>();
                    _playerController.throwableWeaponIcon.sprite = twIcon.weaponIcon;
                    _playerController.throwableWeaponIcon.gameObject.SetActive(true);
                    break;
                }
                else if (itemPrefab.CompareTag("Botiquin") && _playerController.nearItem.CompareTag("Botiquin"))
                {
                    _playerController.RecoveryHealth(25);
                    GameObject.Destroy(_playerController.nearItem.gameObject);
                }
            }
        }

        //AnimLogic
        _playerController.playerAnim.SetFloat("X", _playerController.newDirection.x);
        _playerController.playerAnim.SetFloat("Y", _playerController.newDirection.y);
        _playerController.playerAnim.SetBool("holdPistol", _playerController.hasPistol);
        _playerController.playerAnim.SetBool("holdRiffle", _playerController.hasRiffle);
        _playerController.playerAnim.SetBool("holdGrenade", _playerController.hasGrenade);
        if (_playerController.hasPistol || _playerController.hasRiffle)
        {
            _playerController.playerAnim.SetLayerWeight(2, 0);
            _playerController.playerAnim.SetLayerWeight(1, 1);
        }
        else if (_playerController.hasGrenade)
        {
            _playerController.playerAnim.SetLayerWeight(1, 0);
            _playerController.playerAnim.SetLayerWeight(2, 1);
        }
        if (!_playerController.Active) return;
    }
}
