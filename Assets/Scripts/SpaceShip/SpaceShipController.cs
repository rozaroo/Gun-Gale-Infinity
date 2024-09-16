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
    public int municionPorCartucho; //Cantidad de balas por cartucho
    public int maxCartuchos; //Cantidad máxima de cartuchos
    private Stack<int> municion; //Pila para la munición
    private Queue<int> cartuchos; //Cola para los cartuchos
    public AudioClip shotSound;
    private AudioSource audioSource;
    private float maxHealth = 1000f;
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
        audioSource = GetComponent<AudioSource>();
        //Inicializamos las pilas y colas
        municion = new Stack<int>();
        cartuchos = new Queue<int>();
        //Llenamos la pila con munición inicial
        for (int i = 0; i < municionPorCartucho; i++)
            municion.Push(1); //Cada bala ocupa un "espacio" en la pila
        //Llenamos la cola con cartuchos
        for (int i = 0; i < maxCartuchos; i++)
            cartuchos.Enqueue(municionPorCartucho); //Cada cartucho tiene una cantidad de disparos
        currentHealth = maxHealth;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.OnUpdate();
    }
    public void DestroyShip()
    {
        StartCoroutine(PlayDestructionAnimation(0.5f));
    }
    private IEnumerator PlayDestructionAnimation(float duration)
    {
        GameObject explosionObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
        Destroy(explosionObject, duration);
    }
    public void Shoot()
    {
        if (municion.Count > 0)
        {
            //Disminuye la munición
            municion.Pop();
            //Reproducir sonido
            if (audioSource != null && shotSound != null) audioSource.PlayOneShot(shotSound);
            if (shotPrefab != null && shotSpawnPoint != null) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Vector3 targetPoint;
                if (Physics.Raycast(ray, out hit)) targetPoint = hit.point;
                else targetPoint = ray.GetPoint(1000);
                Vector3 shootDirection = (targetPoint - shotSpawnPoint.position).normalized;
                GameObject shot = Instantiate(shotPrefab,shotSpawnPoint.position, Quaternion.LookRotation(shootDirection));
                Rigidbody shotRb = shot.GetComponent<Rigidbody>();
                if (shotRb != null) shotRb.velocity = shootDirection * shotPower;
            };
        }
    }
    public void Reload()
    {
        if (cartuchos.Count > 0 && municion.Count == 0)
        {
            //Recargamos munición de un cartucho
            int disparos = cartuchos.Dequeue();
            for (int i = 0; i < disparos; i++)
                municion.Push(1);
            Debug.Log("Recargado. Munición disponible: " + municion.Count);
        }
        else if (cartuchos.Count == 0) Debug.Log("Sin cartuchos restantes.");
        else if (municion.Count > 0) Debug.Log("Aún tienes munición");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
    public int MunicionActual()
    {
        return municion.Count;
    }
    public int CartuchosDisponibles()
    {
        return cartuchos.Count;
    }
}
