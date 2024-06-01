using UnityEngine;

public class EnemyBoss : EnemyBase
{
    [Header("Boss Attacks")]
    [SerializeField] private float specialAttackCooldown;
    [SerializeField] private GameObject specialAttack1Prefab;
    [SerializeField] private GameObject specialAttack2Prefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject straightProjectilePrefab;
    [SerializeField] private GameObject arcProjectilePrefab;
    [SerializeField] private bool useArcProjectile = false;

    private float specialAttackCooldownTimer;

    public override void Start()
    {
        base.Start();
        specialAttackCooldownTimer = specialAttackCooldown;
    }

    private new void Update()
    {
        base.Update();

        if (Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
            Attack();

        if (!isDead && Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
        {
            if (specialAttackCooldownTimer <= 0)
            {
                PerformSpecialAttack();
                specialAttackCooldownTimer = specialAttackCooldown;
            }
        }
    }

    private void PerformSpecialAttack()
    {
        int randomAttack = Random.Range(0, 1);

        switch (randomAttack)
        {
            case 0:
                SpecialAttack1();
                break;
            case 1:
                SpecialAttack2();
                break;
        }
    }

    private void SpecialAttack1()
    {
        if (specialAttack1Prefab != null)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            Instantiate(specialAttack1Prefab, spawnPosition, Quaternion.identity);
        }
    }

    private void SpecialAttack2()
    {
        if (specialAttack2Prefab != null)
        {
            if (specialAttack1Prefab != null)
            {
                Vector3 spawnPosition = new Vector3(transform.position.x, 0f, transform.position.z);
                Instantiate(specialAttack1Prefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    protected override void Attack()
    {
        if (enemyAttackCooldownTimer <= 0 && Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
        {
            ShootProjectile();
            enemyAttackCooldownTimer = enemyAttackCooldown;
        }
    }

    private void ShootProjectile()
    {
        GameObject projectilePrefab = useArcProjectile ? arcProjectilePrefab : straightProjectilePrefab;
        if (projectilePrefab == null)
            return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();

        if (enemyProjectile != null)
        {
            enemyProjectile.SetTarget(player.position); // Set the target position
            enemyProjectile.SetDamage(enemyAttack);
        }
    }
}
