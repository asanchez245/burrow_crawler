using System;
using System.Collections;
using UnityEngine;

public class CombatVisuals : MonoBehaviour
{
    internal void InitializeCombatVisuals(CombatData combatData)
    {

    }

    internal IEnumerator SetTurnVisuals(TurnState turnState)
    {
        switch (turnState)
        {
            case TurnState.playerTurn:
                return SetPlayerTurnVisuals();

            case TurnState.enemyTurn:
                return SetEnemyTurnVisuals();

            default:
                return null;
        }
    }

    IEnumerator SetPlayerTurnVisuals()
    {
        yield return null;
    }
    IEnumerator SetEnemyTurnVisuals()
    {
        yield return null;

    }
}
