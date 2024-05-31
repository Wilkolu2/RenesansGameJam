using UnityEngine;

public class WeaponBlunderbuss : WeaponBase
{
    [SerializeField] private GameObject projectilePrefab;

    public override void Attack(Vector3 targetPosition)
    {
        //if (!CanAttack() || firePoint == null)
            //return;

        Debug.Log("Blunderbuss attack");

        playerAttackCooldownTimer = playerAttackCooldown;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        PlayerProjectile projScript = projectile.GetComponent<PlayerProjectile>();
        projScript.Initialize(targetPosition, playerAttackPower);

        // Animation
    }
}
