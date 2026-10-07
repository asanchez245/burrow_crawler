using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    Player_Info playerInfo;
    Player_Visuals playerVisuals;
    public void Initialice()
    {
        playerInfo = GetComponent<Player_Info>();
        if(playerInfo) playerInfo.Initialice();
        playerVisuals = GetComponent<Player_Visuals>();
        if (playerVisuals) playerVisuals.Initialice();
    }
}
