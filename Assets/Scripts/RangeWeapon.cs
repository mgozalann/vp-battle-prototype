using UnityEngine;

public class RangeWeapon : MonoBehaviour, IRangeWeapon
{
    
    //arrow, single target staff
    
    public WeaponData WeaponData => weaponData;
    
    [SerializeField] private WeaponData weaponData;

    [SerializeField] private Transform _spawnPoint;

    private CharacterBase _owner;
    
    private CharacterBase _currentTarget;

    public void OnEquip(CharacterBase owner)
    {
        _owner = owner;
        
        gameObject.SetActive(true);
    }

    public void OnUnequip()
    {
        gameObject.SetActive(false);
    }

    public void Cast()
    {
        GameObject go = ObjectPoolManager.SpawnObject(weaponData.ProjectilePrefab, _spawnPoint.position, Quaternion.identity);

        if (go.TryGetComponent(out IProjectile projectile))
        {
            projectile.Initialize(_owner, weaponData, _currentTarget.transform,_spawnPoint);
        }
    }

    public void SetTarget(CharacterBase target)
    {
        _currentTarget = target;
    }
}