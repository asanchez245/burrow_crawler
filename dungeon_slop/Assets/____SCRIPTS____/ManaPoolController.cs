using UnityEngine;

public class ManaPoolController : MonoBehaviour
{
    public static ManaPoolController Instance;

    [Header("Contenedores Globales en la UI")]
    public Transform neutralManaTarget;
    public Transform yellowManaTarget;
    public Transform blueManaTarget;
    public Transform redManaTarget;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Método que llama el ManaOrb en cada frame para saber a qué contador volar
    public Transform GetTargetForType(ManaOrbGenerator.OrbType type)
    {
        switch (type)
        {
            case ManaOrbGenerator.OrbType.NeutralMana: return neutralManaTarget;
            case ManaOrbGenerator.OrbType.YellowMana: return yellowManaTarget;
            case ManaOrbGenerator.OrbType.BlueMana: return blueManaTarget;
            case ManaOrbGenerator.OrbType.RedMana: return redManaTarget;
            default: return neutralManaTarget;
        }
    }
}