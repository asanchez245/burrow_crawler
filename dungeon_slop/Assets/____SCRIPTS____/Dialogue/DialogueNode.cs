using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogueText;

    public DialogueNode[] nextNodes;
}
