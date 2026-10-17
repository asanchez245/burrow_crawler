using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoSingleton<Player_Controller>
{
    public Player_Info playerInfo { get; private set; }
    public Player_Visuals playerVisuals { get; private set; }
    Player_Movement playerMovement;
    public PlayerInventary_Controller playerInventary;

    public Transform playerSpawnPoint01;
    public Transform playerSpawnPoint02;

    public Transform currentSpawnPoint;

    public override void Awake()
    {
        base.Awake();
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

        StartCoroutine(SpawnCharacter(GameManager.Instance.playerDied));
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

    public void ChangeSpawnPosition()
    {
        GameManager.Instance.spawnPointChanged = true;
        currentSpawnPoint = playerSpawnPoint02;
    }
}
