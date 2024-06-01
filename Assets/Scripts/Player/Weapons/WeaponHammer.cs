using UnityEngine;

public class WeaponHammer : WeaponBase
{
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask targetLayer;

    public override void Attack(Vector3 targetPosition)
    {
        //if (!CanAttack())
            //return;

        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAA");

        playerAttackCooldownTimer = playerAttackCooldown;

        Collider[] hitColliders = Physics.OverlapSphere(targetPosition, attackRadius, targetLayer);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out EnemyBase enemy))
                enemy.TakeDamage(playerAttackPower);
        }

        // Animation and sound
    }
}
