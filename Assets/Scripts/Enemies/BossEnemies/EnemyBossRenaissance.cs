using UnityEngine;

public class EnemyBossRenaissance : EnemyBase
{
    [Header("Boss Attacks")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject straightProjectilePrefab;
    [SerializeField] private GameObject arcProjectilePrefab;
    [SerializeField] private bool useArcProjectile = false;

    public override void Start()
    {
        base.Start();
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
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= enemyAttackRange)
                Attack();
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
            enemyProjectile.SetTarget(player.position);
            enemyProjectile.SetDamage(enemyAttack);
        }
    }
}
