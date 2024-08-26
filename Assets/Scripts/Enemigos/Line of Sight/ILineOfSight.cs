using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILineOfSight
{
    bool CheckRange(Transform target);
    bool CheckAngle(Transform target);
    bool CheckView(Transform target);
}
