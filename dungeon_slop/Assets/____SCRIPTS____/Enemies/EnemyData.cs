using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ObjectWithEffectsData
{
    public string enemyName;

    [Image]
    public Sprite enemySprite;

    [Header("Enemy Stats")]
    public int health;
    public int shield;
    public int maxShield;

    public int maxAbilitiesPerTurn = 1;
}
