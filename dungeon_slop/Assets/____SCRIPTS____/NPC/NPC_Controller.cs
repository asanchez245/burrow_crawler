using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_Controller : MonoBehaviour
{
    public NPC_Data npcData;

    NPC_Info npcInfo;
    NPC_Visuals npcVisuals;
    NPC_Dialogue npcDialogue;

    [Header("Input System")]
    [SerializeField] private InputActionReference interactAction;

    bool interactuable;
    bool inDialogue;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        npcInfo = GetComponent<NPC_Info>();
        if (npcInfo) npcInfo.Initialize(npcData);

        npcVisuals = GetComponent<NPC_Visuals>();
        if (npcVisuals) npcVisuals.Initialize();

        npcDialogue = GetComponent<NPC_Dialogue>();

        if (npcDialogue)
        {
            npcDialogue.Initialize();
            npcDialogue.OnDialogueCompleted -= DialogueEnd;
            npcDialogue.OnDialogueCompleted += DialogueEnd;
        }
    }

    private void OnEnable()
    {
        if (interactAction != null && interactAction.action != null)
        {
            interactAction.action.performed += OnInteractInput;
            interactAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (interactAction != null && interactAction.action != null)
            interactAction.action.performed -= OnInteractInput;

        if (npcDialogue != null)
            npcDialogue.OnDialogueCompleted -= DialogueEnd;
    }

    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (!interactuable || inDialogue || npcDialogue == null)
            return;

        npcVisuals.StopIndicator();
        inDialogue = true;
        npcDialogue.StartDialogue(npcData.dialogueTree);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        interactuable = true;

        if (!inDialogue && npcVisuals != null)
            npcVisuals.ShowInteractionIcon(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        interactuable = false;

        if (!inDialogue && npcVisuals != null)
            npcVisuals.ShowInteractionIcon(false);
    }

    void DialogueEnd()
    {
        inDialogue = false;
        interactuable = false;
    }
}