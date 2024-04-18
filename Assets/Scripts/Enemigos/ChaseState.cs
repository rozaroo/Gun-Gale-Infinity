using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChaseState : StateMachineBehaviour
{
    Transform player;
    ILineOfSight _los;
    LineOfSight lineOfSight;
    Enemy enemy;
    float moveSpeed = 3.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.GetComponent<Enemy>();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ILineOfSight _los = enemy.LOS;
        Vector3 direction = (player.position - animator.transform.position).normalized;
        direction.y = 0;
        //movimiento hacia el jugador
        animator.transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        //Rotación hacia el jugador
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetRotation, 0.15f);
        float distance = Vector3.Distance(player.position, animator.transform.position);

        //if (distance > 15) animator.SetBool("isChasing", false);
        if (!_los.CheckRange(player) && !_los.CheckAngle(player) && !_los.CheckView(player)) animator.SetBool("isChasing", false);
        if (distance < 14f) animator.SetBool("isAttacking", true);
        //if (_los.CheckRange(player) && _los.CheckAngle(player) && _los.CheckView(player)) animator.SetBool("isAttacking", true);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
