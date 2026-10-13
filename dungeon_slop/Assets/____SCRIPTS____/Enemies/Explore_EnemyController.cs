using UnityEngine;
using UnityEngine.AI;

public class Explore_EnemyController : MonoBehaviour
{
    Explore_EnemyInfo enemyInfo;
    Explore_EnemyVisuals enemyVisuals;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        enemyInfo = GetComponent<Explore_EnemyInfo>();
        if (enemyInfo) enemyInfo.Initialize();
        enemyVisuals = GetComponent<Explore_EnemyVisuals>();
        if (enemyVisuals) enemyVisuals.Initialize();
        StartPatroll();
    }

    void Update()
    {
        HandleEnemyBehavior();
    }

    void HandleEnemyBehavior()
    {
        if (enemyInfo.playerReached)
        {
            enemyInfo.agent.velocity = Vector3.zero;
            enemyInfo.agent.isStopped = true;
        }
        else
        {
            enemyInfo.agent.isStopped = false;

            if (enemyInfo.isPlayerOutOfRange)
            {
                enemyInfo.loseAgroTimer += Time.deltaTime;
                if (enemyInfo.loseAgroTimer >= enemyInfo.loseAgroDelay)
                {
                    enemyInfo.SetIsPlayerOutOfRange(false);
                    enemyInfo.SetActualTarget(null);
                    StartPatroll();
                }
            }

            if (enemyInfo.actualTarget != null && enemyInfo.actualTarget.CompareTag("Player") && !enemyInfo.isPlayerOutOfRange)
            {
                enemyInfo.agent.destination = enemyInfo.actualTarget.transform.position;
            }
            else if (enemyInfo.actualTarget == null || !enemyInfo.actualTarget.CompareTag("Player"))
            {
                enemyInfo.patrolTimer += Time.deltaTime;

                if (enemyInfo.patrolTimer >= enemyInfo.timeBetweenPatrols)
                {
                    StartPatroll();
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyInfo.SetActualTarget(other.gameObject);
            enemyInfo.SetIsPlayerOutOfRange(false);
            enemyInfo.loseAgroTimer = 0f;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyInfo.SetPlayerReached(false);
            enemyInfo.SetIsPlayerOutOfRange(true);
            enemyInfo.loseAgroTimer = 0f;
        }
    }

    void StartPatroll()
    {
        enemyInfo.patrolTimer = 0f;
        Vector3 randomPoint = GetRandomNavMeshPosition(enemyInfo.initialPosition, enemyInfo.patrolRadius);
        enemyInfo.agent.destination = randomPoint;
    }

    Vector3 GetRandomNavMeshPosition(Vector3 center, float range)
    {
        Vector3 randomDirection = Random.insideUnitSphere * range;
        Vector3 targetPosition = center + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, range, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return center;
    }
}