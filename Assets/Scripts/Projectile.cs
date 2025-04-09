using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    
    private Transform _target;
    
    private WeaponData _weaponData;
    
    private CharacterBase _owner;
    
    [SerializeField] private GameObject hitEffect;

    public void Initialize(CharacterBase owner, Transform target, WeaponData weaponData)
    {
        _owner = owner;
        
        _target = target;
        
        _weaponData = weaponData;
    }

    private void Update()
    {
        if (_target == null) Destroy(gameObject);

        Vector3 dir = (_target.position - transform.position).normalized;
        
        transform.position += dir * (speed * Time.deltaTime);
   
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out IDamageable damageable)) // sur gibi yapılar için
        {
            if (other.TryGetComponent(out CharacterBase character))
            {
                if (character.IsSameTeam(_owner)) return;
            }

            damageable.TakeDamage(_weaponData.AttackDamage);
            
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
            
            // Burada particle efekt çıkartılabilir
            // Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }
}