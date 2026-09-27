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
    private void Start()
    {
        foreach (var mapping in visuals.manaParticleSystems)
        {
            poolInfo.SetParticleSystemPrefab(mapping.orbType, mapping.particleSystemPrefab);
        }
    }

    public void GenerateMana(OrbType orbType, Vector3 spawnPosition, int count)
    {
        if (!poolInfo.prefabDict.TryGetValue(orbType, out ParticleSystem prefab) || prefab == null)
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
        visuals.UpdateManaVisuals(type, poolInfo.GetCurrentManaCount(type));
    }
    public bool OnOrbUsed(OrbType type, int amount)
    {
        if (poolInfo.GetCurrentManaCount(type) < amount)
        {
            return false;
        }
        poolInfo.RemoveMana(type, amount);
        visuals.UpdateManaVisuals(type, poolInfo.GetCurrentManaCount(type));
        return true;
    }
    public Transform GetTargetForType(OrbType type)
    {
        switch (type)
        {
            case OrbType.NeutralMana: return visuals.neutralManaTarget;
            case OrbType.YellowMana: return visuals.yellowManaTarget;
            case OrbType.BlueMana: return visuals.blueManaTarget;
            case OrbType.RedMana: return visuals.redManaTarget;
            default: return visuals.neutralManaTarget;
        }
    }
}

[System.Serializable]
public class ManaParticleSystemMapping
{
    public OrbType orbType;
    public ParticleSystem particleSystemPrefab;
}