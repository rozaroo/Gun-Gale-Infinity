using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool Player = true;
    public bool Active = true;
    
    //Personaje
    Transform playerTr;
    Rigidbody playerRb;
    internal Animator playerAnim;
    RagdollController playerRagdoll;
    public float maxHealth = 100f;
    public float currentHealth;
    public float playerSpeed = 0f;
    private Vector2 newDirection;
    public bool hasPistol = false;
    public bool hasRifle = false;
    public bool hasGrenade = false;

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
    public GameObject throwableWeapon;
    public Transform primarySlot;
    public Transform secondarySlot;
    public Transform throwableSlot;
    public Transform spawnGrenade;

    void Start()
    {
        playerTr = this.transform;
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        playerRagdoll = GetComponentInChildren<RagdollController>();
        theCamera = Camera.main.transform;
        
        Cursor.lockState = CursorLockMode.Locked;
        currentHealth = maxHealth;
        Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            MoveLogic();
            CameraLogic();
        }
        if (!Active) return;
        ItemLogic();
        AnimLogic();
        
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
        if (hasPistol || hasRifle || hasGrenade)
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
        playerAnim.SetBool("holdGrenade", hasGrenade);
        if (hasPistol || hasRifle) 
        {
            playerAnim.SetLayerWeight(2, 0);
            playerAnim.SetLayerWeight(1, 1); 
        }
        else if (hasGrenade)
        {
            playerAnim.SetLayerWeight(1, 0);
            playerAnim.SetLayerWeight(2, 1);
        }
    }
    public void ItemLogic()
    {
        if (nearItem != null && Input.GetKeyDown(KeyCode.E))
        {
            GameObject instantiatedItem = null;
            
            int countWeapons = 0;

            foreach (GameObject itemPrefab in itemPrefabs)
            {
                if (itemPrefab.CompareTag("PW") && nearItem.CompareTag("PW"))
                {
                    instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
                    primaryWeapon = this.gameObject;
                    
                    countWeapons++;
                    weapons++;
                    Destroy(nearItem.gameObject);
                    instantiatedItem.transform.parent = primarySlot;
                    nearItem = null;
                    break
                }
                else if (itemPrefab.CompareTag("SW") && nearItem.CompareTag("SW"))
                {
                    instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
                    secondaryWeapon = this.gameObject;
                    
                    countWeapons++;
                    weapons++;
                    Destroy(nearItem.gameObject);
                    instantiatedItem.transform.parent = secondarySlot;
                    nearItem = null;
                    break
                }
                else if (itemPrefab.CompareTag("TW") && nearItem.CompareTag("TW"))
                {
                    instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
                    throwableWeapon = this.gameObject;
                    
                    countWeapons++;
                    weapons++;
                    Destroy(nearItem.gameObject);
                    instantiatedItem.transform.parent = throwableSlot;
                    nearItem = null;
                    break
                }
            }
        }
    }
    

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Debug.Log("Estas Muerto");
            playerRagdoll.Active(true);
            Active = false;
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
