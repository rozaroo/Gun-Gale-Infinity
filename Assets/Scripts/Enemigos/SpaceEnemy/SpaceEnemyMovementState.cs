using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnemyMovementState<T> : State<T>
{
    SpaceEnemyController _spaceEnemyController;
    private Transform[] puntosDeMovimiento;
    private int currentPointIndex = 0;
    private float timeBetweenShots = 2f;
    private Coroutine shootingCoroutine;

    public SpaceEnemyMovementState(SpaceEnemyController spacenemycontroller)
    {
        _spaceEnemyController = spacenemycontroller;
        puntosDeMovimiento = _spaceEnemyController.PuntosdeMovimiento;
    }
    public override void Execute()
    {
        MoveBetweenPoints();
        if (shootingCoroutine == null) shootingCoroutine = _spaceEnemyController.StartCoroutine(ShootAtIntervals());
    }
    private void MoveBetweenPoints()
    {
        if (puntosDeMovimiento.Length == 0) return;
        Transform targetPoint = puntosDeMovimiento[currentPointIndex];
        Vector3 direction = (targetPoint.position - _spaceEnemyController.transform.position).normalized;
        _spaceEnemyController.Move(direction);
        if (Vector3.Distance(_spaceEnemyController.transform.position, targetPoint.position) < 0.1f) currentPointIndex = (currentPointIndex + 1) % puntosDeMovimiento.Length;
    }
    private IEnumerator ShootAtIntervals()
    {
        while (true)
        {
            _spaceEnemyController.Shoot();
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
    
}
