using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Info : MonoBehaviour
{
    public PlayerStates currentPlayerState { get; private set; }
    
    public void Initialice()
    {
        SetPlayerState(PlayerStates.state_none);
    }

    public void SetPlayerState(PlayerStates newState)
    {
        currentPlayerState = newState;
    }
    public PlayerStates GetPlayerState()
    {
        return currentPlayerState;
    }
}
