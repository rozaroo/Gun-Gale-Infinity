using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightModel : MonoBehaviour
{
    public bool _isAlert;
    public bool Alert
    {
        set { _isAlert = value; }
        get { return _isAlert; }
    }
}
