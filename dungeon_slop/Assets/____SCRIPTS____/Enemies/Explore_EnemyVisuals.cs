using UnityEngine;

public class Explore_EnemyVisuals : MonoBehaviour
{
    public SpriteRenderer enemySpriteRenderer;
    Quaternion spriteRotation;
    public void Initialize()
    {
        spriteRotation = enemySpriteRenderer.transform.rotation;
    }
    private void Update()
    {
        enemySpriteRenderer.transform.rotation = spriteRotation;
    }
}
