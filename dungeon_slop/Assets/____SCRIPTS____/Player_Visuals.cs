using UnityEngine;

public class Player_Visuals : MonoBehaviour
{
    public SpriteRenderer playerSprite;
    public Animator playerAnimator;
    public void Initialice()
    {

    }
    public void FlipSprite()
    {
        playerSprite.flipX = !playerSprite.flipX;
    }
    public void RunAnimation()
    {
        playerAnimator.SetBool("running", true);
    }
    public void IdleAnimation()
    {
        playerAnimator.SetBool("runing", false);
    }
}
