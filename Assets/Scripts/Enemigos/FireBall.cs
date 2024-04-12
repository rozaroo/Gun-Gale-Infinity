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
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    // Update is called once per frame
    public void SetTarget(Transform newTarget) 
    { 
        target = newTarget; 
        if (target != null ) playerBodyPart = target.GetComponent<BodyPartHitCheck>();
    }
    void Update()
    {
        if (Time.time - startTime >= lifeTime) Destroy(gameObject);
        if (target != null && playerBodyPart != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 movement = direction * speed * Time.deltaTime;
            transform.Translate(movement);
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < 0.5f)
            {
                playerBodyPart.TakeHit(damageAmount);
                Destroy(gameObject);
            }
            else Destroy(gameObject);
            
            
        }
    }
    
}
