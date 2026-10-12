using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    Player_Info playerInfo;
    Player_Visuals playerVisuals;
    Player_Movement playerMovement;
    private void Awake()
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
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Enemy") && !playerInfo.startingCombat)
        {
            GameManager.Instance.EnterCombat(hit.gameObject.GetComponent<Explore_EnemyInfo>().enemyCombatData);
            playerInfo.SetStartingCombat(true);
            playerMovement.speed = 0f;
            hit.gameObject.GetComponent<Explore_EnemyInfo>().SetPlayerReached(true);
        }
    }
}
