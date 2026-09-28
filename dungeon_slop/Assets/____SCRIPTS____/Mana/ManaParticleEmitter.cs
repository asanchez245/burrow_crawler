using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class ManaParticleEmitter : MonoBehaviour
{
    private ParticleSystem pSystem;
    private OrbType orbType;
    private ManaPoolController controller;
    private bool hasEmitted = false;

    public void Initialize(OrbType type, int count, ManaPoolController poolController)
    {
        pSystem = GetComponent<ParticleSystem>();
        orbType = type;
        controller = poolController;

        // Configuración para asegurar que las partículas se muevan libremente en el espacio del mundo
        var mainModule = pSystem.main;
        mainModule.simulationSpace = ParticleSystemSimulationSpace.World;

        // Emitir exactamente las partículas indicadas
        EmitParams emitParams = new EmitParams
        {
            position = transform.position,
            applyShapeToPosition = true
        };

        pSystem.Emit(emitParams, count);
        hasEmitted = true;
    }

    private void LateUpdate()
    {
        if (!hasEmitted || pSystem == null || controller == null) return;

        int maxParticles = pSystem.main.maxParticles;
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[maxParticles];
        int numParticlesAlive = pSystem.GetParticles(particles);

        // Si ya emitió y no quedan partículas vivas, destruimos la instancia
        if (numParticlesAlive == 0)
        {
            Destroy(gameObject);
            return;
        }

        Transform targetDestination = controller.visuals.orbsTarget;
        if (targetDestination == null) return;

        Vector3 targetPosition = targetDestination.position;
        float startLifetime = pSystem.main.startLifetime.constant;

        for (int i = 0; i < numParticlesAlive; i++)
        {
            float particleAge = startLifetime - particles[i].remainingLifetime;
            float individualDelay = (particles[i].randomSeed % 100f / 100f) * controller.visuals.maxFloatDelay;

            if (particleAge < individualDelay)
            {
                particles[i].velocity = Vector3.zero;
                continue;
            }

            Vector3 direction = (targetPosition - particles[i].position);
            float distance = direction.magnitude;

            if (distance < controller.visuals.minDistance)
            {
                particles[i].remainingLifetime = 0f;
                controller.OnOrbCollected(orbType);
            }
            else
            {
                float currentSpeed = Mathf.Lerp(controller.visuals.maxSpeed, controller.visuals.baseSpeed, distance / 10f);
                currentSpeed = Mathf.Clamp(currentSpeed, controller.visuals.baseSpeed, controller.visuals.maxSpeed);

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