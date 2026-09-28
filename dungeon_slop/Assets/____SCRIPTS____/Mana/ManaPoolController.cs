using System.Collections.Generic;
using UnityEngine;

public class ManaPoolController : MonoSingleton<ManaPoolController>
{
    public ManaPoolInfo poolInfo { get; private set; }
    public ManaPoolVisuals visuals { get; private set; }
    public override void Awake()
    {
        base.Awake();
        poolInfo = GetComponent<ManaPoolInfo>();
        poolInfo.Initialize();
        visuals = GetComponent<ManaPoolVisuals>();
        visuals.Initialize();
    }

    public void GenerateMana(OrbType orbType, Vector3 spawnPosition, int count)
    {
        ParticleSystem prefab = visuals.GetParticleSystemForType(orbType);

        if (prefab == null)
        {
            return;
        }

        ParticleSystem instantiatedPS = Instantiate(prefab, spawnPosition, Quaternion.identity);
        ManaParticleEmitter emitter = instantiatedPS.gameObject.AddComponent<ManaParticleEmitter>();
        emitter.Initialize(orbType, count, this);
    }

    public void OnOrbCollected(OrbType type)
    {
        poolInfo.AddMana(type, 1);
        visuals.AddMana(type);
    }
    public bool OnOrbUsed(OrbType type, int amount)
    {
        if (poolInfo.GetCurrentManaCount(type) < amount)
        {
            return false;
        }
        poolInfo.RemoveMana(type, amount);
        visuals.RemoveMana(type);
        return true;
    }
}

[System.Serializable]
public class ManaParticleSystemMapping
{
    public OrbType orbType;
    public ParticleSystem particleSystemPrefab;
}