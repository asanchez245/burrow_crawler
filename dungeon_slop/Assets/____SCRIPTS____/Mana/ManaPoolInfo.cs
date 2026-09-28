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
    public void AddMana(OrbType type, int amount)
    {
        switch (type)
        {
            case OrbType.NeutralMana:
                neutralManaCount += amount;
                break;
            case OrbType.YellowMana:
                yellowManaCount += amount;
                break;
            case OrbType.BlueMana:
                blueManaCount += amount;
                break;
            case OrbType.RedMana:
                redManaCount += amount;
                break;
            default:
                Debug.LogWarning($"[ManaPoolInfo] Tipo de maná desconocido: {type}");
                break;
        }
    }
    public void RemoveMana(OrbType type, int amount)
    {
        switch (type)
        {
            case OrbType.NeutralMana:
                neutralManaCount = Mathf.Max(0, neutralManaCount - amount);
                break;
            case OrbType.YellowMana:
                yellowManaCount = Mathf.Max(0, yellowManaCount - amount);
                break;
            case OrbType.BlueMana:
                blueManaCount = Mathf.Max(0, blueManaCount - amount);
                break;
            case OrbType.RedMana:
                redManaCount = Mathf.Max(0, redManaCount - amount);
                break;
            default:
                Debug.LogWarning($"[ManaPoolInfo] Tipo de maná desconocido: {type}");
                break;
        }
    }
    public int GetCurrentManaCount(OrbType type)
    {
        switch (type)
        {
            case OrbType.NeutralMana:
                return neutralManaCount;
            case OrbType.YellowMana:
                return yellowManaCount;
            case OrbType.BlueMana:
                return blueManaCount;
            case OrbType.RedMana:
                return redManaCount;
            default:
                Debug.LogWarning($"[ManaPoolInfo] Tipo de maná desconocido: {type}");
                return 0;
        }
    }
}
