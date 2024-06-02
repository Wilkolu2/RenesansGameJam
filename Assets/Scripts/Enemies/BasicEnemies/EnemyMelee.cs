using UnityEngine;

public class EnemyMelee : EnemyBase
{
    protected override void Attack()
    {
        base.Attack();

        if (enemyAttackCooldownTimer <= 0 && Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
        {
            if (playerHealth != null)
                playerHealth.TakeDamage(enemyAttack);

            audioSource.PlayOneShot(attackSound);

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
