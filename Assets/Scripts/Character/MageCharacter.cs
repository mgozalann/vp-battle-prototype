public class MageCharacter : CharacterBase
{
    
    protected override void InitializeAttackStrategy()
    {
        AttackStrategy = new MagicAttack(this, CurrentWeapon);
    }
    
    public void OnCast()
    {
        CurrentWeapon.Cast();
    }
    
}