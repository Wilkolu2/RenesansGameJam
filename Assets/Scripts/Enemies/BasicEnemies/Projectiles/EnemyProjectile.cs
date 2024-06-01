using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] protected float lifetime = 5f;
    public float speed = 10f;
    protected Transform player;
    protected Rigidbody rb;
    protected int damageAmount;
    protected Vector3 targetPosition;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = CalculateVelocity(); // Calculate initial velocity
        Destroy(gameObject, lifetime);
    }

    public void SetDamage(int amount)
    {
        damageAmount = amount;
    }

    public void SetTarget(Vector3 targetPos)
    {
        targetPosition = targetPos;
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

    protected virtual Vector3 CalculateVelocity()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        return direction * speed;
    }
}
