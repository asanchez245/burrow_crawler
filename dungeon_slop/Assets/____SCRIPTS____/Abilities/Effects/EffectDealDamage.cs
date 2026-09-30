using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDealDamage : Effect
{
    bool enemySource;
    int damage;

    public EffectDealDamage(ObjectWithEffects owner, EffectDataDealDamage data) : base(owner, data)
    {
        enemySource = data.enemySource;
        damage = data.damage;
    }

    protected override IEnumerator Resolve(EffectContext ctx)
    {
        int totalDamage = damage;
        int targetShield = 0;
        if (enemySource)
        {
            CharacterController target = ctx.characterTargeted;
            targetShield = target.GetCurrentShield();
            if (targetShield > 0)
            {
                target.ModifyCharacterShield(-totalDamage, false);
                totalDamage -= targetShield;
            }
            if (totalDamage > 0)
                target.ModifyCharacterHealth(-totalDamage, false);
        }
        else
        {
            EnemyController target = ctx.enemyTargeted;
            targetShield = target.GetCurrentShield();
            if (targetShield > 0)
            {
                target.ModifyCharacterShield(-totalDamage, false);
                totalDamage -= targetShield;
            }
            if (totalDamage > 0)
                target.ModifyCharacterHealth(-totalDamage, false);
        }
        yield return null;
    }
}
