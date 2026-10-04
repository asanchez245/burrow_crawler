using UnityEngine;

public class ObjectWithEffectsData : ScriptableObject
{
    public AbilityEffectEntry[] effects;
}

[System.Serializable]
public class AbilityEffectEntry
{
    public EffectData[] effects;
    public ConditionData[] conditions;
    public GameEventType triggerEvent;

    public AbilityEffectEntryRuntime CreateRuntimeEntry(ObjectWithEffects owner)
    {
        AbilityEffectEntryRuntime runtimeEntry = new AbilityEffectEntryRuntime();
        runtimeEntry.effects = new Effect[effects != null ? effects.Length : 0];
        for (int i = 0; i < runtimeEntry.effects.Length; i++)
        {
            runtimeEntry.effects[i] = effects[i].CreateRuntimeEffect(owner);
        }
        runtimeEntry.conditions = new Condition[conditions != null ? conditions.Length : 0];
        for (int i = 0; i < runtimeEntry.conditions.Length; i++)
        {
            runtimeEntry.conditions[i] = conditions[i].CreateRuntimeCondition(owner);
        }
        runtimeEntry.triggerEvent = triggerEvent;
        return runtimeEntry;
    }
}

[System.Serializable]
public class AbilityEffectEntryRuntime
{
    public Effect[] effects;
    public Condition[] conditions;
    public GameEventType triggerEvent;

    public virtual void OnEvent(EffectContext ctxt)
    {
        if (!CanTrigger(ctxt)) return;

        foreach (var effect in effects)
        {
            effect.Execute(ctxt);
        }
    }

    bool CanTrigger(EffectContext ctxt)
    {
        foreach (var condition in conditions)
        {
            if (!condition.IsMet(ctxt))
                return false;
        }

        return true;
    }

    public void OnDestroy()
    {
        if (effects != null)
        {
            for (int i = effects.Length - 1; i >= 0; i--)
            {
                effects[i].OnDestroy();
            }
        }

        effects = null;
        conditions = null;
    }
}
