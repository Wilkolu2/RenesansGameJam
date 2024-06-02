using UnityEngine;

public class EnemyRanged : EnemyBase
{
    [SerializeField] private GameObject straightProjectilePrefab;
    [SerializeField] private GameObject arcProjectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private bool useArcProjectile = false;

    protected override void Attack()
    {
        base.Attack();

        if (enemyAttackCooldownTimer <= 0 && Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
        {
            ShootProjectile();

            audioSource.PlayOneShot(attackSound);

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

    private new void Update()
    {
        base.Update();
        if (Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
            Attack();
    }
}
