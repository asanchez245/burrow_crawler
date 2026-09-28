using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyVisuals : MonoBehaviour
{
    public Image enemySprite;
    public GameObject targetedIcon;
    internal void InitializeEnemyVisuals(EnemyData enemyData)
    {
        enemySprite.sprite = enemyData.enemySprite;
        targetedIcon.SetActive(false);
    }
    public void SetTargetedVisuals(bool isTargeted)
    {
        targetedIcon.SetActive(isTargeted);
    }

}
