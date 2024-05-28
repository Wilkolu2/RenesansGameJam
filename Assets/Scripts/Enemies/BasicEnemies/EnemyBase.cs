using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int enemyAttack;
    [SerializeField] protected float enemyMoveSpeed;
    [SerializeField] protected float enemyAttackRange;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected EnemyType enemyType;

    protected Transform player;
    protected float attackCooldownTimer;

    protected enum EnemyType
    {
        Melee,
        Ranged
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackCooldownTimer = 0f;
    }

    protected void Update()
    {
        FacePlayer();
        MoveTowardsPlayer();
        HandleAttackCooldown();
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    }

    private void MoveTowardsPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > enemyAttackRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * enemyMoveSpeed * Time.deltaTime;
        }
    }

    protected virtual void Attack() { }

    private void HandleAttackCooldown()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }
}