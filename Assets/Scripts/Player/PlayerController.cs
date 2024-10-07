using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    #region Variables
    public bool Player = true;
    public bool Active = true;
    public bool HasKeyCard = false;
    [SerializeField] private Transform interactPoint;
    [SerializeField] private LayerMask interactMask;
    public PlayerValues playervalues;

    [Header("Personaje")]
    //Personaje
    public Transform playerTr;
    public Rigidbody playerRb;
    internal Animator playerAnim;
    RagdollController playerRagdoll;
    public float currentHealth;
    public float currentshield;
    public Vector2 newDirection;
    public bool hasPistol = false;
    public bool hasRiffle = false;
    public bool hasGrenade = false;

    [Header("Acciones")]
    //Actions
    public bool inventoryOpen = false;
    public GameObject[] droppedItems;
    public GameObject actualWeaponActive;
    public GameObject dropThisWeapon;
    public bool dropWeapon = false;

    [Header("UI")]
#region UI
    public Canvas playerUI;
    public Image primaryWeaponIcon;
    public Image secondaryWeaponIcon;
    public Image throwableWeaponIcon;
    public GameObject inventoryController;
#endregion

    //Externos
    WeaponSlots weaponSlots;

    [Header("Camara")]
    //Camara
    public Transform cameraAxis;
    public Transform cameraTrack;
    public Transform cameraWeaponTrack;
    public Transform theCamera;
    public float rotY = 0f;
    public float rotX = 0f;

    [Header("Items")]
    //Items
    public GameObject nearItem;
    public Transform itemSlot;
    public GameObject crosshair;
    
    [Header("Armas")]
    //Armas
    public int weapons;
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    public GameObject throwableWeapon;
    public Transform primarySlot;
    public Transform secondarySlot;
    public Transform throwableSlot;
    public Transform spawnGrenade;

    [Header("Post-Process")]
    //Post-Process
    public CustomPostProcessRed postProcessController;
    public GrayScalePP grayscale;

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

        var idle = new IdleState<StatesEnumuno>(this,StatesEnumuno.Walk, StatesEnumuno.Die);
        var walk = new WalkState<StatesEnumuno>(this, StatesEnumuno.Idle, StatesEnumuno.Die);
        var die = new DieState<StatesEnumuno>(this);

        idle.AddTransition(StatesEnumuno.Walk, walk);
        idle.AddTransition(StatesEnumuno.Die, die);
        walk.AddTransition(StatesEnumuno.Idle, idle);
        walk.AddTransition(StatesEnumuno.Die, die);
        die.AddTransition(StatesEnumuno.Walk, walk);
        die.AddTransition(StatesEnumuno.Die, die);
        _fsm.SetInit(idle);
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
        currentHealth = playervalues.MaxHealth[0];
        Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.OnUpdate();
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
            if (secondaryWeapon == null && throwableWeapon == null) playerAnim.SetLayerWeight(1, 0); //Animaci�n sin armas
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

    public void ONInteract()
    {
       Collider[] colliders = Physics.OverlapBox(interactPoint.position,new Vector3(1f, 1f, 1f), quaternion.identity, interactMask);

        foreach (Collider items in colliders)
        {
            if (items.TryGetComponent(out Iinteract interactable))
            {
                interactable.Interact();
                Debug.Log("<color=green>" + interactable + "</color>");
            }
        } 
    }
    public void TakeDamage(float damage)
    {
        if (currentshield > 0) currentshield -= damage;
        if (currentshield <= 0) currentHealth -= damage;
    }
    public void Die()
    {
        playerRagdoll.Active(true);
        Active = false;
        Destroy(this, 1.5f);
    }

    
    public void RecoveryHealth(float health)
    {
        if (currentHealth < 100) currentHealth += health;
    }
    public void RecoverShield(float Value)
    {
        if (currentshield < 100) currentshield += Value;
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item")) nearItem = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item")) nearItem = null;
    }
}
