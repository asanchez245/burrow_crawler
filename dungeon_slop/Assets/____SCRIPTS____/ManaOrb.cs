using UnityEngine;

public class ManaOrb : MonoBehaviour
{
    public enum OrbType { NeutralMana, RedMana, BlueMana, GreenMana }

    [Header("Orb Data")]
    public OrbType orbType;

    [Header("Movement Settings")]
    private Transform targetDestination;
    private float baseSpeed = 6f;
    private float maxSpeed = 35f;
    private float minDistance = 0.4f;
    private float floatDelay = 0.8f;

    private float spawnTime;

    public void Initialize(Transform globalTarget, OrbType type, float delay)
    {
        targetDestination = globalTarget;
        orbType = type;
        floatDelay = delay;
        spawnTime = Time.time;
    }

    void Update()
    {
        if (targetDestination == null) return;

        // Fase 1: Flotar un instante en el sitio al nacer (crea el efecto de "mini burst" ordenado)
        if (Time.time - spawnTime < floatDelay)
        {
            return;
        }

        // Fase 2: Viaje acelerado hacia el contenedor global de la reserva compartida
        float distance = Vector3.Distance(transform.position, targetDestination.position);

        if (distance < minDistance)
        {
            // Al llegar al marcador global de su tipo, sumamos el maná a la pool compartida
            AddToSharedPool();
            Destroy(gameObject); // Se destruye la orbe visual
        }
        else
        {
            // Aceleración tipo imán conforme se acerca al marcador
            float currentSpeed = Mathf.Lerp(maxSpeed, baseSpeed, distance / 10f);
            currentSpeed = Mathf.Clamp(currentSpeed, baseSpeed, maxSpeed);

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetDestination.position,
                currentSpeed * Time.deltaTime
            );
        }
    }

    void AddToSharedPool()
    {
        // Llamas a tu gestor global de recursos compartido
        // Ejemplo:
        // ManaPoolManager.Instance.AddMana(orbType, 1);
        Debug.Log($"¡1 orbe de {orbType} añadida a la reserva compartida del equipo!");
    }
}