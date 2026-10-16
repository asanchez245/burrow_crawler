using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDealAreaDamage : Effect
{
    bool enemySource;
    int damage;

    public EffectDealAreaDamage(ObjectWithEffects owner, EffectDataDealAreaDamage data) : base(owner, data)
    {
        enemySource = data.enemySource;
        damage = data.damage;
    }

    protected override IEnumerator Resolve(EffectContext ctx)
    {
        int totalDamage = damage;

        if (enemySource) //enemigo ataca en area a todos characters
        {
            var characters = CombatController.Instance.charactersControllers;

            List<CharacterController> targets = new();

            int currentIndex = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] != null && characters[i]._info.GetCurrentHealth() > 0)
                {
                    targets.Add(characters[i]);
                    currentIndex++;
                }
            }

            if (targets != null)
            {
                foreach(CharacterController target in targets)
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
        else //character ataca en area a todos eneigos
        {
            var enemies = CombatController.Instance.enemyControllers;

            List<EnemyController> targets = new();

            int currentIndex = 0;
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null && enemies[i].isActiveAndEnabled && enemies[i]._info.GetCurrentHealth() > 0)
                {
                    targets.Add(enemies[i]);
                    currentIndex++;
                }
            }

            if (targets != null)
            {
                foreach (EnemyController target in targets)
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
            
        }
        yield return null;
    }
}
