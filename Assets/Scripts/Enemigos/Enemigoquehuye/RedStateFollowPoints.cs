using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStateFollowPoints<T> : State<T>, IPoints
{
    EnemyControllerTwo _enemycontrollertwo;
    public AgentControllerTwo _agentcontrollertwo;
    List<Vector3> _waypoints;
    int _nextPoint = 0;
    bool _isFinishPath = true;
    bool _isReversing = false;
    public bool ejecutar = true;
    public RedStateFollowPoints(EnemyControllerTwo enemycontrollertwo, AgentControllerTwo agentcontrollertwo)
    {
        _enemycontrollertwo = enemycontrollertwo;
        _agentcontrollertwo = agentcontrollertwo;
    }
    public override void Enter()
    {
        _waypoints = _agentcontrollertwo.RunAStarPlusVector();
        //SetWayPoints(list);
        _enemycontrollertwo.animator.SetBool("IsRunning", true);
        SetWayPoints(_waypoints);
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
        if (_isFinishPath || _waypoints == null || _waypoints.Count == 0) return;
        Vector3 point = _waypoints[_nextPoint];
        Vector3 dir = (point - _enemycontrollertwo.transform.position).normalized;
        
        _enemycontrollertwo.Move(dir.normalized);
        _enemycontrollertwo.LookDir(dir);
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
