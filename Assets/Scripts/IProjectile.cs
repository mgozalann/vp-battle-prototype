using UnityEngine;

public interface IProjectile
{
    void Initialize(CharacterBase owner, WeaponData weaponData, Transform target,Transform spawnPosition);
    
}