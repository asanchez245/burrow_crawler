using System.Collections.Generic;
using UnityEngine;

public class BoardInfo : MonoBehaviour
{
    public List<Character_Controller> charactersInCombat = new();
    public List<EnemyController> enemiesInCombat = new();

    public List<Transform> characterPositionMap = new();
    public void Initialize()
    {
        foreach (var character in charactersInCombat)
        {
            if (character != null)
            {
                characterPositionMap.Add(character.GetCharacterPos());
            }
        }
    }
    public void AddCharacter(Character_Controller character)
    {
        if (!charactersInCombat.Contains(character))
        {
            charactersInCombat.Add(character);
            characterPositionMap.Add(character.GetCharacterPos());
        }
    }
}
