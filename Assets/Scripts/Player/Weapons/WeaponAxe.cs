using UnityEngine;

public class WeaponAxe : WeaponBase
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

        player = FindObjectOfType<Player>();

        if (player == null)
        {
            Debug.LogError("Player reference null");
            return;
        }

        playerAttackCooldownTimer = playerAttackCooldown;

        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, attackRadius, targetLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out EnemyBase enemy))
                enemy.TakeDamage(playerAttackPower);

            if (hitCollider.TryGetComponent(out Cocoon cocoon))
                cocoon.TakeDamage(playerAttackPower);
        }
    }
}
