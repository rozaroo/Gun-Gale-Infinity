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
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (player.secondaryWeapon == null) return;
            ToggleSlot(secondarySlot);
            player.hasRiffle = true;
            player.hasPistol = false;
            player.hasGrenade = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (player.throwableWeapon == null) return;
            ToggleSlot(throwableSlot);
            player.hasGrenade = true;
            player.hasPistol = false;
            player.hasRiffle = false;

        }
    }
    private void ToggleSlot(Transform slot)
    {
        if (slot == lastActivateSlot) return;
        DeactivateAllSlots();
        bool isActivate = slot.gameObject.activateSelf;
        slot.gameObject.SetActivate(!isActivate);
        lastActivateSlot = isActivate ? null : slot;
    }
    public void DeactivateAllSlots() 
    {
        primarySlot.gameObject.SetActivate(false);
        secondarySlot.gameObject.SetActivate(false);
        throwableSlot.gameObject.SetActivate(false);
    }

}
