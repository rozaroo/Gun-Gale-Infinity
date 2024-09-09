using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipLevelManager : MonoBehaviour
{
    public int Enemies;
    public GameObject Oleada1;
    public GameObject Oleada2;
    public GameObject Oleada3;
    [SerializeField] private TextMeshProUGUI oleadaTMP;
    void Start()
    {
        Oleada1.SetActive(true);
        oleadaTMP.text = "Oleada: 1";
        Oleada2.SetActive(false);
        Oleada3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemies == 8) ActivarOleada2();
        if (Enemies == 4) ActivarOleada3();
    }
    public void ActivarOleada2()
    {
        Oleada2.SetActive(true);
        oleadaTMP.text = "Oleada: 2";
    }
    public void ActivarOleada3()
    {
        Oleada3.SetActive(true);
        oleadaTMP.text = "Oleada: 3";
    }
}
