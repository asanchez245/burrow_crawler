using System;
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

    public void QuitItem(ItemController item)
    {
        inventory.Remove(item);
        visuals.HideItem(item);
    }

    internal bool CheckItemRequired(ItemController requiredItem)
    {
        foreach (ItemController item in inventory)
        {
            if(item == requiredItem)
            {
                QuitItem(item);
                return true;
            }
        }
        return false;
    }
}
