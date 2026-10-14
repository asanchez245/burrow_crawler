using UnityEngine;
using UnityEngine.AI;

public class Explore_EnemyInfo : MonoBehaviour
{
    public CombatData enemyCombatData;
    public NavMeshAgent agent { get; private set; }
    public GameObject actualTarget { get; private set; }
    public bool playerReached { get; private set; }
    public bool isPlayerOutOfRange { get; private set; }
    public float loseAgroTimer;
    public float loseAgroDelay = 2f;

    public Vector3 initialPosition { get; private set; }

    public float timeBetweenPatrols = 5f;
    public float patrolRadius = 10f;
    public float patrolTimer;
    public void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        patrolTimer = timeBetweenPatrols;
        initialPosition = transform.position;
    }
    public void SetActualTarget(GameObject target)
    {
        actualTarget = target;
    }
    public void SetPlayerReached(bool value)
    {
        playerReached = value;
    }
    public void SetIsPlayerOutOfRange(bool value)
    {
        isPlayerOutOfRange = value;
    }
    public void SetLosePlayerTimer(float timer)
    {
        loseAgroTimer = timer;
    }
}
