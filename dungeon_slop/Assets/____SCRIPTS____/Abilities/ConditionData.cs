using UnityEngine;

public abstract class ConditionData : ScriptableObject
{
    public abstract Condition CreateRuntimeCondition();
}
