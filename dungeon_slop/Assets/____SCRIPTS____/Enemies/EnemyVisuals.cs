using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EnemyVisuals : MonoBehaviour
{
    public GameObject enemyVisuals;

    public Image enemySprite;
    public GameObject targetedIcon;
    public GameObject attackIntentionIcon;
    public GameObject healIntentionIcon;
    public GameObject deffenseIntentionIcon;
    internal void InitializeEnemyVisuals(EnemyData enemyData)
    {
        enemySprite.sprite = enemyData.enemySprite;
        targetedIcon.SetActive(false);
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
    public void EnemyHitVisuals()
    {
        enemySprite.DOKill();
        enemySprite.DOColor(Color.red, 0.1f).From(Color.white).OnComplete(() => enemySprite.DOColor(Color.white, 0.1f));
    }

    public void EnemyDieVisuals()
    {
        enemySprite.DOKill();
        enemySprite.DOFade(0, 0.5f).OnComplete(() => enemyVisuals.SetActive(false));
    }
}
