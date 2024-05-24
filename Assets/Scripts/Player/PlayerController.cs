using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public bool Player = true;
    public bool Active = true;
    
    //Personaje
    public Transform playerTr;
    public Rigidbody playerRb;
    internal Animator playerAnim;
    RagdollController playerRagdoll;
    public float maxHealth = 100f;
    public float currentHealth;
    public float playerSpeed = 0f;
    public Vector2 newDirection;
    public bool hasPistol = false;
    public bool hasRiffle = false;
    public bool hasGrenade = false;

    //Actions
    public bool inventoryOpen = false;
    public GameObject[] droppedItems;
    public GameObject actualWeaponActive;
    public GameObject dropThisWeapon;
    public bool dropWeapon = false;

    //UI
    public Canvas playerUI;
    public Image primaryWeaponIcon;
    public Image secondaryWeaponIcon;
    public Image throwableWeaponIcon;
    public GameObject inventoryController;

    //Externos
    WeaponSlots weaponSlots;

    //Camara
    public Transform cameraAxis;
    public Transform cameraTrack;
    public Transform cameraWeaponTrack;
    public Transform theCamera;
    public float rotY = 0f;
    public float rotX = 0f;
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

    //FinitStateMachine
    FSM<StatesEnumuno> _fsm;
    #endregion

    private void Awake()
    {
        InitializeFSM();
    }
    void InitializeFSM()
    {
        _fsm = new FSM<StatesEnumuno>();

        var idle = new IdleState<StatesEnumuno>(this,StatesEnumuno.Idle);
        var walk = new WalkState<StatesEnumuno>(this, StatesEnumuno.Walk);
        //var actions = new ActionsState<StatesEnumuno>(this,StatesEnumuno.Actions);
        //var camera = new CameraState<StatesEnumuno>(this, StatesEnumuno.Camera);

        idle.AddTransition(StatesEnumuno.Walk, walk);
        walk.AddTransition(StatesEnumuno.Idle, idle);

        //idle.AddTransition(StatesEnumuno.Actions, actions);
        //walk.AddTransition(StatesEnumuno.Actions, actions);

        //actions.AddTransition(StatesEnumuno.Idle, idle);
        //actions.AddTransition(StatesEnumuno.Walk, walk);

        //idle.AddTransition(StatesEnumuno.Camera, camera);
        //walk.AddTransition(StatesEnumuno.Camera, camera);

        //camera.AddTransition(StatesEnumuno.Idle, idle);
        //camera.AddTransition(StatesEnumuno.Walk, walk);

        _fsm.SetInit(walk);
    }
    void Start()
    {
        playerTr = this.transform;
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        playerRagdoll = GetComponentInChildren<RagdollController>();
        weaponSlots = GetComponentInChildren<WeaponSlots>();
        theCamera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        currentHealth = maxHealth;
        Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active) return;
        //ActionsLogic();
        ItemLogic();
        _fsm.OnUpdate();
    }

    public void ActionsLogic() 
    {
        //Inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryOpen = !inventoryOpen;
            Cursor.lockState = inventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = inventoryOpen;
        }
        if (inventoryOpen == false) inventoryController.gameObject.SetActive(false);
        else inventoryController.gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.G)) Drop();
    }
    public void Drop()
    {
        if (hasPistol && primaryWeapon != null)
        {
            GameObject droppedPW = primaryWeapon.GetComponent<WeaponController>().itemPrefab;
            Instantiate(droppedPW, new Vector3(playerTr.position.x, droppedPW.transform.position.y, playerTr.position.z),droppedPW.transform.rotation);
            primaryWeaponIcon.gameObject.SetActive(false);
            Destroy(primaryWeapon.gameObject);
            hasPistol = false;
            if (secondaryWeapon == null && throwableWeapon == null) playerAnim.SetLayerWeight(1, 0); //Animación sin armas
            else if (secondaryWeapon != null && throwableWeapon == null)
            {
                playerAnim.SetLayerWeight(1, 1);
                hasRiffle = true;
                weaponSlots.ToggleSlot(secondarySlot);
                primaryWeaponIcon.color = Color.white;
                secondaryWeaponIcon.color = Color.red;
            }
           
        }
        else if (hasRiffle && secondaryWeapon != null)
        {
            GameObject droppedSW = secondaryWeapon.GetComponent<WeaponController>().itemPrefab;
            Instantiate(droppedSW, new Vector3(playerTr.position.x, droppedSW.transform.position.y, playerTr.position.z), droppedSW.transform.rotation);
            secondaryWeaponIcon.gameObject.SetActive(false);
            Destroy(secondaryWeapon.gameObject);
            hasRiffle = false;
            if (primaryWeapon == null && throwableWeapon == null) playerAnim.SetLayerWeight(1, 0);
            else if (primaryWeapon != null && throwableWeapon == null)
            {
                playerAnim.SetLayerWeight(1, 1);
                hasPistol = true;
                weaponSlots.ToggleSlot(primarySlot);
                primaryWeaponIcon.color = Color.red;
                secondaryWeaponIcon.color = Color.white;
            }
        }
    }

    public void ItemLogic()
    {
        if (nearItem != null && Input.GetKeyDown(KeyCode.E))
        {
            GameObject instantiatedItem = null;
            int countWeapons = 0;
            foreach (GameObject itemPrefab in itemPrefab)
            {
                if (itemPrefab.CompareTag("PW") && nearItem.CompareTag("PW"))
                {
                    instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
                    primaryWeapon = instantiatedItem.gameObject;
                    countWeapons++;
                    weapons++;
                    Destroy(nearItem.gameObject);
                    instantiatedItem.transform.parent = primarySlot;
                    nearItem = null;
                    WeaponController pwIcon = instantiatedItem.GetComponentInChildren<WeaponController>();
                    primaryWeaponIcon.sprite = pwIcon.weaponIcon;
                    primaryWeaponIcon.gameObject.SetActive(true);
                    break;
                }
                else if (itemPrefab.CompareTag("SW") && nearItem.CompareTag("SW"))
                {
                    instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
                    secondaryWeapon = instantiatedItem.gameObject;
                    countWeapons++;
                    weapons++;
                    Destroy(nearItem.gameObject);
                    instantiatedItem.transform.parent = secondarySlot;
                    nearItem = null;
                    WeaponController swIcon = instantiatedItem.GetComponentInChildren<WeaponController>();
                    secondaryWeaponIcon.sprite = swIcon.weaponIcon;
                    secondaryWeaponIcon.gameObject.SetActive(true);
                    break;
                }
                else if (itemPrefab.CompareTag("TW") && nearItem.CompareTag("TW"))
                {
                    instantiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
                    throwableWeapon = instantiatedItem.gameObject;
                    countWeapons++;
                    weapons++;
                    Destroy(nearItem.gameObject);
                    instantiatedItem.transform.parent = throwableSlot;
                    nearItem = null;
                    GrenadeController twIcon = instantiatedItem.GetComponentInChildren<GrenadeController>();
                    throwableWeaponIcon.sprite = twIcon.weaponIcon;
                    throwableWeaponIcon.gameObject.SetActive(true);
                    break;
                }
                else if (itemPrefab.CompareTag("Botiquin") && nearItem.CompareTag("Botiquin"))
                {
                    RecoveryHealth(25);
                    Destroy(nearItem.gameObject);
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
            Destroy(this, 1.5f);
        }
    }
    
    public void RecoveryHealth(float health)
    {
        currentHealth += health;
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
