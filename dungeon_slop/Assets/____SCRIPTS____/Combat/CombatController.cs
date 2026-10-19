using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class CombatController : MonoSingleton<CombatController>
{
    CombatInfo _info;
    CombatVisuals _visuals;
    [SerializeField] TutorialController tutorialController;

    public EnemyController[] enemyControllers;
    public Character_Controller[] charactersControllers;
    [SerializeField] CombatData _combatData;

    [SerializeField] Button endTurnButton;

    [Header("Localization")]
    StringTable abilitiesTable;

    #region Localization
    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        LoadTables();
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    void LoadTables()
    {
        abilitiesTable = LocalizationSettings.StringDatabase.GetTable("Abilities");
    }

    void OnLocaleChanged(Locale locale)
    {
        LoadTables();
        //cambiar el texto acutal al del nuevo idioma si eso
    }
    #endregion

    public override void Awake()
    {
        base.Awake();
        InitializeCombat();
    }

    public void InitializeCombat()
    {
        _combatData = GameManager.Instance.nextCombat;
        _info = GetComponent<CombatInfo>();
        if (_info != null) _info.InitializeCombatInfo(_combatData);
        _visuals = GetComponent<CombatVisuals>();
        if (_visuals != null) _visuals.InitializeCombatVisuals(_combatData);

        if (_combatData.isTutorial)
        {
            tutorialController.SetTutorial();
        }
    }
    private void Start()
    {
        BoardController.Instance.InitializeBoard();
        InitializeCharacters();
        InitializeEnemies();
        StartCoroutine(StartCombat());
    }

    IEnumerator StartCombat()
    {
        yield return new WaitForSeconds(1);
        SetRandomEnemyTarget();
        StartCoroutine(BoardController.Instance.GenerateManaTurn());
        StartCoroutine(EnemiesPrepareActions());
    }

    public void InitializeEnemies()
    {
        int i = 0;
        foreach(EnemyData data in _combatData.combatEnemies)
        {
            if (data == null) return;
            Debug.Log("Initializing enemy: " + data.enemyName);
            enemyControllers[i].gameObject.SetActive(true);
            enemyControllers[i].InitializeCharacter(data);
            i++;
        }
    }
    public void InitializeCharacters()
    {
        int i = 0;
        foreach(PartyMember charData in GameManager.Instance.playerParty)
        {
            if (charData == null) return;
            Character_Controller controller = charactersControllers[i];
            controller.gameObject.SetActive(true);
            controller.InitializeCharacter(charData.partyMember);
            BoardController.Instance.AddCharacterToCombat(controller);
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
            GameManager.Instance.playerDied = false;
            GameManager.Instance.playerParty[0].hp = charactersControllers[0].GetCurrentHealth();
            _visuals.ShowWinVisuals(true);
            foreach(EnemyInDungeon enemy in GameManager.Instance.enemiesInDungeon)
            {
                if (enemy.activeInCombat) enemy.defeated = true;
            }
        }
        else
        {
            GameManager.Instance.playerDied = true;
            GameManager.Instance.playerParty[0].hp = charactersControllers[0]._info.GetMaxHealth(); ;
            _visuals.ShowDieVisuals(true);
        }
        StartCoroutine(Scene_Manager.Instance.sceneController.ChangeScene(1, 1.5f));
        Sequencer.Instance.TriggerSequence();
    }




    IEnumerator EnemiesResolveActions()
    {
        foreach (EnemyController enemy in enemyControllers)
        {
            if(enemy!=null && enemy.isActiveAndEnabled)
            {
                if (enemy._info._currentHealth > 0)
                {
                    foreach (var play in enemy.chosenPlayTypes)
                    {
                        enemy.ExecuteAction(play);
                    }
                }
            }
            yield return null;
        }
    }
    IEnumerator EnemiesPrepareActions()
    {
        foreach (EnemyController enemy in enemyControllers)
        {
            if(enemy != null && enemy.gameObject.activeSelf)
            enemy.ChooseAction();
            yield return null;
        }
    }
    public void SetSelectedCharacter(Character_Controller character)
    {
        if (_info._charSelected != null)
            _info._charSelected._visuals.SelectedVisuals(false);
        _info.SetCharacterTargeted(character);
        SetCharacterVisualsInInfo(character.characterData);
        character._visuals.SelectedVisuals(true);
    }

    public void UpdateInfoHealth(int newHealth)
    {
        _visuals.UpdateHealthText(newHealth);
    }
    public void UpdateInfoShield(int newShield)
    {
        _visuals.UpdateShieldText(newShield);
    }

    public Character_Controller GetSelectedCharacter => _info._charSelected;

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
        string localizedAbilityName = abilitiesTable.GetEntry(data.abilityName)?.GetLocalizedString() ?? data.abilityName;
        string localizedAbilityDesc = abilitiesTable.GetEntry(data.description)?.GetLocalizedString() ?? data.description;

        SetInfoName(localizedAbilityName);
        SetInfoDesc(localizedAbilityDesc);
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
