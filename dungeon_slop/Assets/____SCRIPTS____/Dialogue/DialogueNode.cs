using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/DialogueNode")]
public class DialogueNode : ScriptableObject
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

    [Header("Lines")]
    public List<DialogueLine> lines = new List<DialogueLine>();
}
