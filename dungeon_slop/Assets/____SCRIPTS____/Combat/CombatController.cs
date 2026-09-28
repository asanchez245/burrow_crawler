using Sirenix.OdinInspector;
using TMPro;
using System.Collections;
using UnityEngine;

public class CombatController : MonoSingleton<CombatController>
{
    CombatInfo _info;
    CombatVisuals _visuals;
    public EnemyController[] enemies;
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
    public void SetEnemyTarget(EnemyController enemy)
    {
        if (_info._enemyTargeted != null)
            _info._enemyTargeted._visuals.SetTargetedVisuals(false);
        _info.SetEnemyTargeted(enemy);
        enemy._visuals.SetTargetedVisuals(true);
    }
    public void SetCharacterVisualsInInfo(CharacterData chData) { _visuals.SetSelectedCharacterInfo(chData); }

    [Button("End Turn", ButtonSizes.Gigantic), GUIColor(1, 0.2f, 0)]
    public void EndTurn()
    {
        SetEnemyTurn();
        SetPlayerTurn();
        Sequencer.Instance.TriggerSequence();
    }


    public void SetPlayerTurn()
    {
        _info.SetCurrentTurnState(TurnState.playerTurn);
        Sequencer.Instance.AddSequence(_visuals.SetTurnVisuals(TurnState.playerTurn));
        Sequencer.Instance.AddSequence(BoardController.Instance.GenerateManaTurn());
        Sequencer.Instance.AddSequence(EnemiesPrepareActions());
    }
    public void SetEnemyTurn()
    {
        _info.SetCurrentTurnState(TurnState.enemyTurn);
        Sequencer.Instance.AddSequence(_visuals.SetTurnVisuals(TurnState.enemyTurn));
        Sequencer.Instance.AddSequence(EnemiesResolveActions());
    }
    public void StartCombat()
    {
        //intro combate
        SetCharacterVisualsInInfo(BoardController.Instance.GetCharacter().characterData);
        SetPlayerTurn();
        Sequencer.Instance.TriggerSequence();
    }
    public void EndCombat()
    {
        //outro sequence
        //recompensas combate
        //cambiar escena

        Sequencer.Instance.TriggerSequence();
    }

    IEnumerator EnemiesResolveActions()
    {
        foreach (EnemyController enemy in enemies)
        {
            Debug.Log("Enemie" + enemy._info._enemyName + "doing action");
            yield return new WaitForSeconds(_info.enemyActionDelay);
        }
    }
    IEnumerator EnemiesPrepareActions()
    {
        foreach (EnemyController enemy in enemies)
        {
            enemy.ChooseAction();
            yield return new WaitForSeconds(_info.enemyActionDelay);
        }
    }

    #region Info Desc.
    public TextMeshProUGUI GetInfoName() { return _visuals.infoName; }
    public TextMeshProUGUI GetInfoDesc() { return _visuals.infoDescription; }
    #endregion
}
