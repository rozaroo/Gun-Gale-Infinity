using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LineOfSight : MonoBehaviour, ILineOfSight
{
    public float range;
    [Range(1, 360)]
    public float angle;
    public LayerMask maskObs;
    Vector3 posplayer;
    public bool CheckRange(Transform target)
    {
        float distance = Vector3.Distance(target.position, Origin);
        UnityEngine.Debug.Log($"Distance to target: {distance}, Range: {range}");
        return distance <= range;
    }
    public bool CheckAngle(Transform target)
    {
        Vector3 dirToTarget = target.position - Origin;
        float angleToTarget = Vector3.Angle(Forward, dirToTarget);
        UnityEngine.Debug.Log($"Angle to target: {angleToTarget}, Angle: {angle}");
        return angleToTarget <= angle / 2;
    }
    public bool CheckView(Transform target)
    {
        posplayer = target.position;
        bool lineOfSight = !Physics.Linecast(Origin + new Vector3(0, 1, 0), target.position + new Vector3(0, 1, 0), maskObs);
        UnityEngine.Debug.Log($"Line of sight: {lineOfSight}");
        return lineOfSight;
    }
    Vector3 Origin => transform.position;
    Vector3 Forward => transform.forward;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Origin, range);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, angle / 2, 0) * Forward * range);
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, -(angle / 2), 0) * Forward * range);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Origin + new Vector3(0, 1, 0), posplayer + new Vector3(0, 1, 0));
    }
}
