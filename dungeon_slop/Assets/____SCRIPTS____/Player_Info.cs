using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Info : MonoBehaviour
{
    public PlayerStates currentPlayerState { get; private set; }
    public bool startingCombat { get; private set; }
    public void Initialice()
    {
        SetPlayerState(PlayerStates.state_none);
        startingCombat = false;
    }
    public void SetStartingCombat(bool value)
    {
        startingCombat = value;
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
