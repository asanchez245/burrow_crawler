using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectWithEffects : MonoBehaviour
{
    protected ObjectWithEffectsData objectWithEffectsData;
    public List<Effect> effects { get; private set; }

    public virtual void Initialize(ObjectWithEffectsData data)
    {
        objectWithEffectsData = data;
        // Create runtime effects and save references
        effects = new List<Effect>();
        foreach (var entry in objectWithEffectsData.effects)
        {
            var effect = entry.effect.CreateRuntimeEffect(this);

            effect.triggerEvent = entry.triggerEvent;
            effect.conditions = entry.conditions
                .Select(c => c.CreateRuntimeCondition())
                .ToList();

            effects.Add(effect);
        }
    }

    public virtual void OnEvent(GameEventType eventType) { }

    protected virtual void TriggerEvent(GameEventType eventType, EffectContext ctxt)
    {
        foreach (Effect effect in effects)
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
        if (effects == null) return;
        if (effects.Count > 0)
        {

            for (int i = effects.Count - 1; i >= 0; i--)
            {
                effects[i].OnDestroy();
            }

        }
        effects.Clear();
        effects = null;
    }
}
