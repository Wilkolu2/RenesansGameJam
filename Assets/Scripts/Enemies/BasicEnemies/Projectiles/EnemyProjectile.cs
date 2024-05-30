using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] protected float lifetime = 5f;
    public float speed = 10f;
    protected Transform player;
    protected Rigidbody rb;
    protected int damageAmount;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    public void SetDamage(int amount)
    {
        damageAmount = amount;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit player");

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damageAmount);

            Destroy(gameObject);
        }
    }
}
