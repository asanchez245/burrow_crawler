using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    UnityEngine.CharacterController controller;
    Player_Visuals visuals;
    Vector3 playerVelocity;
    bool isGrounded;
    public float speed = 5.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;
    private void Awake()
    {
        controller = GetComponent<UnityEngine.CharacterController>();
        visuals = GetComponent<Player_Visuals>();
    }
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        if (moveDirection != Vector3.zero)
        {
            visuals.RunAnimation();
        }
        else
        {
            visuals.IdleAnimation();
        }
        visuals.FlipSprite(moveDirection);

        controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime * speed);
        //playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
