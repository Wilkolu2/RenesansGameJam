using UnityEngine;

public class EnemyBoss1 : EnemyBase
{
    [Header("Boss Attacks")]
    [SerializeField] private int devourHealAmount;
    [SerializeField] private float specialAttackCooldownMax;
    [SerializeField] private float specialAttackCooldownMin;
    [SerializeField] private float leapAttackRange;
    [SerializeField] private float leapAttackForce;
    [SerializeField] private float devourRange;
    [SerializeField] private float pushForce;

    private float specialAttackCurrentCooldown;
    private float specialAttackCooldownTimer;

    public override void Start()
    {
        base.Start();

        specialAttackCurrentCooldown = Random.Range(specialAttackCooldownMin, specialAttackCooldownMax);
        specialAttackCooldownTimer = specialAttackCurrentCooldown;
    }

    private new void Update()
    {
        base.Update();

        CombatLogic();
    }

    private void CombatLogic()
    {
        if (!IsDead)
        {
            if (specialAttackCooldownTimer > 0)
                specialAttackCooldownTimer -= Time.deltaTime;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= enemyAttackRange)
                Attack();

            if (specialAttackCooldownTimer <= 0)// && distanceToPlayer <= leapAttackRange)
            {
                specialAttackCurrentCooldown = Random.Range(specialAttackCooldownMin, specialAttackCooldownMax);
                specialAttackCooldownTimer = specialAttackCurrentCooldown;

                enemyAttackCooldownTimer = enemyAttackCooldown;
                PerformSpecialAttack();
                specialAttackCooldownTimer = specialAttackCurrentCooldown;
            }
        }
    }

    private void PerformSpecialAttack()
    {
        int randomAttack = Random.Range(0, 2);

        switch (randomAttack)
        {
            case 0:
                LeapAttack();
                break;
            case 1:
                DevourAlly();
                break;
            default:
                Debug.Log("No attack performed");
                break;
        }
    }

    protected override void Attack()
    {
        if (enemyAttackCooldownTimer <= 0 && Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
        {
            MeleeAttack();
            enemyAttackCooldownTimer = enemyAttackCooldown;
        }
    }

    private void MeleeAttack()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(enemyAttack);
            PushPlayer();
        }
    }

    private void PushPlayer()
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 pushDirection = (player.position - transform.position).normalized;
            playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }

    private void LeapAttack()
    {
        if (Vector3.Distance(transform.position, player.position) <= leapAttackRange)
        {
            Vector3 leapDirection = (player.position - transform.position).normalized;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(leapDirection * leapAttackForce, ForceMode.Impulse);
                Invoke("MeleeAttack", 0.5f);
            }
        }
    }

    private void DevourAlly()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, devourRange);
        foreach (var hitCollider in hitColliders)
        {
            EnemyBase ally = hitCollider.GetComponent<EnemyBase>();
            if (ally != null && ally != this && !ally.IsDead && enemyCurHp <= 5)
            {
                ally.TakeDamage(ally.EnemyMaxHp);
                enemyCurHp = Mathf.Min(enemyCurHp + devourHealAmount, EnemyMaxHp);
                return;
            }
        }
    }
}
