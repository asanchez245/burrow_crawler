using DG.Tweening;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    CombatInfo _info;
    CombatVisuals _visuals;

    [SerializeField] CombatData _combatData;

    private void Awake()
    {
        InitializeCombat();
    }

    public void InitializeCombat()
    {
        _info = GetComponent<CombatInfo>();
        if (_info != null) _info.InitializeCombatInfo(_combatData);
        _visuals = new CombatVisuals();
        if (_visuals != null) _visuals.InitializeCombatVisuals(_combatData);



    }

    public void StartCombat()
    {
        //intro combate
        SetPlayerTurn();



        Sequencer.Instance.TriggerSequence();
    }

    public void EndTurn()
    {
        //desactivar control player
        SetEnemyTurn();
        //enemigos añaden sus secuencias de ataque


        Sequencer.Instance.TriggerSequence();
    }


    public void SetPlayerTurn() { _info.SetCurrentTurnState(TurnState.playerTurn); Sequencer.Instance.AddSequence(_visuals.SetTurnVisuals(TurnState.playerTurn)); }
    public void SetEnemyTurn() { _info.SetCurrentTurnState(TurnState.enemyTurn); Sequencer.Instance.AddSequence(_visuals.SetTurnVisuals(TurnState.enemyTurn)); }

    public void EndCombat()
    {
        //outro sequence
        //recompensas combate
        //cambiar escena

        Sequencer.Instance.TriggerSequence();
    }
}
