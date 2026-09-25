using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    int damageAmount;
    bool playerIsTarget;
    public DealDamageEffect(ObjectWithEffects owner,DealDamageEffectData data):base(owner,data)
    {
        damageAmount = data.DamageAmount;
        playerIsTarget = data.PlayerIsTarget;
    }

    protected override IEnumerator Resolve(EffectContext ctx)
    {
        //if (ctx.enemyInFront != null && !ctx.sourceCard.IsEnemyCard() && !playerIsTarget)
        //    ctx.enemyInFront.EntityGetDamage(damageAmount, ctx.sourceCard);
        //else
        //{
        //    switch(damageType)
        //    {
        //        case DamageType.baseDamage:
        //            ctx.player.ModifyHealth(-damageAmount);
        //            break;
        //        case DamageType.horrorDamage:
        //            ctx.player.ModifyHorror(damageAmount);
        //            break;
        //    }
        //}
        yield return null;
    }
}