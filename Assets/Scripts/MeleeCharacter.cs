using UnityEngine;

public class MeleeCharacter : CharacterBase
{
    [SerializeField] private float attackRange;
    [SerializeField] private float coolDown;
    [SerializeField] private float damage;
    
    protected override void InitializeAttackStrategy()
    {
        attackStrategy = new MeleeAttack(this, attackRange, coolDown, damage);
    }

}