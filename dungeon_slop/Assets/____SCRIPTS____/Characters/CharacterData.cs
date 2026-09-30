using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;

    [PreviewField]
    public Sprite characterSpriteCombatIdle;
    [PreviewField]
    public Sprite characterPortrait;

    [TextArea(3, 10)]
    public string characterDescription;

    [Header("Character Stats")]
    public int health;
    public int shield;

    [Header("Mana Generation")]
    public List<CharacterManaGeneration> manaGenerationPerTurn = new();

    [Header("_combatInfoAbilities")]
    public List<AbilityData> characterAbilities = new();

}
[System.Serializable]
public class CharacterManaGeneration
{
    public ManaType manaType;
    public int manaQuantity;
}