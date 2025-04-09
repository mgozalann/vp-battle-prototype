using UnityEngine;

public class RangeWeapon : MonoBehaviour, IRangeWeapon
{
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
        GameObject go = Instantiate(weaponData.ProjectilePrefab, _spawnPoint.position, Quaternion.identity);

        Projectile proj = go.GetComponent<Projectile>();
            
        proj.Initialize(_owner,_currentTarget.transform, weaponData);
    }

    public void SetTarget(CharacterBase target)
    {
        _currentTarget = target;
    }
}