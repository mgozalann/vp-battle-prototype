public class RangeCharacter : CharacterBase
{
    
    protected override void InitializeAttackStrategy()
    {
        attackStrategy = new MagicAttack(this, currentWeapon);
    }

}