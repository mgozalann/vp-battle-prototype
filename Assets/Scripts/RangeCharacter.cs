using UnityEngine;

public class RangeCharacter : CharacterBase
{
    [SerializeField] private float attackRange;
    [SerializeField] private float coolDown;
    [SerializeField] private float damage;
    [SerializeField] private GameObject projectilePrefab;
    
    protected override void InitializeAttackStrategy()
    {
        attackStrategy = new RangedAttack(this, attackRange, coolDown, damage,projectilePrefab);
    }

}