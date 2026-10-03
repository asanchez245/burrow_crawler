using System.Collections.Generic;
using UnityEngine;

public class ObjectWithEffects : MonoBehaviour
{
    protected ObjectWithEffectsData objectWithEffectsData;
    public List<AbilityEffectEntryRuntime> abilityEffects { get; private set; }

    public virtual void Initialize(ObjectWithEffectsData data)
    {
        objectWithEffectsData = data;
        abilityEffects = new List<AbilityEffectEntryRuntime>();
        foreach (var entry in objectWithEffectsData.effects)
        {
            abilityEffects.Add(entry.CreateRuntimeEntry(this));
        }
    }

    public virtual void OnEvent(GameEventType eventType) { }

    protected virtual void TriggerEvent(GameEventType eventType, EffectContext ctxt)
    {
        foreach (AbilityEffectEntryRuntime effect in abilityEffects)
        {
            if (effect.triggerEvent == eventType)
                effect.OnEvent(ctxt);
        }
    }

    void OnDestroy()
    {
        DestroyReferences();
    }

    protected virtual void DestroyReferences()
    {
        if (abilityEffects == null) return;
        if (abilityEffects.Count > 0)
        {

            for (int i = abilityEffects.Count - 1; i >= 0; i--)
            {
                abilityEffects[i].OnDestroy();
            }

        }
        abilityEffects.Clear();
        abilityEffects = null;
    }
}