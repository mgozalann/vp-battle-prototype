using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour, IProjectile
{
    protected CharacterBase Owner;
    protected WeaponData WeaponData;
    public virtual void Initialize(
        CharacterBase owner, 
        WeaponData weaponData, 
        Transform target, 
        Transform spawnPosition) { }

    protected virtual void ReturnToPool()
    {
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
}