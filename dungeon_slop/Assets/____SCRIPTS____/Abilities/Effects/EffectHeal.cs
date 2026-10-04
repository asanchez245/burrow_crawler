using System.Collections;
using System;
using System.Linq;
using UnityEngine;

public class EffectHeal : Effect
{
    bool enemySource;
    bool allAllies;
    int healAmount;

    public EffectHeal(ObjectWithEffects owner, EffectDataHeal data) : base(owner, data)
    {
        enemySource = data.enemySource;
        allAllies = data.allAllies;
        healAmount = data.healAmount;
    }

    protected override IEnumerator Resolve(EffectContext ctx)
    {
        int totalHeal = healAmount;

        if (enemySource)
        {
            if(allAllies)
            {
                var enemies = ctx.enemiesOnBoard;
                foreach (var enemy in enemies)
                {
                    enemy._info.SetCurrentHealth(Math.Min(enemy._info.GetCurrentHealth() + totalHeal, enemy._info.GetMaxHealth()));
                }
            }
            else
            {
                ctx.sourceEnemy._info.SetCurrentHealth
                    (Math.Min(ctx.sourceEnemy._info.GetCurrentHealth() + totalHeal, ctx.sourceEnemy._info.GetMaxHealth()));
            }
        }
        else
        {
            if(allAllies)
            {
                var allies = ctx.alliesOnBoard;
                foreach (var ally in allies)
                {
                    ally._info.SetCurrentHealth(Math.Min(ally._info.GetCurrentHealth() + totalHeal, ally._info.GetMaxHealth()));
                }
            }
            else
            {
                ctx.sourceCharacter._info.SetCurrentHealth(Math.Min(ctx.characterTargeted._info.GetCurrentHealth() + totalHeal, ctx.characterTargeted._info.GetMaxHealth()));
            }
        }

        yield return null;
    }
}
