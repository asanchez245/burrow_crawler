using System;
using DG.Tweening;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class CharacterVisuals : MonoBehaviour
{
    [SerializeField] Image characterSpriteCombat;
    [SerializeField] Animator characterAnimator;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI shieldText;

    internal void InitializeCharacterVisuals(CharacterData chData, CharacterController controller)
    {
        SetCharacterPortrait(chData);
        SetAnimatorController(chData.characterAnimationController);
        UpdateHealthText(controller.GetCurrentHealth());
        UpdateShieldText(controller.GetCurrentShield());
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
    public void SetAnimatorController(AnimatorController animatorController)
    {
        characterAnimator.runtimeAnimatorController = animatorController;
    }
    public void CharGetDamageVisuals(int damageAmount)
    {
        SpawnFloatingText(damageAmount.ToString(), Color.red);
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

    internal void CharacterDieVisuals()
    {
        characterSpriteCombat.DOFade(0, .5f).OnComplete(() => this.gameObject.SetActive(false));
    }

    public void SpawnFloatingText(string text, Color color, float size = 45f, float duration = 0.7f, float offsetY = 40f)
    {
        GameObject textObj = new GameObject("FloatingText_" + text);
        textObj.transform.SetParent(this.transform, false);

        RectTransform rect = textObj.AddComponent<RectTransform>();

        Vector2 initialPos = new Vector2(UnityEngine.Random.Range(-10f, 10f), offsetY);
        rect.anchoredPosition = initialPos;

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.raycastTarget = false;

        rect.DOAnchorPosY(initialPos.y + 60f, duration).SetEase(Ease.OutQuad).SetUpdate(true);
        tmp.DOFade(0f, duration).SetEase(Ease.InQuad).SetUpdate(true).OnComplete(() =>
        {
            textObj.transform.DOKill();
            Destroy(textObj);
        });
    }
}
