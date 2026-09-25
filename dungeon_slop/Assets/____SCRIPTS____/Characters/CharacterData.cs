using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;

    [TextArea(3, 10)]
    public string characterDescription;

    [Header("Character Stats")]
    public int health;
    public int shield;


}
