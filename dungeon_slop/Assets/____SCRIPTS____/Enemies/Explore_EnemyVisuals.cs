using UnityEngine;

public class Explore_EnemyVisuals : MonoBehaviour
{
    public SpriteRenderer enemySpriteRenderer;
    public Animator enemyAnimator;
    Quaternion spriteRotation;
    public void Initialize()
    {
        spriteRotation = enemySpriteRenderer.transform.rotation;
    }
    private void Update()
    {
        enemySpriteRenderer.transform.rotation = spriteRotation;
    }
    public void EnemyRunAnimation()
    {
        enemyAnimator.SetBool("running", true);
    }
    public void EnemyIdleAnimation()
    {
        enemyAnimator.SetBool("running", false);
    }
}
