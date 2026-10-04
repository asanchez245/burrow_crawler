using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Effect/GetShield")]
public class EffectDataGetShield : EffectData
{
    public bool enemySource;
    public int shield;
    public override Effect CreateRuntimeEffect(ObjectWithEffects owner)
        => new EffectGetShield(owner, this);
}
