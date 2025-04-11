using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IMeleeWeapon //meleeweapon olabilir ileride çoğunda kullanılacağı için
{
    public WeaponData WeaponData => weaponData;
    
    [SerializeField] private WeaponData weaponData;

    [SerializeField] private Collider _collider;

    [SerializeField] private GameObject _hitVfx;
    
    private CharacterBase _owner;
 
    private CharacterBase _currentTarget;

    public void OnEquip(CharacterBase owner)
    {
        this._owner = owner;
        
        gameObject.SetActive(true);
    }

    public void OpenCollider()
    {
        _collider.enabled = true;
    }
    
    public void CloseCollider()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _owner.gameObject ) return;
        
        if (_currentTarget != null && other.gameObject != _currentTarget.gameObject)
            return;
        
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(weaponData.AttackDamage);
            
            Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);

            ObjectPoolManager.SpawnObject(_hitVfx, hitPoint, Quaternion.identity);
        }
    }

    public void OnUnequip()
    {
        gameObject.SetActive(false);
    }

    public void Cast() { }

    public void SetTarget(CharacterBase target)
    {
        _currentTarget = target;
    }
}