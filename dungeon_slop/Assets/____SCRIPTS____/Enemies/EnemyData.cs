using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ObjectWithEffectsData
{
    public string enemyName;

    [Image]
    public Sprite enemySprite;
    public Animator animatorController;

    [Header("Enemy Abilities Amounts")]
    public int damageAmount;
    public int healingAmount;
    public int shieldingAmount;

    [Header("Enemy Stats")]
    public int health;
    public int shield;
    public int maxShield;

    public int maxAbilitiesPerTurn = 1;
}
