using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Characters/NewCharacter")]
public class CharacterData : ScriptableObject
{
    public string Name;

    public float DetectionRange;

    public int MaxHealth;
}