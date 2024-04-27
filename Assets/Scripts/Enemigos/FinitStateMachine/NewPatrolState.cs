using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPatrolState<T> : State<T>
{
    public Enemy _enemy;
    List<Transform> _wayPoints;
    int _currentWaypointIndex;
    int _patrolDirection;

    public NewPatrolState(Enemy enemy)
    {
        _enemy = enemy;
        _wayPoints = new List<Transform>(enemy.PuntosdePatrullaje);
        _currentWaypointIndex = UnityEngine.Random.Range(0, _wayPoints.Count);
        _patrolDirection = 1;
    }
    public override void Enter()
    {
        _enemy.animator.SetBool("isPatrolling", true);
    }
    public override void Sleep()
    {
        base.Sleep();
        _enemy.animator.SetBool("isPatrolling", false);
    }
    public override void Execute()
    {
        if (_wayPoints.Count == 0) return;
        if (_currentWaypointIndex < 0 || _currentWaypointIndex >= _wayPoints.Count) return;
        Vector3 waypointDirection = _wayPoints[_currentWaypointIndex].position - _enemy.transform.position;
        waypointDirection.y = 0;
        _enemy.transform.Translate(waypointDirection.normalized * Time.deltaTime * _enemy.speed, Space.World);
        if (Vector3.Distance(_enemy.transform.position, _wayPoints[_currentWaypointIndex].position) < 0.5f)
        {
            if (_currentWaypointIndex == _wayPoints.Count - 1 || _currentWaypointIndex == 0) _patrolDirection *= -1;
            _currentWaypointIndex += _patrolDirection;
            _currentWaypointIndex = Mathf.Clamp(_currentWaypointIndex, 0, _wayPoints.Count - 1);
        }
    }
}
