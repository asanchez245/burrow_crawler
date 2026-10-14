using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Character/NPC")]
public class NPC_Data : ScriptableObject
{
    public string npcName;

    public CinematicDialogueTree dialogueTree;

}
