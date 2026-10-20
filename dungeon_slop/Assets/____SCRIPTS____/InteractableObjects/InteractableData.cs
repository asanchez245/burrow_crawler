using UnityEngine;

[CreateAssetMenu(fileName = "InteractableData", menuName = "Scriptable Objects/InteractableData")]
public class InteractableData : ScriptableObject
{
    public string itemID;
    public bool activeInScene = true;
    public bool inInventory = false;
}
