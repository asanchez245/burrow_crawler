using System.Collections.Generic;
using UnityEngine;

public class BoardInfo : MonoBehaviour
{
    public List<CharacterController> charactersInCombat = new();
    public List<EnemyController> enemiesInCombat = new();

    public List<Transform> characterPositionMap = new();
    public void Initialize()
    {
        foreach (var character in charactersInCombat)
        {
            if (character != null)
            {
                character.InitializeCharacter();
                characterPositionMap.Add(character.GetCharacterPos());
            }
        }
    }
    public void AddCharacter(CharacterController character)
    {
        if (!charactersInCombat.Contains(character))
        {
            charactersInCombat.Add(character);
            characterPositionMap.Add(character.GetCharacterPos());
        }
    }
}
