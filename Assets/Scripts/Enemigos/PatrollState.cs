using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PatrollState : StateMachineBehaviour
{
    float timer;
    List<Transform> wayPoints = new List<Transform>();
    Transform player;
    float chaseRange = 8;
    int currentWaypointIndex = 0;
    int patrolDirection = 1; // 1 = adelante y -1 = atras
    Enemy enemyScript;
    ILineOfSight _los;
    LineOfSight lineOfSight;
    


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyScript = animator.GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        wayPoints.AddRange(Array.ConvertAll(enemyScript.PuntosdePatrullaje, item => item.transform));
        currentWaypointIndex = UnityEngine.Random.Range(0, wayPoints.Count);
        lineOfSight = enemyScript.GetComponent<LineOfSight>();
        timer = 0;
        _los = enemyScript.LOS;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wayPoints.Count == 0) return;
        if (currentWaypointIndex < 0 || currentWaypointIndex >= wayPoints.Count) return; 
        if (wayPoints.Count > 0 && currentWaypointIndex >= 0 && currentWaypointIndex < wayPoints.Count) 
        {
            Vector3 direction = wayPoints[currentWaypointIndex].position - animator.transform.position;
            direction.y = 0;
            //Movimiento hacia el actual punto de patrullaje
            animator.transform.Translate(direction.normalized * Time.deltaTime * 1.5f, Space.World);
            //Rotación hacia el siguiente punto de patrulla
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetRotation, 0.15f);

            if (Vector3.Distance(animator.transform.position, wayPoints[currentWaypointIndex].position) < 0.5f)
            {
                //cambiar de dirección
                if (currentWaypointIndex == wayPoints.Count - 1 || currentWaypointIndex == 0)
                {
                    patrolDirection *= -1;
                    //Pasar al siguiente punto
                    currentWaypointIndex += patrolDirection;
                    //Restringir el indice al rango valido
                    currentWaypointIndex = Mathf.Clamp(currentWaypointIndex, 0, wayPoints.Count - 1);
                }
            }
            timer += Time.deltaTime;
            if (timer > 10) animator.SetBool("isPatrolling", false);
            float distance = Vector3.Distance(player.position, animator.transform.position);
            
            if (_los.CheckRange(player) && _los.CheckAngle(player) && _los.CheckView(player))
            {
                if (distance < chaseRange) animator.SetBool("isChasing", true);
            }
        }
        
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
