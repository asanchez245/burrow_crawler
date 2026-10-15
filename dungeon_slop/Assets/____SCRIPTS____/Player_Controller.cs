using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoSingleton<Player_Controller>
{
    Player_Info playerInfo;
    Player_Visuals playerVisuals;
    Player_Movement playerMovement;
    public PlayerInventary_Controller playerInventary;

    public List<Transform> playerSpawnPoints = new();
    public Transform currentSpawnPoint;

    public override void Awake()
    {
        Initialice();
    }
    public void Initialice()
    {
        playerInfo = GetComponent<Player_Info>();
        if(playerInfo) playerInfo.Initialice();
        playerVisuals = GetComponent<Player_Visuals>();
        if (playerVisuals) playerVisuals.Initialice();
        playerMovement = GetComponent<Player_Movement>();
        playerInventary = GetComponent<PlayerInventary_Controller>();
    }

    private void Start()
    {
        GameManager.Instance.playerController = this;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Enemy") && !playerInfo.startingCombat)
        {
            GameManager.Instance.EnterCombat(hit.gameObject.GetComponent<Explore_EnemyInfo>());
            playerInfo.SetStartingCombat(true);
            playerMovement.speed = 0f;
            hit.gameObject.GetComponent<Explore_EnemyInfo>().SetPlayerReached(true);
        }
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
        if (GameManager.Instance.spawnPointChanged)
            ChangeSpawnPosition();

        StartCoroutine(SpawnCharacter());
    }

    public IEnumerator SpawnCharacter(bool respawning = false)
    {    
        Debug.Log("spawneando en " + currentSpawnPoint);
        if (respawning)
        {
            this.transform.position = new Vector3 (currentSpawnPoint.position.x, currentSpawnPoint.position.y, currentSpawnPoint.position.z);
        }
        else
        {
            this.transform.position = GameManager.Instance.playerPos;
        }

            yield return new WaitForSeconds(1.5f);
        playerMovement.ProccessMovement(true);
    }

    

    [Button]
    public void ChangeSpawnPosition()
    {
        GameManager.Instance.spawnPointChanged = true;
        currentSpawnPoint = playerSpawnPoints[1];
    }
}
