using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public float multiplier;
    public Transform target;
    public bool isActive = true;

    private void Update()
    {
        if (target == null) ReassignTarget();
    }

    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        if (isActive) return (target.position - self.Position).normalized * multiplier;
        return Vector3.zero;
    }
    public void ReassignTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
