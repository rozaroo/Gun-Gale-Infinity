using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatesEnumCuatro
{
    Quiet,
    Move,
    Destroy
}

public class SpaceShipController : MonoBehaviour
{
    FSM<StatesEnumCuatro> _fsm;
    //Nave
    public Transform shipTr;
    public Rigidbody shipRb;
    public float currentHealth;
    public Vector2 newDirection;
    public int Speed;
    public GameObject explosionPrefab;
    public GameObject shotPrefab;
    public Transform shotSpawnPoint;
    private void Awake()
    {
        InitializeFSM();
    }
    void InitializeFSM()
    {
        _fsm = new FSM<StatesEnumCuatro>();
        var Quiet = new QuietState<StatesEnumCuatro>(this, StatesEnumCuatro.Move, StatesEnumCuatro.Destroy);
        var Move = new MoveState<StatesEnumCuatro>(this, StatesEnumCuatro.Quiet, StatesEnumCuatro.Destroy);
        var Destroy = new DestroyState<StatesEnumCuatro>(this);
        Quiet.AddTransition(StatesEnumCuatro.Move, Move);
        Quiet.AddTransition(StatesEnumCuatro.Destroy, Destroy);
        Move.AddTransition(StatesEnumCuatro.Quiet, Quiet);
        Move.AddTransition(StatesEnumCuatro.Destroy, Destroy);
        Destroy.AddTransition(StatesEnumCuatro.Move, Move);
        Destroy.AddTransition(StatesEnumCuatro.Quiet, Quiet);
        _fsm.SetInit(Quiet);
    }
    void Start()
    {
        shipTr = this.transform;
        shipRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.OnUpdate();
    }
    public void DestroyShip()
    {
        StartCoroutine(EmitExplosionForTime(0.5f));
    }
    private IEnumerator EmitExplosionForTime(float duration)
    {
        GameObject explosionObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        ExplosionSystem explosionSystem = explosionObject.GetComponent<ExplosionSystem>();
        if (explosionSystem != null)
        {
            explosionSystem.Play();
            yield return new WaitForSeconds(duration);
            explosionSystem.Stop();
            Destroy(gameObject);
            Destroy(explosionObject, explosionSystem.main.startLifetime.constantMax);
        }
    }
    public void Shoot()
    {
        if (shotPrefab != null && shotSpawnPoint != null) Instantiate(shotPrefab, shotSpawnPoint.position, shotSpawnPoint.rotation);
    }
}
