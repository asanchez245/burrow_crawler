
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCinematicDialogueTree",menuName = "Cinematic/CinematicDialogueTree")]
public class CinematicDialogueTree : ScriptableObject
{
    public List<CinematicDialogueData> dialogues = new List<CinematicDialogueData>();
}
