using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ObjectWithEffectsData
{
    public string enemyName;

    [Image]
    public Sprite enemySprite;
    public AnimatorController animatorController;

    [Header("Enemy Stats")]
    public int health;
    public int shield;
    public int maxShield;

    public int maxAbilitiesPerTurn = 1;
}
