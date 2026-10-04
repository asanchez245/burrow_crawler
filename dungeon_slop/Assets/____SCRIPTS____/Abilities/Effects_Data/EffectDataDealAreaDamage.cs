using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Effect/Area/DealDamage")]
public class EffectDataDealAreaDamage : EffectData
{
    public bool enemySource;
    public int damage;
    public override Effect CreateRuntimeEffect(ObjectWithEffects owner)
        => new EffectDealAreaDamage(owner, this);
}
