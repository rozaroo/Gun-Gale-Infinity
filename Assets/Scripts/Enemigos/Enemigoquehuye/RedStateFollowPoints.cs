using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStateFollowPoints<T> : State<T>, IPoints
{
    //Cambiar el c�digo para que en lugar de patrullar vaya de vector3 en vector3 hasta el objeto con c�digo Objetive m�s lejano al jugador
    EnemyControllerTwo _enemycontrollertwo;
    public AgentControllerTwo _agentcontrollertwo;
    List<Vector3> _waypoints;
    int _nextPoint = 0;
    bool _isFinishPath = true;
    public RedStateFollowPoints(EnemyControllerTwo enemycontrollertwo, AgentControllerTwo agentcontrollertwo)
    {
        _enemycontrollertwo = enemycontrollertwo;
        _agentcontrollertwo = agentcontrollertwo;
    }
    public override void Enter()
    {
        var list = _agentcontrollertwo.RunAStarPlusVector();
        SetWayPoints(list);
        _enemycontrollertwo.animator.SetBool("IsRunning", true);
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
        _enemycontrollertwo.animator.SetBool("IsRunning", false);
    }
    
    public void SetWayPoints(List<Node> newPoints)
    {
        var list = new List<Vector3>();
        for (int i = 0; i < newPoints.Count; i++)
        {
            list.Add(newPoints[i].transform.position);
        }
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
            if (_nextPoint + 1 < _waypoints.Count) _nextPoint++;
            else
            {
                _isFinishPath = true;
                return;
            }
        }
        _enemycontrollertwo.Move(dir.normalized);
        _enemycontrollertwo.LookDir(dir);
    }
    public bool IsFinishPath => _isFinishPath;
}
