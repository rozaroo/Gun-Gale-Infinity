using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    float timer;
    Transform player;
    Enemy enemy;
    float chaseRange = 8;
    ILineOfSight _los;
    LineOfSight lineOfSight;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.GetComponent<Enemy>();
        lineOfSight = enemy.GetComponent<LineOfSight>();
        _los = enemy.LOS;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > 5) animator.SetBool("isPatrolling", true);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        
        if (_los.CheckRange(player) && _los.CheckAngle(player) && _los.CheckView(player)) 
        {
            if (distance < chaseRange) animator.SetBool("isChasing", true);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
