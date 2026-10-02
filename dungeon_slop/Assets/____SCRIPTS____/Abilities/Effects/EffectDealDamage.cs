using System.Collections;
using System;
using System.Linq;
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
                if (characters[i] != null && characters[i]._info.GetCurrentHealth() > 0)
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
                    ApplyDamage(
                        totalDamage,
                        target.GetCurrentShield,
                        shieldDmg => target.ModifyCharacterShield(-shieldDmg, false),
                        healthDmg => target.ModifyCharacterHealth(-healthDmg, false)
                    );
                }
            }
        }
        else
        {
            EnemyController target = ctx.enemyTargeted;

            if (target != null && target._info.GetCurrentHealth() > 0)
            {
                ApplyDamage(
                    totalDamage,
                    target.GetCurrentShield,
                    shieldDmg => target.ModifyEnemyShield(-shieldDmg, false),
                    healthDmg => target.ModifyEnemyHealth(-healthDmg, false)
                );
            }
        }

        yield return null;
    }
    private void ApplyDamage(int amount, Func<int> getShield, Action<int> modifyShield, Action<int> modifyHealth)
    {
        int shield = getShield();

        if (shield > 0)
        {
            int damageToShield = Mathf.Min(amount, shield);
            modifyShield(damageToShield);
            amount -= damageToShield;
        }
        if (amount > 0)
        {
            modifyHealth(amount);
        }
    }
}
