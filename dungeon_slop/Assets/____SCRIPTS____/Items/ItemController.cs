using UnityEngine;

public class ItemController : MonoBehaviour
{
    public ItemVisuals visuals;

    private void Awake()
    {
        visuals = GetComponent<ItemVisuals>();
    }


}
