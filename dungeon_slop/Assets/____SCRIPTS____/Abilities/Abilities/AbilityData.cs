using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/AbilityData")]
public class AbilityData : ObjectWithEffectsData
{
    [Header("________  Abilitiy dataBase  ________")]
    [Space(20)]
    public string abilityName;

    [PreviewField]
    public Sprite artwork;

    [TextArea]
    public string description;
    public ManaCost[] manaCost;

    public bool enemyAbility;

    public PlayType playType;
}
[Serializable]
public class ManaCost
{
    public int amount;
    public ManaType manaType;
    public ManaCost(int _cost, ManaType _manaType) { amount = _cost; manaType = _manaType; }
    public String GetCostString() 
    {
        switch(manaType)
        {
            case ManaType.NeutralMana:
                return $"<style=Neutral>{amount}</style>";
            case ManaType.RedMana:
                return $"<style=Red>{amount}</style>";
            case ManaType.BlueMana:
                return $"<style=Blue>{amount}</style>";
            case ManaType.YellowMana:
                return $"<style=Yellow>{amount}</style>";
        }
        return "";
    }
}