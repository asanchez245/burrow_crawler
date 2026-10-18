using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatData", menuName = "Combat/CombatData")]
public class CombatData : ScriptableObject
{
   public List<EnemyData> combatEnemies;

    public bool isTutorial = false;
}
