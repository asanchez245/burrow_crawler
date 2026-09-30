using System;
using DG.Tweening;
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
        SetCharacterPortrait(chData);
        UpdateHealthText(controller.GetCurrentHealth());
        UpdateShieldText(controller.GetCurrentShield());
    }

    public void SetCharacterPortrait(CharacterData chData)
    {
        characterSpriteCombat.sprite = chData.characterSpriteCombatIdle;
    }

    public void UpdateHealthText(int currentHp)
    {
        healthText.text = $"{currentHp.ToString()}";
    }
    public void UpdateShieldText(int currentShield)
    {
        shieldText.text = $"{currentShield.ToString()}";
    }

    public void SelectedCharacterVisuals(bool selected)
    {
        if (selected)
        {

        }
        else
        {

        }
    }

    internal void PointerEnterCharacter(bool enter)
    {
        if (enter)
        {
            this.transform.DOScale(1.05f, .2f);
        }
        else
        {
            this.transform.DOScale(1f, .2f);
        }
    }
}
