using UnityEngine;

public class WeaponBlunderbuss : WeaponBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    public override void Attack(Vector3 targetPosition)
    {
        if (!CanAttack()) return;

        playerAttackCooldownTimer = playerAttackCooldown;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        PlayerProjectile projScript = projectile.GetComponent<PlayerProjectile>();
        projScript.Initialize(targetPosition, playerAttackPower, playerAttackSpeed);

        // animacja
    }
}
