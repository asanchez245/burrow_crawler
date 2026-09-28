using System;
using UnityEngine;
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

        attackIntentionIcon.SetActive(false);
        healIntentionIcon.SetActive(false);
        deffenseIntentionIcon.SetActive(false);
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
}
