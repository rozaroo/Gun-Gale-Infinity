using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public GameObject door;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Open();
            Destroy(gameObject);
        }
    }

    private void Open()
    {
        door.transform.position = new Vector3(door.transform.position.x, 10, door.transform.position.z);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Open();
        }
    }
}
