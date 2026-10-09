using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    public InputSystem_Actions playerInput;
    public InputSystem_Actions.DungeonActions onDungeon;
    //public PlayerInput.CombatActions attack;

    Player_Movement playerMovement;

    void Awake()
    {
        playerInput = new InputSystem_Actions();
        onDungeon = playerInput.Dungeon;

        playerMovement = GetComponent<Player_Movement>();
        //playerInput.Jump.performed += ctx => playerMovement.Jump();
        //onFoot.Menu.performed += ctx => playerMenu.MenuActivation();
    }

    void FixedUpdate()
    {
        playerMovement.ProcessMove(onDungeon.Move.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onDungeon.Enable();
    }

    private void OnDisable()
    {
        onDungeon.Disable();
    }
}
