using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterVisuals : MonoBehaviour
{
    [SerializeField] Image characterSpriteCombat;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI shieldText;

    internal void InitializeCharacterVisuals(CharacterData chData, CharacterController controller)
    {
        characterSpriteCombat.sprite = chData.characterSprite;
        UpdateHealthText(controller.GetCurrentHealth());
        UpdateShieldText(controller.GetCurrentShield());
    }

    public void UpdateHealthText(int currentHp)
    {
        healthText.text = $"{currentHp.ToString()}";
    }
    public void UpdateShieldText(int currentShield)
    {
        shieldText.text = $"{currentShield.ToString()}";
    }
}
