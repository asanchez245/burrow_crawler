using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.ParticleSystem;

public class ManaOrbGenerator : MonoBehaviour
{
    public enum OrbType
    {
        NeutralMana,
        YellowMana,
        BlueMana,
        RedMana,
    }

    [System.Serializable]
    public struct ManaParticleSystemMapping
    {
        public OrbType orbType;
        public ParticleSystem particleSystemRef;
    }

    public List<ManaParticleSystemMapping> manaParticleSystems = new List<ManaParticleSystemMapping>();

    public float baseSpeed = 5f;
    public float maxSpeed = 30f;
    public float minDistance = 0.5f;
    public float maxFloatDelay = 1.0f;

    private Dictionary<OrbType, ParticleSystem> particleSystemDict = new Dictionary<OrbType, ParticleSystem>();

    void Start()
    {
        particleSystemDict.Clear();

        foreach (var mapping in manaParticleSystems)
        {
            if (mapping.particleSystemRef != null)
            {
                if (!particleSystemDict.ContainsKey(mapping.orbType))
                {
                    particleSystemDict.Add(mapping.orbType, mapping.particleSystemRef);
                }
                else
                {
                    Debug.LogWarning($"[ManaOrbGenerator] El OrbType '{mapping.orbType}' está repetido en la lista.", this);
                }
            }
            else
            {
                Debug.LogWarning($"[ManaOrbGenerator] Hay un hueco vacío en la lista para el tipo: {mapping.orbType}", this);
            }
        }
    }

    void LateUpdate()
    {
        if (ManaPoolController.Instance == null) return;

        foreach (var kvp in particleSystemDict)
        {
            OrbType currentType = kvp.Key;
            ParticleSystem pSystem = kvp.Value;

            if (pSystem == null) continue;

            int maxParticles = pSystem.main.maxParticles;
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[maxParticles];

            int numParticlesAlive = pSystem.GetParticles(particles);

            for (int i = 0; i < numParticlesAlive; i++)
            {
                float startLifetime = pSystem.main.startLifetime.constant;
                float particleAge = startLifetime - particles[i].remainingLifetime;
                float individualDelay = (particles[i].randomSeed % 100f / 100f) * maxFloatDelay;

                if (particleAge < individualDelay)
                {
                    particles[i].velocity = Vector3.zero;
                    continue;
                }

                Transform targetDestination = ManaPoolController.Instance.GetTargetForType(currentType);

                if (targetDestination == null) continue;

                Vector3 targetPosition = targetDestination.position;
                Vector3 direction = (targetPosition - particles[i].position);
                float distance = direction.magnitude;

                if (distance < minDistance)
                {
                    particles[i].remainingLifetime = 0f;
                    OnOrbCollected(currentType);
                }
                else
                {
                    float currentSpeed = Mathf.Lerp(maxSpeed, baseSpeed, distance / 10f);
                    currentSpeed = Mathf.Clamp(currentSpeed, baseSpeed, maxSpeed);

                    particles[i].position = Vector3.MoveTowards(
                        particles[i].position,
                        targetPosition,
                        currentSpeed * Time.deltaTime
                    );

                    particles[i].velocity = Vector3.zero;
                }
            }

            pSystem.SetParticles(particles, numParticlesAlive);
        }
    }

    public void EmitExactParticles(int count, OrbType orbType)
    {
        if (particleSystemDict.TryGetValue(orbType, out ParticleSystem targetSystem))
        {
            if (targetSystem != null)
            {
                EmitParams emitParams = new EmitParams();
                emitParams.position = transform.position;
                emitParams.applyShapeToPosition = true;

                targetSystem.Emit(emitParams, count);
            }
            else
            {
                Debug.LogError($"[ManaOrbGenerator] El ParticleSystem asignado para {orbType} es nulo (Null).");
            }
        }
        else
        {
            Debug.LogError($"[ManaOrbGenerator] ¡No existe ninguna entrada en la lista del Inspector para el tipo: {orbType}!");
        }
    }

    private void OnOrbCollected(OrbType type)
    {
        // Aquí puedes sumar el recurso real a la reserva compartida
        // ManaPoolController.Instance.AddManaToSharedPool(type, 1);

        switch (type)
        {
            case OrbType.RedMana:
                Debug.Log("¡Maná Rojo añadido a la reserva compartida!");
                break;
            case OrbType.BlueMana:
                Debug.Log("¡Maná Azul añadido a la reserva compartida!");
                break;
            case OrbType.YellowMana:
                Debug.Log("¡Maná Amarillo añadido a la reserva compartida!");
                break;
            case OrbType.NeutralMana:
                Debug.Log("¡Maná Neutral añadido a la reserva compartida!");
                break;
        }
    }
}