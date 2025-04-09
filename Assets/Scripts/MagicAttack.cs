using UnityEngine;

public class MagicAttack : IAttackStrategy
{
    private readonly CharacterBase _owner;
    
    private readonly IWeapon _weapon;

    private float _lastAttackTime;
    
    public MagicAttack(CharacterBase owner, IWeapon weapon)
    {
        _owner = owner;
        
        _weapon = weapon;

    }

    public void TryAttack(CharacterBase target)
    {
        if (Time.time - _lastAttackTime >= _weapon.WeaponData.AttackCd)
        {
            _lastAttackTime = Time.time;
           
            _owner.Animator?.SetTrigger("Attack");
        }
    }
}