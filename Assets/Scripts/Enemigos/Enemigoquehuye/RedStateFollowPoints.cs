using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStateFollowPoints<T> : State<T>, IPoints
{
    EnemyControllerTwo _enemycontrollertwo;
    public AgentControllerTwo _agentcontrollertwo;
    ObstacleAvoidance _obstacleAvoidance;
    List<Vector3> _waypoints;
    int _nextPoint = 0;
    bool _isFinishPath = true;
    bool _isReversing = false;
    public bool ejecutar = true;
    public RedStateFollowPoints(EnemyControllerTwo enemycontrollertwo, AgentControllerTwo agentcontrollertwo)
    {
        _enemycontrollertwo = enemycontrollertwo;
        _agentcontrollertwo = agentcontrollertwo;
        _obstacleAvoidance = new ObstacleAvoidance(_enemycontrollertwo.transform, _enemycontrollertwo.angle, _enemycontrollertwo.radius, _enemycontrollertwo.maskObs, 2.5f);
    }
    public override void Enter()
    {
        //var list = _agentcontrollertwo.RunAStar(_enemycontrollertwo);
        //SetWayPoints(list);
        _enemycontrollertwo.animator.SetBool("IsRunning", true);
        base.Enter();
    }
    public override void Execute()
    {
        base.Execute();
        //A�adir una comprobaci�n de s� el jugador se movio de su lugar para hacer el runasatar de vuelta
        Run();
    }
    public override void Sleep()
    {
        base.Sleep();
        _enemycontrollertwo.animator.SetBool("IsRunning", false);
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
        pos.y = _enemycontrollertwo.transform.position.y;
        _enemycontrollertwo.SetPosition(pos);
        _isFinishPath = false;
        _isReversing = false;
    }
    void Run()
    {
        if (IsFinishPath) return;
        var point = _waypoints[_nextPoint];
        var posPoint = point;
        posPoint.y = _enemycontrollertwo.transform.position.y;
        Vector3 dir = posPoint - _enemycontrollertwo.transform.position;
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
        var avoidanceDir = _obstacleAvoidance.GetDir(dir.normalized);
        if (avoidanceDir != dir.normalized)
        {
            // Si hay un obstáculo, buscar un punto seguro
            _obstacleAvoidance.MoveToSafePoint(_agentcontrollertwo, _enemycontrollertwo);
            return;
        }
        _enemycontrollertwo.Move(dir.normalized);
        _enemycontrollertwo.LookDir(dir);
    }
    void InvertWaypoints()
    {
        _waypoints.Reverse();
        _nextPoint = _isReversing ? 0 : _waypoints.Count - 1; //Reiniciar el 1er punto en la nueva direcci�n
        _isReversing = !_isReversing;
        _isFinishPath = false;
    }
    public bool IsFinishPath => _isFinishPath;
}
