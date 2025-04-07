using UnityEngine;

public class RangedAttack : IAttackStrategy
{
    private readonly CharacterBase owner;
    public float AttackRange { get; }
    public float Cooldown { get; }
    public float Damage { get; }

    private float lastAttackTime;
    
    private GameObject projectilePrefab;

    public RangedAttack(CharacterBase owner, float range, float cooldown, float damage, GameObject projectilePrefab)
    {
        this.owner = owner;
        
        AttackRange = range;
        
        Cooldown = cooldown;
        
        Damage = damage;
        
        this.projectilePrefab = projectilePrefab;
    }

    public void TryAttack(CharacterBase target)
    {
        if (Time.time - lastAttackTime >= Cooldown)
        {
            lastAttackTime = Time.time;
           
            owner.animator?.SetTrigger("Attack");

            GameObject go = GameObject.Instantiate(projectilePrefab, owner.transform.position + Vector3.up, Quaternion.identity);
           
            Projectile proj = go.GetComponent<Projectile>();
            
            proj.Initialize(target.transform, Damage);
        }
    }
}