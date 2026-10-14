using System.Collections.Generic;
using UnityEngine;

public class PlayerInventary_Controller : MonoBehaviour
{
    PlayerInventary_Visuals visuals;

    public List<ItemController> inventory = new List<ItemController>();

    private void Awake()
    {
        visuals = GetComponent<PlayerInventary_Visuals>();
    }

    public void PickUpItem(ItemController item)
    {
        inventory.Add(item);
        visuals.ShowItem(item);
    }
}
