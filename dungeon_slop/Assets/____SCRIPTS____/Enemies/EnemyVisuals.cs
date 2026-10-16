using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class EnemyVisuals : MonoBehaviour
{
    public GameObject enemyVisuals;

    public Image enemySprite;
    public Animator enemyAnimator;
    public GameObject targetedIcon;
    public GameObject attackIntentionIcon;
    public TextMeshProUGUI attackIntentionValue;
    public GameObject healIntentionIcon;
    public TextMeshProUGUI healIntentionValue;
    public GameObject defenseIntentionIcon;
    public TextMeshProUGUI defenseIntentionValue;
    internal void InitializeEnemyVisuals(EnemyData enemyData)
    {
        SetAnimatorController(enemyData.animatorController);
        enemySprite.sprite = enemyData.enemySprite;
        targetedIcon.SetActive(false);
    }
    public void SetTargetedVisuals(bool isTargeted)
    {
        targetedIcon.SetActive(isTargeted);
        if (isTargeted)
        {
            enemySprite.transform.DOScale(1.15f, .2f);
        }
        else
        {
            enemySprite.transform.DOScale(1f, .2f);
        }
    }
    public void SetIntentionIcon(PlayType playType)
    {
        switch (playType)
        {
            case PlayType.Attack:
                attackIntentionIcon.SetActive(true);
                break;
            case PlayType.Heal:
                healIntentionIcon.SetActive(true);
                break;
            case PlayType.Defense:
                defenseIntentionIcon.SetActive(true);
                break;
        }
    }
    public void CleanAllIntentionIcons()
    {
        attackIntentionIcon.SetActive(false);
        attackIntentionValue.alpha = 0;
        healIntentionIcon.SetActive(false);
        healIntentionValue.alpha = 0;
        defenseIntentionIcon.SetActive(false);
        defenseIntentionValue.alpha = 0;
    }
    public IEnumerator TriggerAnim(PlayType playType)
    {
        string animation = playType.ToString();

        if (!string.IsNullOrEmpty(animation))
        {
            enemyAnimator.SetTrigger(animation);
        }

        yield return null;
    }

    public IEnumerator RevealIntention(PlayType playType)
    {
        GameObject actionSelected = null;
        TextMeshProUGUI actionValue = null;

        switch (playType)
        {
            case PlayType.Attack:
                actionSelected = attackIntentionIcon;
                actionValue = attackIntentionValue;
                break;
            case PlayType.Defense:
                actionSelected = defenseIntentionIcon;
                actionValue = defenseIntentionValue;
                break;
            case PlayType.Heal:
                actionSelected = healIntentionIcon;
                actionValue = healIntentionValue;
                break;
            case PlayType.Utility:
                yield break;
        }

        if (actionSelected != null && actionValue != null)
        {
            Sequence revealIntentionTween = DOTween.Sequence();
            revealIntentionTween.Append(actionSelected.transform.DOScaleX(0, .25f));
            revealIntentionTween.Append(actionSelected.transform.DOScaleX(1, .25f));
            revealIntentionTween.Join(actionValue.DOFade(1, .25f).From(0));

            yield return revealIntentionTween.WaitForCompletion();
        }
        CleanAllIntentionIcons();
    }
    public void SetAnimatorController(AnimatorController animatorController)
    {
        enemyAnimator.runtimeAnimatorController = animatorController;
    }
    public void EnemyDieVisuals()
    {
        enemySprite.DOKill();
        enemySprite.DOFade(0, 0.5f).OnComplete(() => enemyVisuals.SetActive(false));
    }

    internal void PointerEnterEnemy(bool enter)
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
