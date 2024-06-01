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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rb.velocity = CalculateVelocity();
        Destroy(gameObject, lifetime);
    }

    /*
    private void Update()
    {
        FacePlayer();
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    }
    */

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
