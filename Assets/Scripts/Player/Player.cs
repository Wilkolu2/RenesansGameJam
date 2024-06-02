using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int playerHpMax;
    [SerializeField] private WeaponBase playerWeaponCur;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int enemiesUntilLifeRegen = 50;

    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip attackClip;

    private static Player instance;
    private int spareLives = 1;
    private int enemiesKilled = 0;
    private int wavesUntilLifeRegen = 3;
    private int wavesCleared = 0;

    private int playerHpCur;
    private Vector3 playerPos;
    private GameObject currentWeaponModel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        playerHpCur = playerHpMax;
        AttachWeaponModel();
    }

    private void Update()
    {
        playerPos = transform.position;

        if (Input.GetKeyDown(KeyCode.Mouse0))
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
            audioSource.PlayOneShot(attackClip);
        }
    }

    private Vector3 GetAttackTargetPosition()
    {
        return transform.position + transform.forward * playerWeaponCur.playerAttackRange;
    }

    public void SwitchWeapon()
    {
        if (currentWeaponModel != null)
            Destroy(currentWeaponModel);

        if (playerWeaponCur is null)
            playerWeaponCur = Instantiate(Resources.Load<WeaponBase>("Prefabs/Weapons/WeaponHammer"));
        else if (playerWeaponCur is WeaponHammer)
            playerWeaponCur = Instantiate(Resources.Load<WeaponBase>("Prefabs/Weapons/WeaponBlunderbuss"));
        else if (playerWeaponCur is WeaponBlunderbuss)
            playerWeaponCur = Instantiate(Resources.Load<WeaponBase>("Prefabs/Weapons/WeaponHammer"));
        else if (playerWeaponCur is WeaponAxe)
            playerWeaponCur = Instantiate(Resources.Load<WeaponBase>("Prefabs/Weapons/WeaponCrossbow"));
        else if (playerWeaponCur is WeaponCrossbow)
            playerWeaponCur = Instantiate(Resources.Load<WeaponBase>("Prefabs/Weapons/WeaponAxe"));

        AttachWeaponModel();
    }

    public void ChangeWeaponOnDeath()
    {
        if (playerWeaponCur is WeaponHammer || playerWeaponCur is WeaponBlunderbuss)
            playerWeaponCur = Instantiate(Resources.Load<WeaponBase>("Prefabs/Weapons/WeaponAxe"));
        else if (playerWeaponCur is WeaponAxe || playerWeaponCur is WeaponCrossbow)
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

    public bool HasSpareLife()
    {
        return spareLives > 0;
    }

    public void LoseSpareLife()
    {
        if (spareLives > 0)
        {
            spareLives--;
            ResetPlayer();
        }
    }

    public void IncrementEnemiesKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= enemiesUntilLifeRegen)
        {
            RegainLife();
            enemiesKilled = 0; // Reset counter after life is regained
        }
    }

    private void RegainLife()
    {
        spareLives++;
        Debug.Log("Player regained a spare life!");
    }

    public void IncrementWavesCleared()
    {
        wavesCleared++;
        if (wavesCleared >= wavesUntilLifeRegen)
        {
            RegainLife();
            wavesCleared = 0; // Reset counter after life is regenerated
        }
    }

    public void ResetPlayer()
    {
        SetPlayerCurHp(GetPlayerMaxHp());
        playerHpCur = playerHpMax;
    }
}
