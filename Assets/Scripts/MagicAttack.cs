using UnityEngine;

public class MagicAttack : IAttackStrategy
{
    private readonly CharacterBase _owner;
    
    public IWeapon Weapon { get; set; }

    private float lastAttackTime;
    
    private GameObject projectilePrefab;

    public MagicAttack(CharacterBase owner, IWeapon weapon)
    {
        _owner = owner;
        
        Weapon = weapon;

    }

    public void TryAttack(CharacterBase target)
    {
        if (Time.time - lastAttackTime >= Weapon.WeaponData.AttackCd)
        {
            lastAttackTime = Time.time;
           
            _owner.Animator?.SetTrigger("Attack");

            GameObject go = GameObject.Instantiate(projectilePrefab, _owner.transform.position + Vector3.up, Quaternion.identity);
           
            Projectile proj = go.GetComponent<Projectile>();
            
            proj.Initialize(target.transform, Weapon.WeaponData.AttackDamage);
        }
    }
}