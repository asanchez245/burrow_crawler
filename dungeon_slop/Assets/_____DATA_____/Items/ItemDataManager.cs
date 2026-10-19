using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public List<InteractableData> datas;

    [Button("Reset Interactables Data", ButtonSizes.Gigantic), GUIColor(0, 0, 1)]
    public void ResetInteractablesData()
    {
        foreach(var data in datas)
        {
            data.activeInScene = true;
            data.inInventory = false;
        }
    }
}
