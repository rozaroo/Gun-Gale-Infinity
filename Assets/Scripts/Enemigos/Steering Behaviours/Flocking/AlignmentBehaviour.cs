using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public float multiplier = 1;
    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        Vector3 alignment = Vector3.zero;
        for (int i = 0; i < boids.Count; i++)
        {
            alignment += boids[i].Front;
        }
        return alignment.normalized * multiplier;
    }
}
