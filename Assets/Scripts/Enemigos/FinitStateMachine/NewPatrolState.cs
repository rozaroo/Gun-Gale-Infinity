using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPatrolState<T> : State<T>
{
    EnemyController _enemyController;
    List<Transform> _wayPoints;
    int _currentWaypointIndex;
    int _patrolDirection;
    ObstacleAvoidance _obs;

    public NewPatrolState(EnemyController enemyController, ObstacleAvoidance obs)
    {
        _enemyController = enemyController;
        _wayPoints = new List<Transform>(enemyController.PuntosdePatrullaje);
        _currentWaypointIndex = UnityEngine.Random.Range(0, _wayPoints.Count);
        _patrolDirection = 1;
        _obs = obs;
    }
    public override void Enter()
    {
        _enemyController.animator.SetBool("isPatrolling", true);
    }
    public override void Sleep()
    {
        base.Sleep();
        _enemyController.animator.SetBool("isPatrolling", false);
    }
    public override void Execute()
    {
        if (_wayPoints.Count == 0) return;
        if (_currentWaypointIndex < 0 || _currentWaypointIndex >= _wayPoints.Count) return;
        Vector3 waypointDirection = _wayPoints[_currentWaypointIndex].position - _enemyController.transform.position;
        waypointDirection.y = 0;
        waypointDirection = _obs.GetDir(waypointDirection, false);
        _enemyController.Move(waypointDirection.normalized);
        _enemyController.LookDir(waypointDirection);

        if (Vector3.Distance(_enemyController.transform.position, _wayPoints[_currentWaypointIndex].position) < 0.5f)
        {
            if (_currentWaypointIndex == _wayPoints.Count - 1 || _currentWaypointIndex == 0) _patrolDirection *= -1;
            _currentWaypointIndex += _patrolDirection;
            _currentWaypointIndex = Mathf.Clamp(_currentWaypointIndex, 0, _wayPoints.Count - 1);
        }
    }
}
