using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int enemyAttack;
    [SerializeField] protected float enemyMoveSpeed;
    [SerializeField] protected float enemyAttackRange;
    [SerializeField] protected float enemyAttackCooldown;
    [SerializeField] protected int enemyMaxHp = 1;

    protected int enemyCurHp;
    protected float enemyAttackCooldownTimer;
    protected Transform player;
    protected PlayerHealth playerHealth;
    protected bool isDead = false;

    private WaveManager waveManager;

    public virtual void Start()
    {
        enemyCurHp = enemyMaxHp;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        waveManager = FindObjectOfType<WaveManager>();
        enemyAttackCooldownTimer = 0f;
    }

    protected void Update()
    {
        if (isDead) return;

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

    public void TakeDamage(int damage)
    {
        enemyCurHp -= damage;

        if (enemyCurHp <= 0 && !isDead)
            Die();
    }

    protected virtual void Attack() { }

    private void HandleAttackCooldown()
    {
        if (enemyAttackCooldownTimer > 0f)
            enemyAttackCooldownTimer -= Time.deltaTime;
    }

    private void Die()
    {
        Debug.Log("Enemy died");
        isDead = true;
        waveManager.OnEnemyKilled();
    }

    public int GetEnemyAttackAmount() => enemyAttack;
}
