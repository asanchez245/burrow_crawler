using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityController : ObjectWithEffects, IPointerEnterHandler, IPointerExitHandler
{
    AbilityInfo abilityInfo;
    AbilityVisuals abilityVisual;

    public AbilityVisuals Visual => abilityVisual;
    public AbilityInfo Info => abilityInfo;
    public AbilityData abilityData => objectWithEffectsData as AbilityData;

    public override void Initialize(ObjectWithEffectsData data)
    {
        base.Initialize(data);
        abilityInfo = GetComponent<AbilityInfo>();
        if (abilityInfo) abilityInfo.Initialize(abilityData);
        else Debug.LogError($"Trying to initialize the card <b>{gameObject.name}</b> without CardInfo");


        abilityVisual = GetComponent<AbilityVisuals>();
        if (abilityVisual) abilityVisual.Initialize(abilityData);
        else Debug.LogError($"Trying to initialize the card <b>{gameObject.name}</b> without CardVisuals");
    }

    public override void OnEvent(GameEventType eventType)
    {
        EffectContext ctxt = new EffectContext()
        {
            enemyTargeted = CombatController.Instance.GetTargetedEnemy,
            characterTargeted = CombatController.Instance.GetSelectedCharacter,
            poolController = ManaPoolController.Instance
        };

        TriggerEvent(eventType, ctxt);
    }
    public void UseAbility()
    {
        OnEvent(GameEventType.OnTriggerAbility);
    }

    //*********** CARD INFO MANAGEMENT *******************
    public string GetAbilityName() { return abilityInfo._abilityName; }
    public string GetAbilityDesc() { return abilityInfo._description; }
    public int GetCardBaseDamage() { return abilityInfo.GetHealthDamage(); }
    public bool IsCardBaseDamageBuffed() { return abilityInfo._healthDmg < abilityInfo.GetHealthDamage(); }
    public bool IsCardBaseDamageDebuffed() { return abilityInfo._healthDmg > abilityInfo.GetHealthDamage(); }


    public bool IsEnemyCard() { return abilityInfo._enemyAbility; }
    //public bool IsPreviewCard() { return abilityInfo._isPreviewCard; }
    //public bool IsRewardCard() { return abilityInfo._isRewardCard; }
  

    public int GetCost(ManaType manaType)
    {
        switch (manaType)
        {
            case ManaType.NeutralMana:
                return abilityInfo._neutralManaCost;
            case ManaType.YellowMana:
                return abilityInfo._yellowManaCost;
            case ManaType.BlueMana:
                return abilityInfo._blueManaCost;
            case ManaType.RedMana:
                return abilityInfo._redManaCost;
            default:
                return 0;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CombatController.Instance.SetAbilityInfo(abilityData);
        Visual.PointerEnterVisuals();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CombatController.Instance.CleatAbilityInfo();
        Visual.PointerExitVisuals();
    }


}