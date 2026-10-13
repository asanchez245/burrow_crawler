using UnityEngine;

public class ConditionPayMana : Condition
{
    ManaCost[] manaCosts;

    public ConditionPayMana(ConditionDataPayMana data)
    {
        manaCosts = data.manaCosts;
    }

    public override bool IsMet(EffectContext ctx)
    {
        int neutralManaCost = 0;
        int redManaCost = 0;
        int yellowManaCost = 0;
        int blueManaCost = 0;
        foreach (ManaCost cost in manaCosts)
        {
            if (ctx.poolController.ManaCount(cost.manaType) >= cost.amount)
            {
                switch (cost.manaType)
                {
                    case ManaType.NeutralMana:
                        neutralManaCost += cost.amount;
                        break;
                    case ManaType.RedMana:
                        redManaCost += cost.amount;
                        break;
                    case ManaType.YellowMana:
                        yellowManaCost += cost.amount;
                        break;
                    case ManaType.BlueMana:
                        blueManaCost += cost.amount;
                        break;

                }
            }
            else return false;
        }

        if (neutralManaCost > 0)
            ctx.poolController.OnOrbUsed(ManaType.NeutralMana, neutralManaCost);

        if (redManaCost > 0)
            ctx.poolController.OnOrbUsed(ManaType.RedMana, redManaCost);

        if (yellowManaCost > 0)
            ctx.poolController.OnOrbUsed(ManaType.YellowMana, yellowManaCost);

        if (blueManaCost > 0)
            ctx.poolController.OnOrbUsed(ManaType.BlueMana, blueManaCost);
        return true;
    }
}
