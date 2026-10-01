using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EnemyVisuals : MonoBehaviour
{
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
        CleanAllIntentionIcons();
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
    public void CleanAllIntentionIcons()
    {
        attackIntentionIcon.SetActive(false);
        healIntentionIcon.SetActive(false);
        deffenseIntentionIcon.SetActive(false);
    }
    public void CleanIntention(PlayType play)
    {
        switch (play)
        {
            case PlayType.Attack:
                attackIntentionIcon.SetActive(false);
                break;
            case PlayType.Heal:
                healIntentionIcon.SetActive(false);
                break;
            case PlayType.Defense:
                deffenseIntentionIcon.SetActive(false);
                break;
        }
    }
    public void EnemyHitVisuals()
    {
        enemySprite.DOKill();
        enemySprite.DOColor(Color.red, 0.1f).From(Color.white).OnComplete(() => enemySprite.DOColor(Color.white, 0.1f));
    }
}
