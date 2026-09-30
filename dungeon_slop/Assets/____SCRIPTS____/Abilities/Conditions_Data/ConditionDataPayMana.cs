using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Condition/PayMana")]

public class ConditionDataPayMana : ConditionData
{
    public ManaCost[] manaCosts;

    public override Condition CreateRuntimeCondition(ObjectWithEffects owner)
        => new ConditionPayMana(this);
}
