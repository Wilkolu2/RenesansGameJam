using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int playerHpMax;
    [SerializeField] private WeaponBase playerWeaponCur;
    [SerializeField] private Transform firePoint;

    private int playerHpCur;
    private Vector3 playerPos;
    private GameObject currentWeaponModel;

    private void Start()
    {
        playerHpCur = playerHpMax;
        AttachWeaponModel();
    }

    private void Update()
    {
        playerPos = transform.position;

        if (Input.GetKeyDown(KeyCode.Q))
            Attack();

        if (Input.GetKeyDown(KeyCode.E))
            SwitchWeapon();
    }

    public void AttachWeaponModel()
    {
        if (playerWeaponCur == null)
            return;

        if (currentWeaponModel != null)
            Destroy(currentWeaponModel);

        Transform weaponHoldPoint = PlayerController.instance.weaponHoldPoint;
        playerWeaponCur.SetFirePoint(firePoint);
        currentWeaponModel = Instantiate(playerWeaponCur.weaponModelPrefab, weaponHoldPoint);
        currentWeaponModel.transform.localPosition = Vector3.zero;
        currentWeaponModel.transform.localRotation = Quaternion.identity;
    }

    public void Attack()
    {
        if (playerWeaponCur != null && playerWeaponCur.CanAttack())
        {
            Vector3 targetPosition = GetAttackTargetPosition();
            playerWeaponCur.Attack(targetPosition);
        }
    }

    private Vector3 GetAttackTargetPosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
    }

    public void SwitchWeapon()
    {
        if (currentWeaponModel != null)
            Destroy(currentWeaponModel);

        if (playerWeaponCur is WeaponHammer)
            playerWeaponCur = Instantiate(Resources.Load<WeaponBase>("Prefabs/Weapons/WeaponBlunderbuss"));
        else
            playerWeaponCur = Instantiate(Resources.Load<WeaponBase>("Prefabs/Weapons/WeaponHammer"));

        AttachWeaponModel();
    }

    public void ModifyPlayerHp(int amount)
    {
        playerHpCur = Mathf.Clamp(playerHpCur + amount, 0, playerHpMax);
    }

    public void SetPlayerCurHp(int value)
    {
        playerHpCur = value;
    }

    public int GetPlayerCurHp()
    {
        return playerHpCur;
    }

    public int GetPlayerMaxHp()
    {
        return playerHpMax;
    }
}
