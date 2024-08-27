using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public enum StatesEnumDos
{
    Idle,
    Dead,
    Steering
}

public class EnemyControllerTwo : MonoBehaviour, ILineOfSight
{
    public float distance;
    public Transform player;
    LineOfSight _los;
    FSM<StatesEnumDos> _fsm;
    ITreeNode _root;
    Func<bool> QuestionRange;
    QuestionNode auxiliarnode;
    //Line of Sight
    public float range;
    [Range(1, 360)]
    public float angle;
    public LayerMask maskObs;
    Vector3 posplayer;

    //Steerings behaviours
    public Rigidbody target;
    public float timePrediction;
    public float radius;
    ISteering _steering;
    ObstacleAvoidance _obstacleAvoidance;
    LevelManager lvlManager;
    AgentControllerTwo agentControllertwo;
    #region EnemyTwo
    Quaternion targetRotation;
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;
    public float speed;
    public DropPrefabstwo dropPrefabs;
    public DropProbabilities dropProbabilities;
    public Transform dropSpawnPoint;
    //------------------------
    float timer;
    Rigidbody _rb;
    #endregion


    private void Awake()
    {
        lvlManager = FindObjectOfType<LevelManager>();
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        healthBar.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InitializeSteerings();
        InitializeFSM();
        InitializedTree();
    }
    private void Update()
    {
        healthBar.value = HP;
        UpdateHealthBarVisibility();
        distance = Vector3.Distance(player.position, transform.position);
        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
    }
    void InitializeSteerings()
    {
        var evade = new Evade(this.transform, target, timePrediction);
        _steering = evade;
        _obstacleAvoidance = new ObstacleAvoidance(this.transform, angle, radius, maskObs, 2.5f);
    }
    void InitializeFSM()
    {
        var idle = new EnemyIdleState<StatesEnumDos>(this);
        var dead = new DeathStateTwo<StatesEnumDos>(this, lvlManager);
        var steering = new EnemyStateSteeringTwo<StatesEnumDos>(this, _steering, _obstacleAvoidance);
        idle.AddTransition(StatesEnumDos.Steering, steering);
        idle.AddTransition(StatesEnumDos.Dead, dead);
        dead.AddTransition(StatesEnumDos.Steering, steering);
        dead.AddTransition(StatesEnumDos.Idle, idle);
        steering.AddTransition(StatesEnumDos.Dead, dead);
        steering.AddTransition(StatesEnumDos.Idle, idle);
        _fsm = new FSM<StatesEnumDos>(idle);
    }
    void InitializedTree()
    {
        var dead = new ActionNode(() => _fsm.Transition(StatesEnumDos.Dead));
        var steering = new ActionNode(() => _fsm.Transition(StatesEnumDos.Steering));
        var idle = new ActionNode(() => _fsm.Transition(StatesEnumDos.Idle));
        var qLoS = new QuestionNode(QuestionLosPlayer(), steering, idle);
        var qHasLife = new QuestionNode(QuestionHP(), dead, qLoS);
        _root = qHasLife;
    }
    #region Questions
    Func<bool> QuestionRango()
    {
        return () => distance < (range - 10f);
    }
    Func<bool> QuestionLos()
    {
        return () => !(CheckRange(player) && CheckAngle(player) && CheckView(player));
    }
    Func<bool> QuestionLosPlayer()
    {
        return () => (CheckRange(player) && CheckAngle(player) && CheckView(player));
    }
    Func<bool> QuestionHP()
    {
        return () => HP <= 0;
    }

    #endregion
    #region LineOfSight
    //Line Of Sight

    public bool CheckRange(Transform target)
    {
        float distance = Vector3.Distance(target.position, Origin);
        return distance <= range;
    }

    public bool CheckAngle(Transform target)
    {
        Vector3 dirToTarget = target.position - Origin;
        float angleToTarget = Vector3.Angle(Forward, dirToTarget);
        return angleToTarget <= angle / 2;
    }

    public bool CheckView(Transform target)
    {
        posplayer = target.position;
        return !Physics.Linecast(Origin + new Vector3(0, 1, 0), target.position + new Vector3(0, 1, 0), maskObs);
    }
    Vector3 Origin => transform.position;
    Vector3 Forward => transform.forward;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Origin, range);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(Origin, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, angle / 2, 0) * Forward * range);
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, -(angle / 2), 0) * Forward * range);
    }
    #endregion

    #region EnemyTwo
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        animator.SetTrigger("damage");
    }

    public void SpawnRandomDrop()
    {
        if (dropPrefabs.prefabs.Length == 0 || dropProbabilities.probabilities.Length == 0 || dropPrefabs.prefabs.Length != dropProbabilities.probabilities.Length) return;
        float randomValue = UnityEngine.Random.value;
        //Dtermino que prefab spawmear basado en las probabilidades 
        float cumulativeProbability = 0f;
        for (int i = 0; i < dropProbabilities.probabilities.Length; i++)
        {
            cumulativeProbability += dropProbabilities.probabilities[i];
            if (randomValue < cumulativeProbability)
            {
                Instantiate(dropPrefabs.prefabs[i], dropSpawnPoint.position, Quaternion.identity);
                break;
            }
        }
    }

    public void Move(Vector3 dir)
    {
        transform.position += dir * Time.deltaTime * speed;
        //dir *= speed;
        //dir.y = _rb.velocity.y;
        //_rb.velocity = dir;
    }
    public void LookDir(Vector3 dir)
    {
        if (dir.x == 0 && dir.z == 0) return;
        transform.forward = dir;
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    void UpdateHealthBarVisibility()
    {
        bool isVisible = CheckRange(player) && CheckAngle(player) && CheckView(player);
        healthBar.gameObject.SetActive(isVisible);
    }
    #endregion
}
