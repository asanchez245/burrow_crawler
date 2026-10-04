using DG.Tweening;
using System.Collections;
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
    public GameObject healIntentionIcon;
    public GameObject deffenseIntentionIcon;
    internal void InitializeEnemyVisuals(EnemyData enemyData)
    {
        enemySprite.sprite = enemyData.enemySprite;
        targetedIcon.SetActive(false);
        SetAnimatorController(enemyData.animatorController);
    }
    public void SetTargetedVisuals(bool isTargeted)
    {
        targetedIcon.SetActive(isTargeted);
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
                deffenseIntentionIcon.SetActive(true);
                break;
        }
    }
    public IEnumerator CleanAllIntentionIcons()
    {
        attackIntentionIcon.SetActive(false);
        healIntentionIcon.SetActive(false);
        deffenseIntentionIcon.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }
    public void TriggerAnim(PlayType playType)
    {
        switch (playType)
        {
            case PlayType.Attack:
                enemyAnimator.SetTrigger("Attack");
                break;
            case PlayType.Defense:
                enemyAnimator.SetTrigger("Defense");
                break;
            case PlayType.Heal:
                enemyAnimator.SetTrigger("Heal");
                break;
            case PlayType.Utility:
                enemyAnimator.SetTrigger("Utility");
                break;
        }
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
}
