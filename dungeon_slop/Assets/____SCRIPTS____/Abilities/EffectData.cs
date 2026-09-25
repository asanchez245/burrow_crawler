using UnityEngine;
public abstract class EffectData : ScriptableObject
{
    public abstract Effect CreateRuntimeEffect(ObjectWithEffects owner);
}