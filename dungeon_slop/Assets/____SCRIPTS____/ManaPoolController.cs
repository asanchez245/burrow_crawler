using UnityEngine;

public class ManaPoolController : MonoBehaviour
{
    public static ManaPoolController Instance;

    [Header("Destinos Globales en la UI/Escena para la Pool")]
    public Transform neutralManaTarget;
    public Transform redManaTarget;
    public Transform blueManaTarget;
    public Transform greenManaTarget;

    [Header("Prefab de la Orbe visual")]
    public GameObject orbPrefab; // El prefab que tiene el script ManaOrb

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Método centralizado para que CUALQUIER aliado genere orbes hacia la pool compartida
    public void SpawnManaOrbs(Vector3 spawnPosition, ManaOrb.OrbType type, int amount)
    {
        Transform destination = GetGlobalTargetForType(type);
        if (destination == null) return;

        for (int i = 0; i < amount; i++)
        {
            // Ligera dispersión aleatoria al nacer desde la posición del aliado
            Vector3 randomOffset = Random.insideUnitSphere * 1.6f;
            GameObject orbObj = Instantiate(orbPrefab, spawnPosition + randomOffset, Quaternion.identity);

            ManaOrb orb = orbObj.GetComponent<ManaOrb>();
            if (orb != null)
            {
                // Retraso escalonado aleatorio para que salgan una a una con fluidez
                float staggeredDelay = Random.Range(0.05f, 0.5f);
                orb.Initialize(destination, type, staggeredDelay);
            }
        }
    }

    Transform GetGlobalTargetForType(ManaOrb.OrbType type)
    {
        switch (type)
        {
            case ManaOrb.OrbType.NeutralMana: return neutralManaTarget;
            case ManaOrb.OrbType.RedMana: return redManaTarget;
            case ManaOrb.OrbType.BlueMana: return blueManaTarget;
            case ManaOrb.OrbType.GreenMana: return greenManaTarget;
            default: return neutralManaTarget;
        }
    }
}