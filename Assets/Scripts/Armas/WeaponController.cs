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
    public GameObject explosionPrefab;
    void Start()
    {
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
    public void InstantiateBullet()
    {
        Vector3 spawnPosition = shootSpawn.position + shootSpawn.forward * 0.1f;
        Instantiate(bulletPrefab, spawnPosition, shootSpawn.rotation);
        StartCoroutine(PlayExplosion(0.2f, spawnPosition));
    }
    IEnumerator AutomaticShoot()
    {
        while (shooting)
        {
            InstantiateBullet();
            yield return new WaitForSeconds(shootDelay);
        }
    }
    private IEnumerator PlayExplosion(float duration, Vector3 position)
    {
        GameObject explosionObject = Instantiate(explosionPrefab, position, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        Destroy(explosionObject, duration);
    }
}
