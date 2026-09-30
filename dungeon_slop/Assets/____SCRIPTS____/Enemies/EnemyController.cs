using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour, IPointerClickHandler
{
    public EnemyInfo _info { get; private set; }
    public EnemyVisuals _visuals { get; private set; }

    public EnemyData enemyData;

    public List<PlayType> chosenPlayTypes = new();

 

    private void Awake()
    {
        InitializeCharacter(enemyData);
    }

    public void InitializeCharacter(EnemyData data)
    {
        _info = GetComponent<EnemyInfo>();
        if (_info != null) _info.InitializeEnemyInfo(data);
        _visuals = GetComponent<EnemyVisuals>();
        if (_visuals != null) _visuals.InitializeEnemyVisuals(data);
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
            defenseWeight = 2.0f; // Buen momento para el escudo
        }

        PlayType chosenPlay = GetWeightedRandomPlay(attackWeight, healWeight, defenseWeight);
        _visuals.SetIntentionIcon(chosenPlay);

        chosenPlayTypes.Clear();
        chosenPlayTypes.Add(chosenPlay);

        Debug.Log($"Acción elegida: {chosenPlay} con un % de vida de {healthPercentage * 100}%");
    }

    private PlayType GetWeightedRandomPlay(float attackW, float healW, float shieldW)
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
    #region Character Stats Management
    public void ModifyCharacterHealth(int amount, bool isFixed)
    {
        int healthAmount;
        if (!isFixed)
        {
            healthAmount = _info.GetCurrentHealth() - amount;
        }
        else
        {
            healthAmount = amount;
        }
        _info.SetCurrentHealth(healthAmount);
    }
    public void SetMaxHealth(int amount) { _info.SetMaxHealth(amount); }

    public void ModifyCharacterShield(int amount, bool isFixed)
    {
        int shieldAmount;
        if (!isFixed)
        {
            shieldAmount = _info.GetCurrentShield() - amount;
        }
        else
        {
            shieldAmount = amount;
        }
        _info.SetCurrentShield(shieldAmount);
    }
    public void SetMaxShield(int amount) { _info.SetMaxShield(amount); }

    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        CombatController.Instance.SetEnemyTarget(this);
    }

    internal void ExecuteAction(PlayType play)
    {
        switch (play)
        {
            case PlayType.Attack:
                break;
            case PlayType.Defense:
                break;
            case PlayType.Utility:

                break;

        }
        Debug.Log("Enemie" + _info._enemyName + "doing action: " + play);
        CleanAction(play);
    }

    public void CleanAction(PlayType play)
    {
        _visuals.CleanIntention(play);
    }
}
