using System.Collections.Generic;
using UnityEngine;

public class BoardInfo : MonoBehaviour
{
    public List<CharacterController> characters = new();

    public List<Transform> characterPositionMap = new();
    public void Initialize()
    {
        foreach (var character in characters)
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
        if (!characters.Contains(character))
        {
            characters.Add(character);
            characterPositionMap.Add(character.GetCharacterPos());
        }
    }
}
