using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public Player_Controller playerController;

    public CombatData nextCombat;
    public List<PartyMember> playerParty;

    public Vector3 playerPos;
    public List<EnemyInDungeon> enemiesInDungeon;
    public bool dungeonGenerated = false;

    public bool spawnPointChanged;

    public bool playerDied;

    public override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            LoadEntites();
            dungeonGenerated = true;
        }
    }
    private void Start()
    {
        SaveEntitiesPositions(null);
    }
    public void EnterCombat(Explore_EnemyInfo enemyHitted)
    {
        nextCombat = enemyHitted.enemyCombatData;
        SaveEntitiesPositions(enemyHitted.gameObject);
        StartCoroutine(Scene_Controller.Instance.ChangeScene(2, 0.5f));
    }
    //public void testshit()
    //{
    //    SaveEntitiesPositions();
    //    LoadEntites();
    //}
    void SaveEntitiesPositions(GameObject enemyToCombat)
    {
        if (playerController == null) return; //Ñapa historica para probar el combate sin la dungeon
        playerPos = playerController.transform.position;

        foreach (EnemyInDungeon enemy in enemiesInDungeon)
        {
            if (enemy.enemyGameObject != null)
                enemy.enemyPosition = enemy.enemyGameObject.transform.position;
            if (enemyToCombat != null)
                enemy.activeInCombat = enemy.enemyGameObject == enemyToCombat;
        }
    }
    void LoadEntites()
    {
        if (!dungeonGenerated) return;
        Explore_EnemyController[] oldEnemies = FindObjectsByType<Explore_EnemyController>(FindObjectsSortMode.None);
        foreach (var enemy in oldEnemies)
        {
            if (enemy != null) Destroy(enemy.gameObject);
        }
        foreach (EnemyInDungeon enemy in enemiesInDungeon)
        {
            if (enemy.defeated)
            {
                enemy.activeInCombat = false;
                continue;
            }
            GameObject newEnemy = Instantiate(enemy.enemyPrefab, enemy.enemyPosition, Quaternion.identity);
            enemy.enemyGameObject = newEnemy;
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
    public GameObject enemyGameObject;
    public Vector3 enemyPosition;
    public bool defeated;
    public bool activeInCombat;
}
