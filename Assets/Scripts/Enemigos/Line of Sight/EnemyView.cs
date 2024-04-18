using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    IAlert _alert;
    private void Awake()
    {
        _alert = GetComponent<IAlert>();
    }
    
}
