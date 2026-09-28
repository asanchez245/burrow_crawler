using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CombatController : MonoSingleton<CombatController>
{
    CombatInfo _info;
    CombatVisuals _visuals;

    [SerializeField] CombatData _combatData;

    public  override void Awake()
    {
        base.Awake();
        InitializeCombat();
    }

    public void InitializeCombat()
    {
        _info = GetComponent<CombatInfo>();
        if (_info != null) _info.InitializeCombatInfo(_combatData);
        _visuals = GetComponent<CombatVisuals>();
        if (_visuals != null) _visuals.InitializeCombatVisuals(_combatData);
    }
    private void Start()
    {
        BoardController.Instance.Initialize();
        
        StartCombat();
    }

    public void SetCharacterVisualsInInfo(CharacterData chData) { _visuals.SetSelectedCharacterInfo(chData); }

    [Button("End Turn",ButtonSizes.Gigantic),GUIColor(1, 0.2f, 0)]
    public void EndTurn()
    {
        if(TurnState.playerTurn == _info.GetCurrentTurnState())
        {
            SetEnemyTurn();
        }
        else if (TurnState.enemyTurn == _info.GetCurrentTurnState())
        {
            SetPlayerTurn();
        }
        Sequencer.Instance.TriggerSequence();
    }


    public void SetPlayerTurn()
    { 
        _info.SetCurrentTurnState(TurnState.playerTurn);
        Sequencer.Instance.AddSequence(_visuals.SetTurnVisuals(TurnState.playerTurn));
        Sequencer.Instance.AddSequence(BoardController.Instance.GenerateManaTurn());
        // generar mana cada character
        //activar control player
    }
    public void SetEnemyTurn()
    {
        _info.SetCurrentTurnState(TurnState.enemyTurn);
        Sequencer.Instance.AddSequence(_visuals.SetTurnVisuals(TurnState.enemyTurn));
    }
    public void StartCombat()
    {
        //intro combate
        SetCharacterVisualsInInfo(BoardController.Instance.GetCharacter().characterData);
        SetPlayerTurn();
    }
    public void EndCombat()
    {
        //outro sequence
        //recompensas combate
        //cambiar escena

        Sequencer.Instance.TriggerSequence();
    }

    #region Info Desc.
    public TextMeshProUGUI GetInfoName() { return _visuals.infoName; }
    public TextMeshProUGUI GetInfoDesc() { return _visuals.infoDescription; }
    #endregion
}
