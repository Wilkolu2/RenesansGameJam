using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class WeaponBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int playerAttackPower;
    [SerializeField] protected float playerAttackSpeed;
    [SerializeField] protected float playerAttackRange;
    [SerializeField] protected float playerAttackCooldown;
    [SerializeField] public GameObject weaponModelPrefab;

    protected float playerAttackCooldownTimer;

    public abstract void Attack(Vector3 targetPosition);

    protected void Start()
    {
        playerAttackCooldownTimer = 0f;
    }

    protected void Update()
    {
        HandleAttackCooldown();
    }

    protected void HandleAttackCooldown()
    {
        if (playerAttackCooldownTimer > 0f)
            playerAttackCooldownTimer -= Time.deltaTime;
    }

    public bool CanAttack()
    {
        return playerAttackCooldownTimer <= 0f;
    }
}
