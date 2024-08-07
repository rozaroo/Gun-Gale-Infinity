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
        //ActionsLogic
        if (_playerController.inventoryOpen == false) _playerController.inventoryController.gameObject.SetActive(false);
        else _playerController.inventoryController.gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.G)) _playerController.Drop();
        if (Input.GetKeyDown(KeyCode.Escape)) _playerController.QuitGame();

        //MoveLogic
        if (_playerController.inventoryOpen == true) return;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(moveX, 0, moveZ).normalized;
        float theTime = Time.deltaTime;
        _playerController.newDirection = new Vector2(moveX, moveZ);

        //Vector3 movementDirection = new Vector3(moveX, 0, moveZ).normalized;
        //Vector3 velocity = movementDirection * playerSpeed * Time.deltaTime;

        Vector3 side = _playerController.playervalues.Speed[0] * moveX * theTime * _playerController.playerTr.right;
        Vector3 forward = _playerController.playervalues.Speed[0] * moveZ * theTime * _playerController.playerTr.forward;
        Vector3 endDirection = side + forward;
        _playerController.playerRb.velocity = endDirection;
        //Transicion
        if (moveX == 0 && moveZ == 0) _fsm.Transition(_idleInput);

        //CameraLogic
        if (_playerController.Player)
        {
            if (_playerController.inventoryOpen == true) return;
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
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
            foreach (GameObject itemPrefab in _playerController.playervalues.ItemPrefab)
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
        //Activacion de Post-Procesos
        if (_playerController.currentHealth <= 25f)
        {
            _playerController.postProcessController.ActivateShader();
            _playerController.grayscale.ActivateShader();
        }
        if (_playerController.currentHealth > 25f)
        {
            _playerController.postProcessController.DesactivateShader();
            _playerController.grayscale.DesactivateShader();
        }
    }
}