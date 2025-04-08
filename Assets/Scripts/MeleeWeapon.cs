using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IMeleeWeapon //meleeweapon olabilir ileride çoğunda kullanılacağı için
{
    public WeaponData WeaponData => weaponData;
    
    [SerializeField] private WeaponData weaponData;

    [SerializeField] private Collider _collider;
    
    private CharacterBase owner;

    public void OnEquip(CharacterBase owner)
    {
        this.owner = owner;
        
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
        if(other.gameObject == owner.gameObject ) return;
        
        if (other.TryGetComponent(out IDamageable damageable))
        {
            
            damageable.TakeDamage(weaponData.AttackDamage);
            
            Debug.Log("hasar vurdu");
            
            //burada particle çıkacak.
        }
    }

    public void OnUnequip()
    {
        gameObject.SetActive(false);
    }
    
}