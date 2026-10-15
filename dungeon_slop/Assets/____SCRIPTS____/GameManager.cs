using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Diagnostics;

public class GameManager : MonoSingleton<GameManager>
{
    public Player_Controller playerController;

    public CombatData nextCombat;
    public List<PartyMember> playerParty;

    public Vector3 playerPos;
    public List<EnemyInDungeon> enemiesInDungeon;
    public bool dungeonGenerated = false;

    public bool spawnPointChanged;
    public override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        if (dungeonGenerated) LoadEntites();

    }


    public void EnterCombat(CombatData combatData)
    {
        nextCombat = combatData;
        SaveEntitiesPositions();
        StartCoroutine(Scene_Controller.Instance.ChangeScene(2, 0.5f));
    }
    //public void testshit()
    //{
    //    SaveEntitiesPositions();
    //    LoadEntites();
    //}
    void SaveEntitiesPositions()
    {
        playerPos = playerController.transform.position;

        foreach(EnemyInDungeon enemy in enemiesInDungeon)
        {
            enemy.enemyPosition = enemy.enemyPrefab.transform.position;
            GameObject enemyCopy = enemy.enemyPrefab;
            enemy.enemyPrefab = enemyCopy;
        }
        dungeonGenerated = true;
    }
    void LoadEntites()
    {
        Explore_EnemyController[] oldEnemies = FindObjectsByType<Explore_EnemyController>(FindObjectsSortMode.None);
        foreach(var enemy in oldEnemies)
        {
            if (enemy != null) Destroy(enemy.gameObject);
        }
        foreach(EnemyInDungeon enemy in enemiesInDungeon)
        {
            GameObject newEnemy = Instantiate(enemy.enemyPrefab, enemy.enemyPosition, Quaternion.identity);

            newEnemy.SetActive(true);

            var controller = newEnemy.GetComponent<Explore_EnemyController>();
            var info = newEnemy.GetComponent<Explore_EnemyInfo>();
            var visuals = newEnemy.GetComponent<Explore_EnemyVisuals>();
            var navmesh = newEnemy.GetComponent<NavMeshAgent>();
            if (controller != null) controller.enabled = true;
            if (info != null) info.enabled = true;
            if (visuals != null) visuals.enabled = true;
            if (navmesh != null) navmesh.enabled = true;
        }
    }
}
[System.Serializable]
public class PartyMember
{
    public CharacterData partyMember;
    public int hp;
}
[System.Serializable]
public class EnemyInDungeon
{
    public GameObject enemyPrefab;
    public Vector3 enemyPosition;
}