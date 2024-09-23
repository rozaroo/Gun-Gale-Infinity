using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextTriggerController : MonoBehaviour
{
  [SerializeField] private GameObject NoCardText;
  private PlayerController playercontroller;

  void Start()
  {
    playercontroller = FindObjectOfType<PlayerController>();
  }

  void OnTriggerStay(Collider other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      if (!playercontroller.HasKeyCard)
      {
        NoCardText.SetActive(true);
      }
    }
  }

  void OnTriggerExit(Collider other)
  {
    NoCardText.SetActive(false);
  }

}
