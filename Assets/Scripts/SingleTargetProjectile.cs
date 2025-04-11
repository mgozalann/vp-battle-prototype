using UnityEngine;

public class SingleTargetProjectile : ProjectileBase
{
    public float speed = 10f;
    
    private Transform _target;

    private Transform _spawnPoint;
    
    [SerializeField] private GameObject hitEffect;
    
    public override void Initialize(CharacterBase owner, WeaponData weaponData, Transform target, Transform spawnPoint)
    {
        Owner = owner;
        
        WeaponData = weaponData;
        
        _target = target;
        
        _spawnPoint = spawnPoint;
        
        transform.position = _spawnPoint.position;
        
    }

    private void Update()
    {

        if (_target == null)
        {
            ReturnToPool();
            
            return;
        }
        
        Vector3 dir = (_target.position  + Vector3.up * .5f - transform.position).normalized;

        transform.position += dir * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Ground"))
        {
            ReturnToPool();
            
            return;
        }
        
        
        if (other.TryGetComponent(out IDamageable damageable)) // sur gibi yapılar için
        {
            if (other.TryGetComponent(out CharacterBase character))
            {
                if (character.IsSameTeam(Owner)) return;
            }

            damageable.TakeDamage(WeaponData.AttackDamage);
            
            ObjectPoolManager.SpawnObject(hitEffect, transform.position, Quaternion.identity);
            
            ReturnToPool();
            
        }
    }
}