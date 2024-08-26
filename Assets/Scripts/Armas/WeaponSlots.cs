using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlots : MonoBehaviour
{
    public Transform primarySlot;
    public Transform secondarySlot;
    public Transform throwableSlot;
    private Transform lastActivateSlot;

    //Externals
    PlayerController player;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    
    void Update()
    {
        if (player.weapons < 1) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (player.primaryWeapon == null) return;
            ToggleSlot(primarySlot);
            player.hasPistol = true;
            player.hasRiffle = false;
            player.hasGrenade = false;
            player.primaryWeaponIcon.color = Color.red;
            player.secondaryWeaponIcon.color = Color.white;
            player.throwableWeaponIcon.color = Color.white;
            player.actualWeaponActive = player.primaryWeapon;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (player.secondaryWeapon == null) return;
            ToggleSlot(secondarySlot);
            player.hasRiffle = true;
            player.hasPistol = false;
            player.hasGrenade = false;
            player.primaryWeaponIcon.color = Color.white;
            player.secondaryWeaponIcon.color = Color.red;
            player.throwableWeaponIcon.color = Color.white;
            player.actualWeaponActive = player.secondaryWeapon;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (player.throwableWeapon == null) return;
            ToggleSlot(throwableSlot);
            player.hasGrenade = true;
            player.hasPistol = false;
            player.hasRiffle = false;
            player.primaryWeaponIcon.color = Color.white;
            player.secondaryWeaponIcon.color = Color.white;
            player.throwableWeaponIcon.color = Color.red;
            player.actualWeaponActive = player.throwableWeapon;
        }
    }
    public void ToggleSlot(Transform slot)
    {
        if (slot == lastActivateSlot) return;
        DeactivateAllSlots();
        bool isActivate = slot.gameObject.activeSelf;
        slot.gameObject.SetActive(!isActivate);
        lastActivateSlot = isActivate ? null : slot;
    }
    public void DeactivateAllSlots() 
    {
        primarySlot.gameObject.SetActive(false);
        secondarySlot.gameObject.SetActive(false);
        throwableSlot.gameObject.SetActive(false);
    }

}
