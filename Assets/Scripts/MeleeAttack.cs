using UnityEngine;

public class MeleeAttack : IAttackStrategy
{
    private readonly CharacterBase owner;

    public float AttackRange { get; }
    public float Cooldown { get; }
    public float Damage { get; }

    private float lastAttackTime;
    public MeleeAttack(CharacterBase owner, float range, float cooldown, float damage)
    {
        this.owner = owner;
        
        AttackRange = range;
        
        Cooldown = cooldown;
        
        Damage = damage;
    }

    

    public void TryAttack(CharacterBase target)
    {
        if (Time.time - lastAttackTime >= Cooldown)
        {
            lastAttackTime = Time.time;
            owner.animator?.SetTrigger("Attack");
            
            Debug.Log("saldırdı");
            
            target.TakeDamage(Damage);
        }
    }
}