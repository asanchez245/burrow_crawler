using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatInfo : MonoBehaviour
{
    public TurnState _currentTurnState { get; private set; }
    public TurnState GetCurrentTurnState() {  return _currentTurnState; }
    public EnemyController _enemyTargeted { get; private set; }
    public Character_Controller _charSelected { get; private set; }

    [SerializeField] List<AbilityController> _combatInfoAbilities;
    public List<AbilityController> GetCombatInfoAbilities() { return _combatInfoAbilities; }
    public void SetCurrentTurnState(TurnState state) { _currentTurnState = state; }
    public void SetEnemyTargeted(EnemyController enemy) { _enemyTargeted = enemy; }
    public void SetCharacterTargeted(Character_Controller character) { _charSelected = character; }

    internal void InitializeCombatInfo(CombatData combatData)
    {

    }

}
