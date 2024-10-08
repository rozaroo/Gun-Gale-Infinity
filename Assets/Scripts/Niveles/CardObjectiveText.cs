using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardObjectiveText : MonoBehaviour
{
    [SerializeField] private GameObject CardObjective;
    [SerializeField] private float AnimationTime;
    private float maxTime;
    private PlayerController playercontroller;
    private Animator animator;

    void Start()
    {
        playercontroller = FindObjectOfType<PlayerController>();
        maxTime = AnimationTime;
        animator = CardObjective.GetComponent<Animator>();
    }


    void Update()
    {
        if (maxTime > 0) maxTime -= Time.deltaTime;
        else if (maxTime < 0) maxTime = 0;

        animator.SetFloat("Time", maxTime);

    }

/*
    // --------------- si quiero hacerlo con trigger  -----------------------------
    void OnTriggerStay(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))  
      {
        animator.Play("CardObjectiveEntry_Animation");
      }
    }

    void OnTriggerExit(Collider other)
    {
        animator.Play("CardObjectiveExit_Animation");
    }*/
}