using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            player.HasKeyCard = true;
            Destroy(gameObject);
        }
    }
}