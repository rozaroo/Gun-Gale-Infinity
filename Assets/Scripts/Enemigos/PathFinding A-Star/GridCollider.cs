using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCollider : MonoBehaviour
{
    private void Awake()
    {
        FindObjectOfType<MyGrid>().AddCollider(GetComponent<Collider>());
    }
}
