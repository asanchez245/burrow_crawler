using System.Collections;
using UnityEngine;

public class EffectGetShield : Effect
{
    bool enemySource;
    int shield;

    public EffectGetShield(ObjectWithEffects owner, EffectDataGetShield data) : base(owner, data)
    {
        enemySource = data.enemySource;
        shield = data.shield;
    }

    protected override IEnumerator Resolve(EffectContext ctx)
    {
        int totalShield = shield;

        if (!enemySource) //character se añade asi mismo
        {
            ctx.sourceCharacter.ModifyCharacterShield(totalShield);
        }
        else //enemy se añade a si mismp
        {
            ctx.sourceEnemy.ModifyEnemyShield(totalShield);
        }

        yield return null;
    }
}
