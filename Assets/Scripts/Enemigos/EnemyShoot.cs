using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public float speed;
    private float lifeTime = 4f;
    private float startTime;
    private int damageAmount = 20;
    private Transform target;
    private Transform player;

    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    public void SetTarget(Transform newTarget) 
    { 
        target = newTarget; 
        var targetPos = target.position;
        targetPos.y = transform.position.y;
        Vector3 direction = (targetPos - transform.position).normalized;
        GetComponent<Rigidbody>().velocity = direction * speed;
    }
    void Update()
    {
        if (Time.time - startTime >= lifeTime) Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<SpaceShipController>();
        if (player != null) player.TakeDamage(damageAmount);
        if (other.transform.tag == "Nave") Destroy(gameObject);
    }

}
