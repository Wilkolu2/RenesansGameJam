using UnityEngine;

public class EnemyRanged : EnemyBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    protected override void Attack()
    {
        if (attackCooldownTimer <= 0 && Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
        {
            // logika ataku dystansowego
            ShootProjectile();

            attackCooldownTimer = attackCooldown;
        }
    }

    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * enemyMoveSpeed;
    }

    private void Update()
    {
        base.Update();
        if (Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
        {
            Attack();
        }
    }
}

