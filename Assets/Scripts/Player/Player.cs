using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int playerHpMax;
    [SerializeField] private WeaponBase playerWeaponCur;

    private int playerHpCur;
    private Vector3 playerPos;

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

        //if (Input.GetKeyDown(KeyCode.Q))
        //SwitchWeapon();
    }

    public void AttachWeaponModel()
    {
        if (playerWeaponCur == null) return;

        Transform weaponHoldPoint = PlayerController.instance.weaponHoldPoint;

        GameObject weaponModel = Instantiate(playerWeaponCur.weaponModelPrefab, weaponHoldPoint);
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
    }

    public void Attack()
    {
        Vector3 targetPosition = GetAttackTargetPosition();
        playerWeaponCur.Attack(targetPosition);
    }

    private Vector3 GetAttackTargetPosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
    }

    public void SwitchWeapon()
    {

    }

    public void SetPlayerCurHp(int hp) => playerHpCur = hp;
    public void ModifyPlayerHp(int amount) => playerHpCur += amount;
    public int GetPlayerCurHp() => playerHpCur;
    public int GetPlayerMaxHp() => playerHpMax;
    public Vector3 GetPlayerPos() => playerPos;
}
