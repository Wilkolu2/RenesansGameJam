using UnityEngine;

public class EnemyMelee : EnemyBase
{
    protected override void Attack()
    {
        if (enemyAttackCooldownTimer <= 0 && Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
        {
            Debug.Log("Melee attack");

            if (playerHealth != null)
                playerHealth.TakeDamage(enemyAttack);

            enemyAttackCooldownTimer = enemyAttackCooldown;
        }
    }

    private new void Update()
    {
        base.Update();

        if (Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
            Attack();
    }
}
