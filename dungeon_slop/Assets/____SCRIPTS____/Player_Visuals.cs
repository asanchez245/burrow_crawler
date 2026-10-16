using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class Player_Visuals : MonoBehaviour
{
    public SpriteRenderer playerSprite;
    public Animator playerAnimator;
    public Sprite frontSprite;
    public Sprite backSprite;
    public CinemachineCamera playerCam;
    public void Initialice()
    {

    }
    public void FlipSprite(Vector3 dir)
    {
        if (dir.z < 0)
        {
            playerSprite.sprite = frontSprite;
        }
        else if (dir.z > 0)
        {
            playerSprite.sprite = backSprite;
        }
        if (dir.x < 0)
        {
            playerSprite.flipX = playerSprite.sprite == frontSprite? false : true;
        }
        else if (dir.x > 0)
        {
            playerSprite.flipX = playerSprite.sprite == frontSprite ? true : false;
        }
    }
    public void RunAnimation()
    {
        playerAnimator.SetBool("running", true);
    }
    public void IdleAnimation()
    {
        playerAnimator.SetBool("running", false);
    }
    public void EnterCombatCam()
    {
        float targetSize = 5f;
        DOTween.To(() => playerCam.Lens.OrthographicSize, x => playerCam.Lens.OrthographicSize = x, targetSize + 2f, 0.25f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                DOTween.To(() => playerCam.Lens.OrthographicSize, x => playerCam.Lens.OrthographicSize = x, targetSize, 0.2f)
                    .SetEase(Ease.InQuad);
            });
    }
}
