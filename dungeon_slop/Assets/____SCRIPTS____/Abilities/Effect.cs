using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public ObjectWithEffects owner;
    public EffectData data;
    public GameEventType triggerEvent;
    public List<Condition> conditions;

    public Effect(ObjectWithEffects owner, EffectData data)
    {
        this.owner = owner;
        this.data = data;
    }

    public void OnEvent(EffectContext ctxt)
    {
        if(CanTrigger(ctxt))
            Sequencer.Instance.InsertSequence(Resolve(ctxt));
    }

    bool CanTrigger(EffectContext context)
    {
        foreach (var c in conditions)
            if (!c.IsMet(context))
                return false;

        return true;
    }

    protected abstract IEnumerator Resolve(EffectContext context);

    public void OnDestroy()
    {
        // Clean up references to avoid memory allocation
        conditions.Clear();
        conditions = null;
    }
}
