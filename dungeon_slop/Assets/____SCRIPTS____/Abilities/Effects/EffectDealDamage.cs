using System;
using System.Collections;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V12;
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

        if (enemySource)
        {
            var characters = CombatController.Instance.charactersControllers;

            int aliveCount = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] != null && characters[i].isActiveAndEnabled && characters[i]._info.GetCurrentHealth() > 0)
                    aliveCount++;
            }

            if (aliveCount > 0)
            {
                int targetIndex = UnityEngine.Random.Range(0, aliveCount);
                CharacterController target = null;

                int currentIndex = 0;
                for (int i = 0; i < characters.Length; i++)
                {
                    if (characters[i] != null && characters[i]._info.GetCurrentHealth() > 0)
                    {
                        if (currentIndex == targetIndex)
                        {
                            target = characters[i];
                            break;
                        }
                        currentIndex++;
                    }
                }

                if (target != null)
                {
                    int shield = target.GetCurrentShield();

                    if (shield > 0)
                    {
                        int damageToShield = Mathf.Min(totalDamage, shield);
                        target.ModifyCharacterShield(-damageToShield, false);
                        totalDamage -= damageToShield;
                    }
                    if (totalDamage > 0)
                    {
                        target.ModifyCharacterHealth(-totalDamage, false);
                    }
                }
            }
        }
        else
        {
            EnemyController target = ctx.enemyTargeted;
            ctx.sourceCharacter._visuals.TriggerAnim(PlayType.Attack);
            if (target != null && target._info.GetCurrentHealth() > 0)
            {
                int shield = target.GetCurrentShield();

                if (shield > 0)
                {
                    int damageToShield = Mathf.Min(totalDamage, shield);
                    target.ModifyEnemyShield(-damageToShield, false);
                    totalDamage -= damageToShield;
                }
                if (totalDamage > 0)
                {
                    target.ModifyEnemyHealth(-totalDamage, false);
                }
            }
        }

        yield return null;
    }
}
