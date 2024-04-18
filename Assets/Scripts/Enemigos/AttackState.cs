using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    Enemy enemy;
    Transform player;
    ILineOfSight _los;
    float elapsedTime = 0f;
    float attackInterval = 2f;
    LineOfSight lineOfSight;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lineOfSight = enemy.LineOfSight;
        _los = enemy.LOS;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(player);
        //float distance = Vector3.Distance(player.position, animator.transform.position);
        if (!_los.CheckRange(player) && !_los.CheckAngle(player) && !_los.CheckView(player))
        {
            animator.SetBool("isAttacking", false);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= attackInterval)
            {
                enemy.ShootFireball();
                elapsedTime = 0f;
            } 
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
