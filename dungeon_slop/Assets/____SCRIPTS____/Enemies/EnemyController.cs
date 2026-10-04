using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class EnemyController : ObjectWithEffects, IPointerClickHandler
{
    public EnemyInfo _info { get; private set; }
    public EnemyVisuals _visuals { get; private set; }

    public EnemyData enemyData;

    public List<PlayType> chosenPlayTypes = new();

    public void InitializeCharacter(EnemyData data)
    {
        base.Initialize(data);
        _info = GetComponent<EnemyInfo>();
        if (_info != null) _info.InitializeEnemyInfo(data);
        _visuals = GetComponent<EnemyVisuals>();
        if (_visuals != null) _visuals.InitializeEnemyVisuals(data);

        BoardController.Instance.AddEnemyToCombat(this);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        CombatController.Instance.SetEnemyTarget(this);
    }

    public void EnemyDie()
    {
        BoardController.Instance.RemoveEnemyFromCombat(this);
        _visuals.EnemyDieVisuals();
    }

    #region Character Actions Management
    public override void OnEvent(GameEventType eventType)
    {
        EffectContext ctxt = new EffectContext()
        {
            enemyTargeted = CombatController.Instance.GetTargetedEnemy,
            characterTargeted = CombatController.Instance.GetSelectedCharacter,
            poolController = ManaPoolController.Instance,
            enemiesOnBoard = BoardController.Instance.GetEnemiesInCombat(),
            alliesOnBoard = BoardController.Instance.GetCharactersInCombat(),
            sourceCharacter = CombatController.Instance.GetSelectedCharacter,
        };

        TriggerEvent(eventType, ctxt);
    }
    internal void ExecuteAction(PlayType play)
    {
        GameEventType gameEvent = play switch
        {
            PlayType.Attack => GameEventType.OnEnemyAttack,
            PlayType.Defense => GameEventType.OnEnemyDefense,
            PlayType.Heal => GameEventType.OnEnemyHeal,
            _ => throw new System.ArgumentOutOfRangeException(nameof(play), play, null)
        };

        OnEvent(gameEvent);
        Debug.Log($"Enemigo {_info._enemyName} usando acción: {play}");
        _visuals.CleanIntention(play);
    }
    public void ChooseAction()
    {
        int currentHealth = _info.GetCurrentHealth();
        int maxHealth = _info.GetMaxHealth();
        int currentShield = _info.GetCurrentShield();

        float healthPercentage = (float)currentHealth / maxHealth;

        bool canHeal = healthPercentage < 0.95f;
        bool needsShield = currentShield <= 0 && healthPercentage < 0.7f;

        float attackWeight = 1f;
        float healWeight = 0f;
        float defenseWeight = 0f;

        if (canHeal)
        {
            healWeight = Mathf.Lerp(3f, 0.5f, healthPercentage);

            if (healthPercentage < 0.25f)
            {
                attackWeight = 1.5f;
            }
        }

        if (needsShield)
        {
            defenseWeight = 2.0f;
        }

        PlayType chosenPlay = GetWeightedRandomPlay(attackWeight, healWeight, defenseWeight);
        _visuals.SetIntentionIcon(chosenPlay);

        chosenPlayTypes.Clear();
        chosenPlayTypes.Add(chosenPlay);

        Debug.Log($"Acción elegida: {chosenPlay} con un % de vida de {healthPercentage * 100}%");
    }
    PlayType GetWeightedRandomPlay(float attackW, float healW, float shieldW)
    {
        float totalWeight = attackW + healW + shieldW;
        float randomValue = Random.Range(0, totalWeight);

        if (randomValue < attackW)
        {
            return PlayType.Attack;
        }
        else if (randomValue < attackW + healW)
        {
            return PlayType.Heal;
        }
        else
        {
            return PlayType.Defense;
        }
    }

    #endregion
    #region Character Stats Management
    public int GetCurrentHealth() { return _info.GetCurrentHealth(); }
    public void ModifyEnemyHealth(int amount, bool isFixed)
    {
        int healthAmount;
        if(amount < 0)
        {
            _visuals.EnemyHitVisuals();
        }
        if (!isFixed)
        {
            healthAmount = _info.GetCurrentHealth() + amount;
        }
        else
        {
            healthAmount = amount;
        }
        _info.SetCurrentHealth(healthAmount);
        if(_info.GetCurrentHealth() <= 0)
        {
            EnemyDie();
        }
    }
    public void SetMaxHealth(int amount) { _info.SetMaxHealth(amount); }

    public int GetCurrentShield() { return _info.GetCurrentShield(); }
    public void ModifyEnemyShield(int amount, bool isFixed)
    {
        int shieldAmount;
        if (!isFixed)
        {
            shieldAmount = _info.GetCurrentShield() + amount;
        }
        else
        {
            shieldAmount = amount;
        }
        shieldAmount = Mathf.Max(0, shieldAmount);
        _info.SetCurrentShield(shieldAmount);
    }
    public void SetMaxShield(int amount) { _info.SetMaxShield(amount); }

    #endregion
}
