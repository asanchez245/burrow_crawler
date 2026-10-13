
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCinematicDialogue", menuName = "Cinematic/CinematicDialogue")]
public class CinematicDialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(2, 5)]
        public string text;

        [Header("Sprite")]
        public bool changeSprite;
        public Sprite sprite;
    }

    [Header("Background")]
    public bool changeBackground;
    public Sprite background;

    [Header("Lines")]
    public List<DialogueLine> lines = new List<DialogueLine>();
}
