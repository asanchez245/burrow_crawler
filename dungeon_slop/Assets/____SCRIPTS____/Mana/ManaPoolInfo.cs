using System.Collections.Generic;
using UnityEngine;

public class ManaPoolInfo : MonoBehaviour
{
    public int neutralManaCount { get; private set; } = 0;
    public int yellowManaCount { get; private set; } = 0;
    public int blueManaCount { get; private set; } = 0;
    public int redManaCount { get; private set; } = 0;

    public void Initialize()
    {
    }
    public void AddMana(ManaType type, int amount)
    {
        switch (type)
        {
            case ManaType.NeutralMana:
                neutralManaCount += amount;
                break;
            case ManaType.YellowMana:
                yellowManaCount += amount;
                break;
            case ManaType.BlueMana:
                blueManaCount += amount;
                break;
            case ManaType.RedMana:
                redManaCount += amount;
                break;
            default:
                Debug.LogWarning($"[ManaPoolInfo] Tipo de maná desconocido: {type}");
                break;
        }
    }
    public void RemoveMana(ManaType type, int amount)
    {
        switch (type)
        {
            case ManaType.NeutralMana:
                neutralManaCount = Mathf.Max(0, neutralManaCount - amount);
                break;
            case ManaType.YellowMana:
                yellowManaCount = Mathf.Max(0, yellowManaCount - amount);
                break;
            case ManaType.BlueMana:
                blueManaCount = Mathf.Max(0, blueManaCount - amount);
                break;
            case ManaType.RedMana:
                redManaCount = Mathf.Max(0, redManaCount - amount);
                break;
            default:
                Debug.LogWarning($"[ManaPoolInfo] Tipo de maná desconocido: {type}");
                break;
        }
    }
    public int GetCurrentManaCount(ManaType type)
    {
        switch (type)
        {
            case ManaType.NeutralMana:
                return neutralManaCount;
            case ManaType.YellowMana:
                return yellowManaCount;
            case ManaType.BlueMana:
                return blueManaCount;
            case ManaType.RedMana:
                return redManaCount;
            default:
                Debug.LogWarning($"[ManaPoolInfo] Tipo de maná desconocido: {type}");
                return 0;
        }
    }
}
