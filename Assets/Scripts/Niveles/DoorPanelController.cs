using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanelController : MonoBehaviour, Iinteract
{
    [SerializeField] private GameObject keyCardIndicator;
    [SerializeField] private GameObject oNLed;
    [SerializeField] private GameObject oFFLed;
    [SerializeField] private GameObject door;
    [SerializeField] private AudioSource KeyCardSound;
    [SerializeField] private AudioSource NoKeyCardSound;
    [SerializeField] private float AnimationTime;
    private float maxTime;
    private PlayerController playercontroller;
    private Animator animator;

    void Start()
    {
        playercontroller = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
        maxTime = AnimationTime;
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
        Debug.Log("<color=blue>"+"Interactuando Panel"+ "</color>");
    }

    private void HasCard()
    {
       /* oNLed.gameObject.SetActive(true);
        oFFLed.gameObject.SetActive(false);*/
        animator.Play("DoorPanelOn_Animation");
        KeyCardSound.Play();
        keyCardIndicator.SetActive(false);
        Open();
    }

    private void NoCard()
    {
       /* oNLed.gameObject.SetActive(false);
        oFFLed.gameObject.SetActive(true);*/
        animator.Play("DoorPanelOFF_Animation");
        NoKeyCardSound.Play();
    }

    private void Open()
    {
        var anim = door.GetComponent<Animator>();
        anim.SetTrigger("Open");
    }

// es para debug y para probar la puerta
    void Update()
    {
        if (maxTime > 0) maxTime -= Time.deltaTime;
        else if (maxTime < 0) maxTime = 0;

        animator.SetFloat("Time", maxTime);

        if (Input.GetKeyDown(KeyCode.L))
        {
            Open();
        }
    }
}
