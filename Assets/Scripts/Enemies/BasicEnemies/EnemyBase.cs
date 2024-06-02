using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int enemyAttack;
    [SerializeField] protected float enemyMoveSpeed;
    [SerializeField] protected float enemyAttackRange;
    [SerializeField] protected float enemyAttackCooldown;
    [SerializeField] protected int enemyMaxHp = 1;
    [SerializeField] protected AudioClip attackSound;
    [SerializeField] protected AudioClip deathSound;

    protected int enemyCurHp;
    protected float enemyAttackCooldownTimer;
    protected Transform player;
    protected PlayerHealth playerHealth;
    protected bool isDead = false;
    protected Animator animator;
    protected AudioSource audioSource;

    private WaveManager waveManager;

    public int EnemyMaxHp => enemyMaxHp;
    public bool IsDead => isDead;

    public event System.Action<EnemyBase> OnDeath;

    public virtual void Start()
    {
        enemyCurHp = enemyMaxHp;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        waveManager = FindObjectOfType<WaveManager>();
        enemyAttackCooldownTimer = 0f;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected void Update()
    {
        if (isDead)
            return;

        FacePlayer();
        MoveTowardsPlayer();
        HandleAttackCooldown();

        // If in attack range, attack
        if (Vector3.Distance(transform.position, player.position) <= enemyAttackRange && enemyAttackCooldownTimer <= 0f)
        {
            Attack();
            enemyAttackCooldownTimer = enemyAttackCooldown;
        }
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
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    public void TakeDamage(int damage)
    {
        enemyCurHp -= damage;

        if (enemyCurHp <= 0 && !isDead)
            Die();
    }

    protected virtual void Attack()
    {
        animator.SetTrigger("Attack");
    }

    private void HandleAttackCooldown()
    {
        if (enemyAttackCooldownTimer > 0f)
        {
            enemyAttackCooldownTimer -= Time.deltaTime;
        }
    }

    private void Die()
    {
        isDead = true;
        OnDeath?.Invoke(this);
        StartCoroutine(PlayDeathSoundAndDestroy());
    }

    private IEnumerator PlayDeathSoundAndDestroy()
    {
        audioSource.PlayOneShot(deathSound);

        yield return new WaitForSeconds(deathSound.length);

        Destroy(gameObject);
    }

    public int GetEnemyAttackAmount() => enemyAttack;
}
