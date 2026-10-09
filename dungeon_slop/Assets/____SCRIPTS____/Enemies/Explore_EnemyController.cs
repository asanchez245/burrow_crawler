using UnityEngine;

public class Explore_EnemyController : MonoBehaviour
{
    Explore_EnemyInfo enemyInfo;
    Explore_EnemyVisuals enemyVisuals;

    public void Initialize()
    {
        enemyInfo = GetComponent<Explore_EnemyInfo>();
        if (enemyInfo) enemyInfo.Initialize();
        enemyVisuals = GetComponent<Explore_EnemyVisuals>();
        if(enemyVisuals) enemyVisuals.Initialize();
    }
}
