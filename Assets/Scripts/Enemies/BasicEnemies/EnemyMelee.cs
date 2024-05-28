using UnityEngine;

public class EnemyMelee : EnemyBase
{
    protected override void Attack()
    {
        if (attackCooldownTimer <= 0 && Vector3.Distance(transform.position, player.position) <= enemyAttackRange)
        {
            Debug.Log("Melee attack");

            attackCooldownTimer = attackCooldown;
        }
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
