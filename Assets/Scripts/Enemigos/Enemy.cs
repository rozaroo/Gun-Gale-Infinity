using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;
    private Transform player;
    public GameObject[] PuntosdePatrullaje;
    public float speed;

    ILineOfSight _los;
    LineOfSight lineOfSight;
    private void Awake()
    {
        _los = GetComponent<ILineOfSight>();
        lineOfSight = GetComponent<LineOfSight>();
    }
    public LineOfSight LineOfSight {  get { return lineOfSight; } }
    public ILineOfSight LOS { get { return _los; } }

    void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player").transform;
        BodyPartHitCheck playerBodyPart = player.GetComponent<BodyPartHitCheck>();
    }

    void Update() 
    { 
        healthBar.value = HP;
    }
    
    public void TakeDamage(int damageAmount) 
    {
        HP -= damageAmount;
        if (HP < 0) 
        { 
            animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 5);
        }
        else animator.SetTrigger("damage");
    }
    public void ShootFireball() 
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
        FireBall fireballScript = fireball.GetComponent<FireBall>();
        if (fireballScript != null && player != null) fireballScript.SetTarget(player);
    }
    public void Move(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * speed;
    }
    

}
