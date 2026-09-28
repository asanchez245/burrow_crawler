using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public ObjectWithEffects owner;
    public EffectData data;

    public Effect(ObjectWithEffects owner, EffectData data)
    {
        this.owner = owner;
        this.data = data;
    }

    public void Execute(EffectContext ctxt)
    {
        Sequencer.Instance.InsertSequence(Resolve(ctxt));
    }

    protected abstract IEnumerator Resolve(EffectContext context);

    public void OnDestroy()
    {
        owner = null;
        data = null;
    }
}
