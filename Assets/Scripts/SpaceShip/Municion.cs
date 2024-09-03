using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Municion : MonoBehaviour
{
    SpaceShipController Player;
    
    void Start()
    {
        Player = FindObjectOfType<SpaceShipController>();
    }

    void Update()
    {
        
    }
}
