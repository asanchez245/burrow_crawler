using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class AbilityController : ObjectWithEffects
{
    AbilityInfo abilityInfo;
    AbilityVisuals abilityVisual;

    public AbilityVisuals Visual => abilityVisual;
    public AbilityInfo Info => abilityInfo;
    public AbilityData abilityData => objectWithEffectsData as AbilityData;

    private void Awake()
    {
        //PlayerInfo.OnDeliriumStateChanged += SetDeliriumMode;
    }
    private void OnDisable()
    {
        //PlayerInfo.OnDeliriumStateChanged -= SetDeliriumMode;
    }


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
            //sourceCard = this,
            //leftCards = new(),
            //rightCards = new(),
            //cardsInHand = handcontroller.GetCardsInHand(),
            //cardsUsed = BoardController.Instance.GetUsedCards(),
            //player = PlayerController.Instance,
            //enemyCards = new(),
            //playerCards = new()
        };


        //int thisSlotIndex = -1;

        ////ActionSlot pSlot = playerSlots[thisSlotIndex];

        //bool isCharacterAbility = pSlot.cardInSlot == this;

        //if (isCharacterAbility)
        //{
        //    ctxt.enemyInFront =
        //        BoardController.Instance.GetEnemyAtPosition(thisSlotIndex);
        //}
        //else
        //{
        //    // Enemy or boss card faces the player's card.
        //    ctxt.cardInFront = pSlot.cardInSlot;
        //}
        


        TriggerEvent(eventType, ctxt);
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


}