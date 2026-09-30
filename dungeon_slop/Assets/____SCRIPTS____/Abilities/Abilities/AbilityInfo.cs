using DG.Tweening;
using System.Linq;
using UnityEngine;

public class AbilityInfo : MonoBehaviour
{

    public string _abilityName { get; private set; }
    public string _description { get; private set; }

    public int _healthDmg { get; private set; }
    public int _healthDmgAdd { get; private set; }
    public int _healthDmgMult { get; private set; } = 1;
    public float _healthDmgReduction { get; private set; } = 1;

    public int _neutralManaCost { get; private set; }
    public int _yellowManaCost { get; private set; }
    public int _blueManaCost { get; private set; }
    public int _redManaCost { get; private set; }
    
    public bool _enemyAbility { get; private set; }

    //public bool _isPreviewCard;
    //public bool _isRewardCard;

    public GameObject _cardOwner { get; private set;  }
    public void Initialize(AbilityData data)
    {
        _abilityName = data.abilityName;
        _description = data.description;
        _enemyAbility = data.enemyAbility;

        foreach(CardEffectEntry effectEntry in data.effects)
        {
            foreach (EffectData effect in effectEntry.effects)
            {
                if (effect is EffectDataDealDamage
                && effectEntry.conditions.Count() == 0)
                {
                    EffectDataDealDamage damageEffectData = effect as EffectDataDealDamage;         
                    _healthDmg += damageEffectData.damage;                 
                }
            }
        }
        foreach (ManaCost cost in data.manaCost) SetCost(cost);
    }
    public void SetCost(ManaCost cost)
    {
        switch (cost.manaType)
        {
            case ManaType.NeutralMana:
                _neutralManaCost += cost.amount;
                break;
            case ManaType.YellowMana:
                _yellowManaCost += cost.amount;
                break;
            case ManaType.BlueMana:
                _blueManaCost += cost.amount;
                break;
            case ManaType.RedMana:
                _redManaCost += cost.amount;
                break;
        }
    }
    
    public void SetHealthDamageAdd(int value){_healthDmgAdd += value;}
    public void SetHealthDamageMult(int value){_healthDmgMult += value;}
    public void SetHealthDamageReduction(float value) { _healthDmgReduction += value; }
    public int GetHealthDamage() { return (int)(((_healthDmg + _healthDmgAdd) * _healthDmgMult) * _healthDmgReduction); }
}