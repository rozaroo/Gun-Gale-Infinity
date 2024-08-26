using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStateFollowPoints<T> : State<T>, IPoints
{
    SlimeController _slimecontroller;
    public AgentController _agentcontroller;
    List<Vector3> _waypoints;
    int _nextPoint = 0;
    bool _isFinishPath = true;
    bool _isReversing = false;
    public bool ejecutar = true;
    public SlimeStateFollowPoints(SlimeController slimecontroller, AgentController agentcontroller)
    {
        _slimecontroller = slimecontroller;
        _agentcontroller = agentcontroller;
    }
    public override void Enter()
    {
        var list = _agentcontroller.RunAStar();
        SetWayPoints(list);
        base.Enter();
    }
    public override void Execute()
    {
        base.Execute();
        //Añadir una comprobación de sí el jugador se movio de su lugar para hacer el runasatar de vuelta
        Run();
    }
    public override void Sleep()
    {
        base.Sleep();
    }
    public void SetWayPoints(List<Node> newPoints)
    {
        var list = new List<Vector3>();
        for (int i = 0; i < newPoints.Count; i++)
            list.Add(newPoints[i].transform.position);
        SetWayPoints(list);
    }
    public void SetWayPoints(List<Vector3> newPoints)
    {
        _nextPoint = 0;
        if (newPoints.Count == 0) return;
        //_anim.Play("CIA_Idle");
        _waypoints = newPoints;
        var pos = _waypoints[_nextPoint];
        pos.y = _slimecontroller.transform.position.y;
        _slimecontroller.SetPosition(pos);
        _isFinishPath = false;
        _isReversing = false;
    }
    void Run()
    {
        if (IsFinishPath) return;
        var point = _waypoints[_nextPoint];
        var posPoint = point;
        posPoint.y = _slimecontroller.transform.position.y;
        Vector3 dir = posPoint - _slimecontroller.transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (!_isReversing)
            {
                if (_nextPoint + 1 < _waypoints.Count) _nextPoint++;
                else
                {
                    _isFinishPath = true;
                    InvertWaypoints();
                }
            }
            else
            {
                if (_nextPoint - 1 >= 0) _nextPoint--;
                else
                {
                    _isFinishPath = true;
                    InvertWaypoints();
                }
            }
        }
        _slimecontroller.Move(dir.normalized);
        _slimecontroller.LookDir(dir);
    }
    void InvertWaypoints()
    {
        _waypoints.Reverse();
        _nextPoint = _isReversing ? 0 : _waypoints.Count - 1; //Reiniciar el 1er punto en la nueva dirección
        _isReversing = !_isReversing;
        _isFinishPath = false;
    }
    public bool IsFinishPath => _isFinishPath;
}
