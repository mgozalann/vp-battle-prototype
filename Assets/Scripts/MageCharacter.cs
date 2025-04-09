public class MageCharacter : CharacterBase
{
    
    protected override void InitializeAttackStrategy()
    {
        attackStrategy = new MagicAttack(this, currentWeapon);
    }
    
    public void OnCast()
    {
        currentWeapon.Cast();
    }
    
}