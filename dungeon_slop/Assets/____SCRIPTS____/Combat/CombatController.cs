using Sirenix.OdinInspector;
using TMPro;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatController : MonoSingleton<CombatController>
{
    CombatInfo _info;
    CombatVisuals _visuals;


    public EnemyController[] enemyControllers;
    public CharacterController[] charactersControllers;
    [SerializeField] CombatData _combatData;

    [SerializeField] Button endTurnButton;


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
        BoardController.Instance.InitializeBoard();
        InitializeEnemies();
        NextTurn();
    }

    public void InitializeEnemies()
    {
        int i = 0;
        foreach(EnemyData data in _combatData.combatEnemies)
        {
            if (data == null) return;
            enemyControllers[i].gameObject.SetActive(true);
            enemyControllers[i].InitializeCharacter(data);
            i++;
        }
    }

    public void SetEnemyTarget(EnemyController enemy)
    {
        if (_info._enemyTargeted != null)
            _info._enemyTargeted._visuals.SetTargetedVisuals(false);
        _info.SetEnemyTargeted(enemy);
        enemy._visuals.SetTargetedVisuals(true);
    }
    public void SetRandomEnemyTarget()
    {
        List<EnemyController> enemiesInCombat = BoardController.Instance.GetEnemiesInCombat();
        if (enemiesInCombat.Count <= 0) return;
        int randomIndex = UnityEngine.Random.Range(0, enemiesInCombat.Count);
        SetEnemyTarget(enemiesInCombat[randomIndex]);
    }
    public EnemyController GetTargetedEnemy => _info._enemyTargeted;
    public void SetCharacterVisualsInInfo(CharacterData chData) { _visuals.SetSelectedCharacterInfo(chData); }

    public void NextTurn()
    {
        endTurnButton.interactable = false;

        Sequencer.Instance.AddSequence(EnemiesResolveActions());

        Sequencer.Instance.AddSequence(BoardController.Instance.GenerateManaTurn());

        Sequencer.Instance.AddSequence(EnemiesPrepareActions());
        
        Sequencer.Instance.AddSequence(EndTurnDelay());

        Sequencer.Instance.TriggerSequence();
    }

    IEnumerator EndTurnDelay()
    {
        yield return new WaitForSeconds(1f);
        endTurnButton.interactable = true;
    }

    public void EndCombat(bool victory)
    {
        if (victory)
        {
            _visuals.ShowWinVisuals(true);
        }
        else
        {
            _visuals.ShowDieVisuals(true);
        }
        Sequencer.Instance.TriggerSequence();
    }


    IEnumerator EnemiesResolveActions()
    {
        foreach (EnemyController enemy in enemyControllers)
        {
            foreach (var play in enemy.chosenPlayTypes)
            {
                enemy.ExecuteAction(play);
                yield return new WaitForSeconds(_info.enemyActionDelay);
            }
        }
    }
    IEnumerator EnemiesPrepareActions()
    {
        foreach (EnemyController enemy in enemyControllers)
        {
            if(enemy != null && enemy.gameObject.activeSelf)
            enemy.ChooseAction();
            yield return new WaitForSeconds(_info.enemyActionDelay/2);
        }
    }
    public void SetSelectedCharacter(CharacterController character)
    {
        foreach(var ch in BoardController.Instance.GetCharactersInCombat())
        {
            if(character != null)
            {
               ch.selectedCharacter = false;
            }
        }

        character.selectedCharacter = true;
        _info.SetCharacterTargeted(character);
        SetCharacterVisualsInInfo(character.characterData);
    }

    public void UpdateInfoHealth(int newHealth)
    {
        _visuals.UpdateHealthText(newHealth);
    }
    public void UpdateInfoShield(int newShield)
    {
        _visuals.UpdateShieldText(newShield);
    }

    public CharacterController GetSelectedCharacter => _info._charSelected;

    #region Info Desc.
    public TextMeshProUGUI GetInfoName() { return _visuals.infoName; }
    public TextMeshProUGUI GetInfoDesc() { return _visuals.infoDescription; }
    public List<AbilityController> GetCombatInfoAbilities() { return _info.GetCombatInfoAbilities(); }

    public void SetInfoName(string name) { _visuals.infoName.text = name; }
    public void SetInfoDesc(string desc) { _visuals.infoDescription.text = desc; }
    public void SetInfoCost(string cost) { _visuals.infoCost.text += cost; }
    public void ClearInfoCost() { _visuals.infoCost.text = ""; }
    public void SetAbilityInfo (AbilityData data)
    {
        if (data == null) return;
        SetInfoName(data.abilityName);
        SetInfoDesc(data.description);
        foreach (ManaCost cost in data.manaCost)
        {
            if (cost == null) continue;
            SetInfoCost(cost.GetCostString());
        }
    }
    public void CleatAbilityInfo()
    {
        SetInfoName("");
        SetInfoDesc("");
        ClearInfoCost();
    }
    #endregion

    public void CheckCombatEnd(bool victory)
    {
        if (!victory) //character ha muerto
        {
            if(BoardController.Instance.boardInfo.charactersInCombat.Count <= 0)
            {
                EndCombat(false); //pierdes combate
            }
        }
        else //enemy ha muerto
        {
            if (BoardController.Instance.boardInfo.enemiesInCombat.Count <= 0)
            {
                EndCombat(true); //ganas combate
            }
            else //quedan enemigos vivos
            {
                SetRandomEnemyTarget();
            }
        }

    }
}
