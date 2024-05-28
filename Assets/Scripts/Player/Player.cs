using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int playerAttack;
    [SerializeField] private int playerHpMax;
    [SerializeField] private WeaponBase playerEeaponCur;

    private int playerHpCur;
    private int playerScore;
}
