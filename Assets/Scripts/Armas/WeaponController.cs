using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform shootSpawn;
    public bool shooting = false;
    public float shootDelay = 0f;
    public float lastShootTime = 0f;
    public GameObject itemPrefab;
    public GameObject bulletPrefab;
    public Sprite weaponIcon;
    public PlayerController playerController;
    //Pila de balas
    public Stack<int> municion = new Stack<int>();
    //Cola de cartuchos
    public Queue<int> cartuchos = new Queue<int>();
    //Capacidad de balas por cartucho
    public int balasPorCartucho = 16;

    void Start()
    {
        //Inicializar con munición y cartuchos de ejemplo
        for (int i = 0; i < 3; i++)
            municion.Push(balasPorCartucho);//Añade 16 balas al cartucho
        for (int i = 0; i < 2; i++)
            cartuchos.Enqueue(balasPorCartucho); //Añade 2 cartuchos en total
        playerController = FindObjectOfType<PlayerController>();
    }

    public enum ShootMode
    {
        Single,
        Auto
    }
    public ShootMode currentShootMode = ShootMode.Single;

    void Update()
    {
        if (playerController != null) 
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && (playerController.hasPistol || playerController.hasRiffle))
            {
                shooting = true;
                Shoot();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0)) shooting = false;
        }
        if (Input.GetKeyDown(KeyCode.R)) Recargar();
        Debug.DrawLine(shootSpawn.position, shootSpawn.forward * 10f, Color.red);
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 10f, Color.blue);
        RaycastHit cameraHit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out cameraHit))
        {
            Vector3 shootDirection = cameraHit.point - shootSpawn.position;
            shootSpawn.rotation = Quaternion.LookRotation(shootDirection);
        }
    }
    public void Shoot()
    {
        if (Time.time - lastShootTime > shootDelay)
        {
            if (shooting)
            {
                switch (currentShootMode)
                {
                    case ShootMode.Single:
                        InstantiateBullet();
                        break;
                    case ShootMode.Auto:
                        StartCoroutine(AutomaticShoot());
                        break;
                }
            }
        }

    }
    public void Recargar()
    {
        if (cartuchos.Count == 0) return;
        //Extraer un cartucho de la cola y cargar la pila de balas
        int balasRecargadas = cartuchos.Dequeue();
        municion.Push(balasRecargadas);
    }
    public void InstantiateBullet()
    {
        Vector3 spawnPosition = shootSpawn.position + shootSpawn.forward * 0.1f;
        Instantiate(bulletPrefab, spawnPosition, shootSpawn.rotation);
    }
    IEnumerator AutomaticShoot()
    {
        while (shooting)
        {
            if (municion.Count == 0 || municion.Peek() <= 0)
            {
                Debug.Log("Sin munición. Recarga!");
            }
            //Restar una bala
            municion.Push(municion.Pop() - 1);
            InstantiateBullet();
            yield return new WaitForSeconds(shootDelay);
        }
    }
    public void AddCartridge()
    {
        cartuchos.Enqueue(balasPorCartucho);
    }
}
