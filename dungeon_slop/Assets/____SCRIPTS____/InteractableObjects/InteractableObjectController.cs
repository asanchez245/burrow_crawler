using UnityEngine;
using UnityEngine.InputSystem;
using static InteractableObjectController;

public class InteractableObjectController : MonoBehaviour
{
    ItemController item;
    RequiredItem requiredItem;
    NPC_Visuals npcVisuals;

    [SerializeField] PlayerInventary_Controller playerInventary;

    [Header("Input System")]
    [SerializeField] private InputActionReference interactAction;

    bool interactable;
    bool interacting;

    public InteractionType interactionType;

    public enum InteractionType
    {
        item,
        door,
        spwanPoint,
        demoEnd
    }

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        item = GetComponent<ItemController>();
        requiredItem = GetComponent<RequiredItem>();
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
        if (!interactable || interacting)
            return;

        npcVisuals.StopIndicator();

        Interaction(interactionType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        interactable = true;

        if (!interacting && npcVisuals != null)
            npcVisuals.ShowInteractionIcon(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        interactable = false;

        if (!interacting && npcVisuals != null)
            npcVisuals.ShowInteractionIcon(false);
    }

    public void Interaction(InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.item:
                playerInventary.PickUpItem(item);
                this.gameObject.SetActive(false);
                break;
            case InteractionType.door:
                if (playerInventary.CheckItemRequired(requiredItem.requiredItem))
                {
                    Debug.Log("abriendo puelta");
                    this.gameObject.SetActive(false);
                }
                else
                {
                    interacting = false;
                    interactable = true;

                    npcVisuals.ShowInteractionIcon(true);
                }
                break;
            case InteractionType.spwanPoint:
                GameManager.Instance.spawnPointChanged = true;
                this.gameObject.SetActive(false);
                break;
            case InteractionType.demoEnd:
                interactable = false;
                npcVisuals.ShowInteractionIcon(false);
                StartCoroutine(Scene_Manager.Instance.sceneController.ChangeScene(4, 1.5f));
                break;
        }
    }
}
