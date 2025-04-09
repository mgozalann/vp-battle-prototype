public interface IWeapon
{
    WeaponData WeaponData { get;}
    
    void OnEquip(CharacterBase owner);
    void OnUnequip();
    void Cast();
    void SetTarget(CharacterBase target);
}