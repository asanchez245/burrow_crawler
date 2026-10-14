using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObjectController : MonoBehaviour
{
    ItemController item;
    NPC_Visuals npcVisuals;

    [SerializeField] PlayerInventary_Controller playerInventary;

    [Header("Input System")]
    [SerializeField] private InputActionReference interactAction;

    bool interactuable;
    bool interacting;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        item = GetComponent<ItemController>();
        npcVisuals = GetComponent<NPC_Visuals>();
        if (npcVisuals) npcVisuals.Initialize();

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
    }

    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (!interactuable || interacting)
            return;

        npcVisuals.StopIndicator();

        Interaction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        interactuable = true;

        if (!interacting && npcVisuals != null)
            npcVisuals.ShowInteractionIcon(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        interactuable = false;

        if (!interacting && npcVisuals != null)
            npcVisuals.ShowInteractionIcon(false);
    }

    public void Interaction()
    {
        playerInventary.PickUpItem(item);
        this.gameObject.SetActive(false);
    }
}
