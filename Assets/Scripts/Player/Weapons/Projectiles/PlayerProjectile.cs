using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector3 targetPosition;
    private int attackPower;

    public void Initialize(Vector3 targetPosition, int attackPower)
    {
        this.targetPosition = targetPosition;
        this.attackPower = attackPower;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out EnemyBase enemy))
                {
                    enemy.TakeDamage(attackPower);
                }
            }
            Destroy(gameObject);
        }
    }
}
