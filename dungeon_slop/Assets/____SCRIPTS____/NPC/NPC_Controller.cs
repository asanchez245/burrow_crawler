using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public NPC_Data npcData;

    NPC_Info npcInfo;
    NPC_Visuals npcVisuals;

    void Initialize()
    {
        npcInfo = GetComponent<NPC_Info>();
        if (npcInfo) npcInfo.Initialize(npcData);
        npcVisuals = GetComponent<NPC_Visuals>();
        if (npcVisuals) npcVisuals.Initialize();
    }
}
