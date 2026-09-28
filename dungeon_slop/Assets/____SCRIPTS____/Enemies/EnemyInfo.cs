using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [Header("Character Stats")]
    public int _maxHealth { get; private set; }
    public int _currentHealth { get; private set; }

    public int _maxShield { get; private set; }
    public int _currentShield { get; private set; }

    public int GetMaxHealth() { return _maxHealth; }
    public int GetCurrentHealth() { return _currentHealth; }
    public int GetMaxShield() { return _maxShield; }
    public int GetCurrentShield() { return _currentShield; }

    public void SetMaxHealth(int amount) { _maxHealth = amount; }
    public void SetCurrentHealth(int amount) { _currentHealth = amount; }
    public void SetMaxShield(int amount) { _maxShield = amount; }
    public void SetCurrentShield(int amount) { _currentShield = amount; }



    internal void InitializeEnemyInfo(EnemyData enemyData)
    {
        SetMaxHealth(enemyData.health);
        SetMaxShield(enemyData.shield);
    }
}
