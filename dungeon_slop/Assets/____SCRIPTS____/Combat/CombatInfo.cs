using System;
using UnityEngine;

public class CombatInfo : MonoBehaviour
{
    public float enemyActionDelay;
    public TurnState _currentTurnState { get; private set; }
    public TurnState GetCurrentTurnState() {  return _currentTurnState; }
    public EnemyController _enemyTargeted { get; private set; }
    public void SetCurrentTurnState(TurnState state) { _currentTurnState = state; }
    public void SetEnemyTargeted(EnemyController enemy) { _enemyTargeted = enemy; }

    internal void InitializeCombatInfo(CombatData combatData)
    {

    }

}
