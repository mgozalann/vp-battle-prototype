
public class WarriorCharacter : CharacterBase
{

    protected override void InitializeAttackStrategy()
    {
        AttackStrategy = new MeleeAttack(this, CurrentWeapon);
    }

    public void OnAttackStarted()
    {
        if (CurrentWeapon is IMeleeWeapon meleeWeapon)
        {
            meleeWeapon.OpenCollider();
        }
    }

    public void OnAttackEnded()
    {
        if (CurrentWeapon is IMeleeWeapon meleeWeapon)
        {
            meleeWeapon.CloseCollider();
        }
    }
    
}

