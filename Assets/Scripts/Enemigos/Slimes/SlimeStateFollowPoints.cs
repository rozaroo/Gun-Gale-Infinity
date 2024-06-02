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
    public SlimeStateFollowPoints(SlimeController slimecontroller, AgentController agentcontroller)
    {
        _slimecontroller = slimecontroller;
        _agentcontroller = agentcontroller;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Execute()
    {
        base.Execute();
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
            if (_nextPoint + 1 < _waypoints.Count) _nextPoint++;
            else
            {
                _isFinishPath = true;
                return;
            }
        }
        _slimecontroller.Move(dir.normalized);
        _slimecontroller.LookDir(dir);
        _agentcontroller.RunAStar();
    }
    public bool IsFinishPath => _isFinishPath;
}
