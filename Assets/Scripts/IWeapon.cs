public interface IWeapon
{
    WeaponData WeaponData { get;}
    
    void OnEquip(CharacterBase owner);
    void OnUnequip();
}