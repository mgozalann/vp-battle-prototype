public interface IAttackStrategy
{
    float AttackRange { get; }
    float Cooldown { get; }
    float Damage { get; }
    void TryAttack(CharacterBase target);
    
    
}