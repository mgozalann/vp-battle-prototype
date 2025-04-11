using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Combat/Weapon")]
public class WeaponData : ScriptableObject
{
    public GameObject WeaponPrefab;
    
    public float AttackRange = 5f;
    
    public int AttackDamage = 50;

    public float AttackCd;

    public GameObject ProjectilePrefab;
}
