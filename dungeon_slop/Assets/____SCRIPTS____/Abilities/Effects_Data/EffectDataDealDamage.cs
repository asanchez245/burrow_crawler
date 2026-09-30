using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Effect/DealDamage")]
public class EffectDataDealDamage : EffectData
{
    public bool enemySource;
    public int damage;
    public override Effect CreateRuntimeEffect(ObjectWithEffects owner)
        => new EffectDealDamage(owner, this);
}
