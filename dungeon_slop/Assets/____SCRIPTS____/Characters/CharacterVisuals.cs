using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterVisuals : MonoBehaviour
{
    [SerializeField] Image characterSpriteCombat;
    [SerializeField] Animator characterAnimator;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI shieldText;
    FloatingTextGenerator textGenerator;
    internal void InitializeCharacterVisuals(CharacterData chData, Character_Controller controller)
    {
        SetCharacterPortrait(chData);
        //SetAnimatorController(chData.characterAnimationController);
        UpdateHealthText(controller.GetCurrentHealth());
        UpdateShieldText(controller.GetCurrentShield());
        textGenerator = GetComponent<FloatingTextGenerator>();
    }
    public void TriggerAnim(PlayType playType)
    {
        switch(playType)
        {
            case PlayType.Attack:
                characterAnimator.SetTrigger("Attack");
                break;
            case PlayType.Defense:
                characterAnimator.SetTrigger("Defense");
                break;
            case PlayType.Heal:
                characterAnimator.SetTrigger("Heal");
                break;
            case PlayType.Utility:
                characterAnimator.SetTrigger("Utility");
                break;
        }
    }
    public void SetCharacterPortrait(CharacterData chData)
    {
        characterSpriteCombat.sprite = chData.characterSpriteCombatIdle;
    }
    public void SetAnimatorController(Animator animatorController)
    {
        //characterAnimator = animatorController;
    }
    public void CharGetDamageVisuals(int damageAmount)
    {
        textGenerator.SpawnFloatingText(damageAmount, PlayType.Attack);
        characterAnimator.SetTrigger("Hurt");
    }
    public void UpdateHealthText(int currentHp)
    {
        healthText.DOKill();

        healthText.color = Color.red;
        healthText.text = currentHp.ToString();
        healthText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 1, 1).OnComplete(() =>
        {
            healthText.transform.localScale = Vector3.one;
            healthText.DOColor(Color.white, 0.2f);
        });
    }
    public void UpdateShieldText(int currentShield)
    {
        shieldText.DOKill();
        shieldText.color = Color.red;
        shieldText.text = currentShield.ToString();
        shieldText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 1, 1).OnComplete(() =>
        {
            shieldText.transform.localScale = Vector3.one;
            shieldText.DOColor(Color.white, 0.2f);
        });
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

    internal void CharacterDieVisuals()
    {
        characterSpriteCombat.DOFade(0, .5f).OnComplete(() => this.gameObject.SetActive(false));
    }


    internal void SelectedVisuals(bool isSelected)
    {
        if (isSelected)
        {
            characterSpriteCombat.transform.DOScale(1.1f, .2f);
        }
        else
        {
            characterSpriteCombat.transform.DOScale(1f, .2f);
        }
    }
    
}
