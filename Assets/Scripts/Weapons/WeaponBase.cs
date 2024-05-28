using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] WeaponType weaponType;

    private enum WeaponType
    {
        Melee,
        Ranged
    }
}