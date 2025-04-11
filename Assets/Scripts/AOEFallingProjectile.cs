using UnityEngine;

public class AOEFallingProjectile : ProjectileBase //one hit not continuous
{
    [SerializeField] private float hitRadius;

    [SerializeField] private float speed = 10f;

    [SerializeField] private GameObject hitAoEffect;

    private float _elapsedTime = 0f;

    private Transform _spawnPoint;

    private Vector3 _direction;

    private Transform _target;

    public override void Initialize(CharacterBase owner, WeaponData weaponData, Transform target, Transform spawnPoint)
    {
        Owner = owner;

        WeaponData = weaponData;

        _target = target;

        _spawnPoint = spawnPoint;

        transform.position = _spawnPoint.position;

        _direction = (_target.position - transform.position).normalized;
    }

    private void Update()
    {
        transform.position += _direction * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);

            ObjectPoolManager.SpawnObject(hitAoEffect, hitPoint, hitAoEffect.transform.rotation);
            
            ApplyDamage();

            ReturnToPool();
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