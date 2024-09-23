using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanelController : MonoBehaviour, Iinteract
{
    [SerializeField] private GameObject oNLed;
    [SerializeField] private GameObject oFFLed;
    [SerializeField] private GameObject door;
    private PlayerController playercontroller;

    void Start()
    {
        playercontroller = FindObjectOfType<PlayerController>();
    }

    public void Interact()
    {
        if (playercontroller != null)
        {
            if (playercontroller.HasKeyCard)
            {
                HasCard();
            }else 
            {
                NoCard();
            }
        }
        Debug.Log("<color=blue>"+"sisiss"+ "</color>");
    }

    private void HasCard()
    {
        oNLed.gameObject.SetActive(true);
        oFFLed.gameObject.SetActive(false);
        Open();
    }

    private void NoCard()
    {
        oNLed.gameObject.SetActive(false);
        oFFLed.gameObject.SetActive(true);
    }

    private void Open()
    {
        var anim = door.GetComponent<Animator>();
        anim.SetTrigger("Open");
    }

// es para debug y para probar la puerta
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Open();
        }
    }
}
