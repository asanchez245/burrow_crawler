using System;
using UnityEngine;

public class CombatInfo : MonoBehaviour
{
    public TurnState _currentTurnState { get; private set; }

    public TurnState GetCurrentTurnState() {  return _currentTurnState; }
    public void SetCurrentTurnState(TurnState state) { _currentTurnState = state; }

    internal void InitializeCombatInfo(CombatData combatData)
    {

    }

}
