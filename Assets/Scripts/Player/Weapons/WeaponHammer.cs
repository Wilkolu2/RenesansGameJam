using UnityEngine;

public class WeaponHammer : WeaponBase
{
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask targetLayer;
    private Player player;

    public void Initialize(Player player)
    {
        this.player = player;
    }

    public override void Attack(Vector3 targetPosition)
    {
        if (!CanAttack())
            return;

        if (player == null)
        {
            Debug.LogError("Player reference null");
            return;
        }

        playerAttackCooldownTimer = playerAttackCooldown;

        //player

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
        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }

        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, attackRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.transform.position, playerAttackRange);
        }
    }
}