using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject keyCardIndicator;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            player.HasKeyCard = true;
            keyCardIndicator.SetActive(true);
            Destroy(gameObject);
        }
    }
}