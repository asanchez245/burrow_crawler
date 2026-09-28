using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;

    [Image]
    public Sprite characterSprite;
    [Image]
    public Sprite characterPortrait;

    [TextArea(3, 10)]
    public string characterDescription;

    [Header("Character Stats")]
    public int health;
    public int shield;
    [Header("Mana Generation")]
    public List<CharacterManaGeneration> manaGenerationPerTurn = new();

}
[System.Serializable]
public class CharacterManaGeneration
{
    public OrbType manaType;
    public int manaQuantity;
}