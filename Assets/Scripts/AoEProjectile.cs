using UnityEngine;

public class AOEProjectile : ProjectileBase //one hit not continuous
{
    private Transform _target;

    [SerializeField] private float _duration;
    
    [SerializeField] private float hitRadius;

    [SerializeField] private GameObject _chargingEffect;
    
    private float _elapsedTime = 0f;

    public override void Initialize(CharacterBase owner, WeaponData weaponData, Transform target, Transform spawnPoint)
    {
        Owner = owner;
        
        WeaponData = weaponData;

        _target = target;
        
        ObjectPoolManager.SpawnObject(_chargingEffect, Owner.transform.position, _chargingEffect.transform.rotation);
    }


    private void Update()
    {

        _elapsedTime += Time.deltaTime;
        
        if (_elapsedTime >= _duration)
        {
            _elapsedTime = 0;
            
            ApplyDamage();
        }
        
        if (_target != null)
        {
            transform.position = _target.position;
        }
    }

    private void ApplyDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, hitRadius);
        
        foreach (var col in hits)
        {
            if (col.TryGetComponent(out IDamageable damageable))
            {
                if (col.TryGetComponent(out CharacterBase character) && character.IsSameTeam(Owner))
                    continue;

                damageable.TakeDamage(WeaponData.AttackDamage);
            }
        }
    }
}