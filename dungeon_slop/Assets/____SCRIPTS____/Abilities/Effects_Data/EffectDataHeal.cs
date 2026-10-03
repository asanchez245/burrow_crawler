using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Effect/Heal")]
public class EffectDataHeal : EffectData
{
    public bool enemySource;
    public bool allAllies;
    public int healAmount;
    public override Effect CreateRuntimeEffect(ObjectWithEffects owner)
        => new EffectHeal(owner, this);
}
