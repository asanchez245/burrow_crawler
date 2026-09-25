using UnityEngine;

[CreateAssetMenu(menuName = "Card/Effect/Deal Damage")]
public class DealDamageEffectData : EffectData
{
    public int DamageAmount;
    public bool PlayerIsTarget = false;

    public override Effect CreateRuntimeEffect(ObjectWithEffects owner)
        => new DealDamageEffect(owner, this);
}
