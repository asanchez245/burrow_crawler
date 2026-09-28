using System.Collections.Generic;
using UnityEngine;

public class ObjectWithEffects : MonoBehaviour
{
    protected ObjectWithEffectsData objectWithEffectsData;
    public List<CardEffectEntryRuntime> cardEffects { get; private set; }

    public virtual void Initialize(ObjectWithEffectsData data)
    {
        objectWithEffectsData = data;
        cardEffects = new List<CardEffectEntryRuntime>();
        foreach (var entry in objectWithEffectsData.effects)
        {
            cardEffects.Add(entry.CreateRuntimeEntry(this));
        }
    }

    public virtual void OnEvent(GameEventType eventType) { }

    protected virtual void TriggerEvent(GameEventType eventType, EffectContext ctxt)
    {
        foreach (CardEffectEntryRuntime effect in cardEffects)
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
        if (cardEffects == null) return;
        if (cardEffects.Count > 0)
        {

            for (int i = cardEffects.Count - 1; i >= 0; i--)
            {
                cardEffects[i].OnDestroy();
            }

        }
        cardEffects.Clear();
        cardEffects = null;
    }
}