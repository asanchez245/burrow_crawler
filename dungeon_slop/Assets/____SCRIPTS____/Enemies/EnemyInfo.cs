using Newtonsoft.Json.Bson;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [Header("Character Stats")]
    public string _enemyName { get; private set; }
    public int _maxHealth { get; private set; }
    public int _currentHealth { get; private set; }

    public int _maxShield { get; private set; }
    public int _currentShield { get; private set; }

    public AbilityData[] abilities { get; private set; }
    public int maxAbilitiesPerTurn { get; private set; } = 1;

    public List<AbilityData> GetAbilitiesByPlayType(PlayType playType)
    {
        var filtered = abilities.Where(a => a.playType == playType).ToList();
        if (filtered.Count == 0) return filtered;

        int countToTake = Random.Range(1, maxAbilitiesPerTurn + 1);

        return filtered.OrderBy(a => Random.value)
                       .Take(countToTake)
                       .ToList();
    }
    public int GetMaxHealth() { return _maxHealth; }
    public int GetCurrentHealth() { return _currentHealth; }
    public int GetMaxShield() { return _maxShield; }
    public int GetCurrentShield() { return _currentShield; }

    public void SetMaxHealth(int amount) { _maxHealth = amount; }
    public void SetCurrentHealth(int amount) { _currentHealth = amount; }
    public void SetMaxShield(int amount) { _maxShield = amount; }
    public void SetCurrentShield(int amount) { _currentShield = amount; }

    [Button]
    public void DamageTest()
    {
        _currentHealth--;
    }
    internal void InitializeEnemyInfo(EnemyData enemyData)
    {
        _enemyName = enemyData.enemyName;

        SetCurrentHealth(enemyData.health);
        SetMaxHealth(enemyData.health);

        SetCurrentShield(enemyData.shield);
        SetMaxShield(enemyData.maxShield);

        abilities = enemyData.abilities;
        maxAbilitiesPerTurn = enemyData.maxAbilitiesPerTurn;
    }
}
