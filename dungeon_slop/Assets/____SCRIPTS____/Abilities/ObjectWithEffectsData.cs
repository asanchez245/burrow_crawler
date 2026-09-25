using UnityEngine;

public class ObjectWithEffectsData : ScriptableObject
{
    public CardEffectEntry[] effects;

    [System.Serializable]
    public class CardEffectEntry
    {
        public EffectData effect;
        public ConditionData[] conditions;
        public GameEventType triggerEvent;
    }
}
