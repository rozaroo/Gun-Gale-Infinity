using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 100f;
    public float lifeTime = 4f;
    private float startTime;
    public int damageAmount = 20;
    private Transform target;
    private Transform player;
    private BodyPartHitCheck playerBodyPart;

    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    public void SetTarget(Transform newTarget) 
    { 
        target = newTarget; 
        if (target != null ) playerBodyPart = target.GetComponent<BodyPartHitCheck>();
        Vector3 direction = (target.position - transform.position).normalized;
        GetComponent<Rigidbody>().velocity = direction * speed;
    }
    void Update()
    {
        if (Time.time - startTime >= lifeTime) Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.collider.GetComponent<PlayerController>();
        if (player != null) player.TakeDamage(damageAmount);
        Destroy(gameObject);
    }

}
