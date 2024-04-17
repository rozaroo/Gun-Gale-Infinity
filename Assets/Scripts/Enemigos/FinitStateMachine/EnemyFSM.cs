using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class EnemyFSM : MonoBehaviour
{
    
    FSM<EnemyStateType> _fsm;
    public Animator animator;
    
   public EnemyStateType CurrentState 
    { 
        get 
        {
            return _fsm.CurrentState;
        } }

    void Start()
    {
        EnemyFSM.EnemyStateType estado = EnemyFSM.EnemyStateType.Idle;
        _fsm = new FSM<EnemyStateType>();
        _fsm.SetInit(new IdleState());
        currentState = _fsm.CurrentState;
    }

    void Update()
    {
        _fsm.OnUpdate();
        UpdateAnimatorParameters();
    }
    void LateUpdate()
    {
        _fsm.OnLateUpdate();
    }
    public void TransitionState(EnemyStateType newState)
    {
        _fsm.Transition(newState);
    }
    void UpdateAnimatorParameters()
    {
        EnemyStateType currentState = _fsm.CurrentState;
        EnemyStateType currentStateFromInterface = _currentState.CurrentState;
        if (currentState != null)
        {
            //Parametros del animator
            animator.SetBool("isIdle", currentState = EnemyStateType.Idle);
            animator.SetBool("isPatrolling", currentState = EnemyStateType.Patrol);
            animator.SetBool("isChasing", currentState = EnemyStateType.Chase);
            animator.SetBool("isAttacking", currentState = EnemyStateType.Attack);

            animator.SetBool("isIdle", currentStateFromInterface == EnemyStateType.Idle);
            animator.SetBool("isPatrolling", currentStateFromInterface == EnemyStateType.Patrol);
            animator.SetBool("isChasing", currentStateFromInterface == EnemyStateType.Chase);
            animator.SetBool("isAttacking", currentStateFromInterface == EnemyStateType.Attack);
        }
    }

    public enum EnemyStateType
    {
        Idle,
        Attack,
        Patrol,
        Chase,
    }
    public abstract class EnemyState : State<EnemyStateType>
    {
        protected EnemyFSM _enemyFSM;
        public override void Enter()
        {
            base.Enter();
            _fsm = new FSM<EnemyStateType>(this);
        }
    }
    public class IdleState : EnemyState
    {
        float timer;
        Transform player;
        float chaseRange = 8;
        

        public override void Enter()
        {
            base.Enter();
            timer = 0;
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void Execute()
        {
            base.Execute();
            Debug.Log("Idle State");
            timer += Time.deltaTime;
            if (timer > 5) _enemyFSM.TransitionState(EnemyStateType.Idle);
            float distance = Vector3.Distance(player.position, _enemyFSM.transform.position);
            if (distance < chaseRange) _enemyFSM.TransitionState(EnemyStateType.Chase);
          
        }
    }
    public class AttackState : EnemyState
    {
        Enemy enemy;
        Transform player;
        float elapsedTime = 0f;
        float attackInterval = 1f;

       
        public override void Enter()
        {
            base.Enter();
            enemy = _enemyFSM.GetComponent<Enemy>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void Execute()
        {
            base.Execute();
            Debug.Log("Attack State");
            _enemyFSM.transform.LookAt(player);
            float distance = Vector3.Distance(player.position, _enemyFSM.transform.position);
            //Si la distancia es mayor a 3.5f dejar de atacar
            if (distance > 3.5f)
            {
                _enemyFSM.animator.SetBool("isAttacking", false);
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= attackInterval)
                {
                    enemy.ShootFireball();
                    elapsedTime = 0f;
                }
            }

        }
    }
    public class PatrolState : EnemyState
    {
        
        float timer;
        List<Transform> wayPoints = new List<Transform>();
        Transform player;
        float chaseRange = 8;
        int currentWaypointIndex = 0;
        int patrolDirection = 1; // 1 = adelante y -1 = atras
        Enemy enemyScript;

        public PatrolState(EnemyFSM enemyFSM) : base() { _enemyFSM = enemyFSM; }

        public override void Enter()
        {
            base.Enter();
            Enemy enemyScript = _enemyFSM.GetComponent<Enemy>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            wayPoints.AddRange(Array.ConvertAll(enemyScript.PuntosdePatrullaje, item => item.transform));
            currentWaypointIndex = UnityEngine.Random.Range(0, wayPoints.Count);
            timer = 0;
        }
        public void Execute()
        {
            base.Execute();
            Debug.Log("Patrol State");
            if (wayPoints.Count == 0) return;

            Vector3 direction = wayPoints[currentWaypointIndex].position - _enemyFSM.transform.position;
            direction.y = 0;
            //Movimiento hacia el actual punto de patrullaje
            _enemyFSM.transform.Translate(direction.normalized * Time.deltaTime * 1.5f, Space.World);
            //Rotación hacia el siguiente punto de patrulla
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _enemyFSM.transform.rotation = Quaternion.Slerp(_enemyFSM.transform.rotation, targetRotation, 0.15f);

            if (Vector3.Distance(_enemyFSM.transform.position, wayPoints[currentWaypointIndex].position) < 0.5f)
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
            if (timer > 10) _enemyFSM.animator.SetBool("isPatrolling", false);
            float distance = Vector3.Distance(player.position, _enemyFSM.transform.position);
            if (distance < chaseRange) _enemyFSM.animator.SetBool("isChasing", true);
        }
    }
    public class ChaseState : EnemyState
    {
        Transform player;
        float chaseSpeed = 3.5f;

        public ChaseState(EnemyFSM enemyFSM) : base() { _enemyFSM = enemyFSM; }

        public override void Enter()
        {
            base.Enter();
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void Execute()
        {
            base.Execute();
            Debug.Log("ChaseState");
            Vector3 direction = (player.position - _enemyFSM.transform.position).normalized;
            //movimiento hacia el jugador
            _enemyFSM.transform.position += direction * chaseSpeed * Time.deltaTime;
            float distance = Vector3.Distance(player.position, _enemyFSM.transform.position);
            if (distance > 15) _enemyFSM.animator.SetBool("isChasing", false);
            if (distance < 14f) _enemyFSM.animator.SetBool("isAttacking", true);
        }
    }
    */
//}
