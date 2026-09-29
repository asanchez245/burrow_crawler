using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class AltCharacterController : CharacterController
{
    public bool altCharacter;
    public CharacterData altCharacterData;



    [Button("Sun Mode", ButtonSizes.Gigantic)]
    public void ChangeToAltCharacter()
    {
        if (!altCharacter) return;
        ChangeCharacter(altCharacterData);
    }

    [Button("Moon Mode", ButtonSizes.Gigantic), GUIColor(0, 0, 1)]
    public void ChangeToBaseharacter()
    {
        if (!altCharacter) return;
        ChangeCharacter(characterData);
    }

    public void ChangeCharacter(CharacterData data)
    {
        ChangeAbilities(data);
        ChangePortrait(data);
    }
    public void ChangeAbilities(CharacterData data)
    {
        InitializeCharacterAbilities(data);
    }
}
