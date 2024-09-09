using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    public bool throwing = false;
    public float throwDelayTime = 0f;
    public float time = 0f;
    public GameObject itemPrefab;
    public GameObject theGranade;
    public Sprite weaponIcon;

    //Externos 
    PlayerController player;
    WeaponSlots slots;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        slots = GetComponentInParent<WeaponSlots>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Crear una funcion que marque la trayectoria que va a seguir la granada
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) throwing = true;
        if (throwing == true) Throw();
    }
    public void Throw()
    {
        time += Time.deltaTime;
        player.playerAnim.Play("Final Grenade");
        
        if (time >= throwDelayTime)
        {
           /* Instantiate(theGranade, player.spawnGrenade.position, player.spawnGrenade.rotation);
            Destroy(this.gameObject);*/
            throwing = false;
            player.throwableWeapon = null;
            player.weapons--;
            player.hasGrenade = false;
            player.playerAnim.SetLayerWeight(1,0);
            player.playerAnim.SetLayerWeight(2, 0);
            player.throwableWeaponIcon.color = Color.white;
            player.throwableWeaponIcon.gameObject.SetActive(false);
        }
        if (player.secondaryWeapon != null && player.throwableWeapon == null)
        {
            player.playerAnim.SetLayerWeight(1, 1);
            player.hasRiffle = true;
            slots.ToggleSlot(player.secondarySlot);
            player.primaryWeaponIcon.color = Color.white;
            player.secondaryWeaponIcon.color = Color.red;
        }
        else if (player.primaryWeapon != null && player.secondaryWeapon == null && player.throwableWeapon == null)
        {
            player.playerAnim.SetLayerWeight(1, 1);
            player.hasPistol = true;
            slots.ToggleSlot(player.primarySlot);
            player.primaryWeaponIcon.color = Color.red;
            player.secondaryWeaponIcon.color = Color.white;
        }
    }
}
