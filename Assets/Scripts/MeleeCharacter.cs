
public class MeleeCharacter : CharacterBase
{

    protected override void InitializeAttackStrategy()
    {
        attackStrategy = new MeleeAttack(this, currentWeapon);
    }

    public void OnAttackStarted()
    {
        if (currentWeapon is IMeleeWeapon meleeWeapon)
        {
            meleeWeapon.OpenCollider();
        }
    }

    public void OnAttackEnded()
    {
        if (currentWeapon is IMeleeWeapon meleeWeapon)
        {
            meleeWeapon.CloseCollider();
        }
    }
    
}

