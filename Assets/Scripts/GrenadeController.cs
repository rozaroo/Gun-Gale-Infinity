using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    public bool throwing = false;
    public float throwDelayTime = 0f;
    public float time = 0f;
    public GameObject theGranade;

    //Externos 
    PlayerController player;


    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Crear una funcion que marque la trayectoria que va a seguir la granada
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            throwing = true;
        }
        if (throwing == true) Throw();
    }
    public void Throw()
    {
        time += time.deltaTime;
        player.playerAnim.Play("Final Grenade");
        if (time >= throwDelayTime)
        {
            Instantiate(theGranade, player.spawnGrenade.position, player.spawnGrenade.rotation);
            throwing = false;
            Destroy(this.gameObject);
            player.weapons--;
            player.hasGrenade = false;
            player.playerAnim.SetLayerWeight(1);
            player.playerAnim.SetLayerWeight(2, 0);
        }
    }
}
