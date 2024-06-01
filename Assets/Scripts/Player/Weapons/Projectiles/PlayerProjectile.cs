using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private LayerMask enemyLayer;

    private Vector3 targetPosition;
    private int attackPower;
    private Rigidbody rb;
    private Transform player;

    public void Initialize(Vector3 targetPosition, int attackPower)
    {
        this.targetPosition = targetPosition;
        this.attackPower = attackPower;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        MoveTowardsTarget();
        //FacePlayer();
    }

    private void MoveTowardsTarget()
    {
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        rb.velocity = rb.gameObject.transform.forward * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            Destroy(gameObject);
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            if (other.TryGetComponent(out Cocoon cocoon))
            {
                cocoon.TakeDamage(attackPower);
                Destroy(gameObject);
            }
            else if (other.TryGetComponent(out EnemyBase enemy))
            {
                enemy.TakeDamage(attackPower);
                Destroy(gameObject);
            }
        }
    }
}
