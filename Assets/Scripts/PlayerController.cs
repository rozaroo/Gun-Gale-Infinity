using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Personaje
    Transform playerTr;
    Rigidbody playerRb;
    Animator playerAnim;
    public float playerSpeed = 0f;
    private Vector2 newDirection;
    public bool hasPistol = false;
    public bool hasRifle = false;

    //Camara
    public Transform cameraAxis;
    public Transform cameraTrack;
    public Transform cameraWeaponTrack;
    private Transform theCamera;
    private float rotY = 0f;
    private float rotX = 0f;
    public float camRotSpeed = 200f;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float cameraSpeed = 200f;

    //Items
    public GameObject nearItem;
    public GameObject[] itemPrefab;
    public Transform itemSlot;
    public GameObject crosshair;
    
    //Armas
    public int weapons;
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    public GameObject crosshair;

    void Start()
    {
        playerTr = this.transform;
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        theCamera = Camera.main.transform;
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MoveLogic();
        CameraLogic();
        ItemLogic();
        AnimLogic();
        if (hasPistol || hasRifle)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) SwitchWeapon();
        }
    }

    public void MoveLogic()
    {
        Vector3 direction = playerRb.velocity;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float theTime = Time.deltaTime;
        newDirection = new Vector2(moveX, moveZ);
        Vector3 side = playerSpeed * moveX * theTime * playerTr.right;
        Vector3 forward = playerSpeed * moveZ * theTime * playerTr.forward;
        Vector3 endDirection = side + forward;
        playerRb.velocity = endDirection;
    }
    
    public void CameraLogic()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float theTime = Time.deltaTime;
        rotY += mouseY * theTime * camRotSpeed;
        rotX = mouseX * theTime * camRotSpeed;
        playerTr.Rotate(0, rotX, 0); //Para que rote con la camara
        rotY = Mathf.Clamp(rotY, minAngle, maxAngle);
        Quaternion localRotation = Quaternion.Euler(-rotY, 0, 0);
        cameraAxis.localRotation = localRotation;
        if (hasPistol || hasRifle)
        {
            cameraTrack.gameObject.SetActive(false);
            cameraWeaponTrack.gameObject.SetActive(true);
            crosshair.gameObject.SetActive(true);
            theCamera.position = Vector3.Lerp(theCamera.position, cameraWeaponTrack.position, cameraSpeed * theTime);
            theCamera.rotation = Quaternion.Lerp(theCamera.rotation, cameraWeaponTrack.rotation, cameraSpeed * theTime);
        }
        else
        {
            cameraTrack.gameObject.SetActive(true);
            cameraWeaponTrack.gameObject.SetActive(false);
            theCamera.position = Vector3.Lerp(theCamera.position, cameraTrack.position, cameraSpeed * theTime);
            theCamera.rotation = Quaternion.Lerp(theCamera.rotation, cameraTrack.rotation, cameraSpeed * theTime);
        }
    }
    public void AnimLogic()
    {
        playerAnim.SetFloat("X", newDirection.x);
        playerAnim.SetFloat("Y", newDirection.y);
        playerAnim.SetBool("holdPistol", hasPistol);
        playerAnim.SetBool("holdRiffle", hasRiffle);
        if (hasPistol || hasRifle) playerAnim.SetLayerWeight(1, 1);
    }
    public void ItemLogic()
    {
        if (nearItem != null && Inout.GetKeyDown(KeyCode.E))
        {
            GameObject instantiatedItem = null;
            bool haveWeapon = false;
            int countWeapons = 0;

            foreach (GameObject itemPrefab in itemPrefabs)
            {
                if (itemPrefab.CompareTag("PW") && nearItem.CompareTag("PW"))
                {
                    instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
                    primaryWeapon = this.gameObject;
                    haveWeapon = true;
                    countWeapons++;
                    weapons++;
                    Destroy(nearItem.gameObject);
                    instantiatedItem.transform.parent = itemSlot;
                    nearItem = null;
                    break
                }
                else if (itemPrefab.CompareTag("SW") && nearItem.CompareTag("SW"))
                {
                    instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
                    secondaryWeapon = this.gameObject;
                    haveWeapon = true;
                    countWeapons++;
                    weapons++;
                    Destroy(nearItem.gameObject);
                    instantiatedItem.transform.parent = itemSlot;
                    nearItem = null;
                    break
                }
            }
            if (haveWeapon && hasPistol && countWeapons > 1) hasPistol = false;
            else if (haveWeapon && hasRiffle && countWeapons > 1) hasRiffle = false;
            if (instantiatedItem.CompareTag("PW"))
            {
                primaryWeapon = instantiatedItem;
                hasPistol = true;
                hasRiffle = false;
                primaryWeapon.SetActive(true);
                secondaryWeapon.SetActive(false);
            }
            else if (instantiatedItem.CompareTag("SW"))
            {
                secondaryWeapon = instantiatedItem;
                hasRiffle = true;
                hasPistol = false;
                primaryWeapon.SetActive(false);
                secondaryWeapon.SetActive(true);
            }

        }
    }
    public void SwitchWeapon()
    {
        if (primaryWeapon.activeSelf == true)
        {
            hasPistol = false;
            hasRiffle = true;
            primaryWeapon.gameObject.SetActive(false);
            secondaryWeapon.gameObject.SetActive(true);
        }
        else if (secondaryWeapon.activeSelf == true)
        {
            hasRiffle = false;
            hasPistol = true;
            secondaryWeapon.gameObject.SetActive(false);
            primaryWeapon.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("Hay un item cerca");
            nearItem = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("Ya no hay item cerca...");
            nearItem = null;
        }
    }
}
