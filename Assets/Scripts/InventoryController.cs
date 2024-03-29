using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Image primaryBackpackIcon;
    public Image secondaryBackpackIcon;
    public Image throwableBackpackIcon;
    //Externos
    PlayerController player;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInventoryIcons();
    }
    public void UpdateInventoryIcons()
    {
        if (player.primaryWeapon != null) primaryBackpackIcon.sprite = player.primaryWeaponIcon.sprite;
        else primaryBackpackIcon.sprite = null;

        if (player.secondaryWeapon != null) secondaryBackpackIcon.sprite = player.secondaryWeaponIcon.sprite;
        else secondaryBackpackIcon.sprite = null;

        if (player.throwableWeapon != null) throwableBackpackIcon.sprite = player.throwableWeaponIcon.sprite;
        else throwableBackpackIcon.sprite = null;
    }
}
