using UnityEngine;

public class WeaponHammer : WeaponBase
{
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public override void Attack(Vector3 targetPosition)
    {
        if (!CanAttack())
            return;

        Debug.Log("Hammer attack");

        playerAttackCooldownTimer = playerAttackCooldown;

        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, attackRadius, targetLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out EnemyBase enemy))
            {
                enemy.TakeDamage(playerAttackPower);
                Debug.Log("Enemy hit by hammer");
            }
            if (hitCollider.TryGetComponent(out Cocoon cocoon))
            {
                cocoon.TakeDamage(playerAttackPower);
                Debug.Log("Cocoon hit by hammer");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerAttackRange);
    }
}
