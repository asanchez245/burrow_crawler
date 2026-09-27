using System.Collections.Generic;
using UnityEngine;

public class BoardInfo : MonoBehaviour
{
    public List<CharacterController> characters = new();

    public Dictionary<Transform, CharacterController> characterPositionMap = new();
    public void Initialize()
    {
        foreach (var character in characters)
        {
            if (character != null)
            {
                characterPositionMap.Add(character.transform, character);
            }
        }
    }
    public void AddCharacter(CharacterController character)
    {
        if (!characters.Contains(character))
        {
            characters.Add(character);
            characterPositionMap.Add(character.transform, character);
        }
    }
}
