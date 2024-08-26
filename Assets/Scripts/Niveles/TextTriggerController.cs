using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextTriggerController : MonoBehaviour
{
    [SerializeField] private GameObject text;

  void OnTriggerStay(Collider other)
  {
        if (other.gameObject.CompareTag("Player"))
        {
            text.SetActive(true);
        }
  }
  void OnTriggerExit(Collider other)
  {
    if (other.gameObject.CompareTag("Player"))
        {
            text.SetActive(false);
        }
  }
}
