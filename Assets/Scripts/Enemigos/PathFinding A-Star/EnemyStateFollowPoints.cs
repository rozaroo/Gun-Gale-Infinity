using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFollowPoints<T> : State<T>, IPoints
{
    EnemyController _enemyController;
    List<Vector3> _waypoints;
    int _nextPoint = 0;
    bool _isFinishPath = true;
    Animator _anim;
    public EnemyStateFollowPoints(EnemyController enemyController, Animator anim)
    {
        _enemyController = enemyController;
        _anim = anim;
    }
    public override void Enter()
    {
        base.Enter();
        _anim.SetFloat("Vel", 1);
    }
    public override void Execute()
    {
        base.Execute();
        Run();
    }
    public override void Sleep()
    {
        base.Sleep();
        _anim.SetFloat("Vel", 0);
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
        pos.y = _enemyController.transform.position.y;
        _enemyController.SetPosition(pos);
        _isFinishPath = false;
    }
    void Run()
    {
        if (IsFinishPath) return;
        var point = _waypoints[_nextPoint];
        var posPoint = point;
        posPoint.y = _enemyController.transform.position.y;
        Vector3 dir = posPoint - _enemyController.transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < _waypoints.Count) _nextPoint++;
            else
            {
                _isFinishPath = true;
                return;
            }
        }
        _enemyController.Move(dir.normalized);
        _enemyController.LookDir(dir);
    }
    public bool IsFinishPath => _isFinishPath;
}
