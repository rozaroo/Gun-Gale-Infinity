using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    void Start()
    {
        if (target == null)
        {
            GameObject playerShip = GameObject.FindWithTag("Nave");
            if (playerShip != null) target = playerShip.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
