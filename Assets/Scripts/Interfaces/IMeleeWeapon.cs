public interface IMeleeWeapon : IWeapon
{
    void OpenCollider();  // Melee silahlarında collider açma
    void CloseCollider(); // Melee silahlarında collider kapama
}