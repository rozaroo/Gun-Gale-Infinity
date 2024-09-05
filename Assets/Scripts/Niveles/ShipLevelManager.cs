using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLevelManager : MonoBehaviour
{
    public int Enemies;
    public GameObject Oleada1;
    public GameObject Oleada2;
    public GameObject Oleada3;
    void Start()
    {
        Oleada1.SetActive(true);
        Oleada2.SetActive(false);
        Oleada3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemies == 8) Oleada2.SetActive(true);
        if (Enemies == 4) Oleada3.SetActive(false);
    }
}
