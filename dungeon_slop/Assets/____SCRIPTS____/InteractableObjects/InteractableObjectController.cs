using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class InteractableObjectController : MonoBehaviour
{
    ItemController item;
    RequiredItem requiredItem;
    NPC_Visuals npcVisuals;

    [SerializeField] PlayerInventary_Controller playerInventary;

    [Header("Input System")]
    [SerializeField] private InputActionReference interactAction;

    bool neverInteractable;
    bool interactable;
    bool interacting;

    public CanvasGroup msgContainer;
    public TextMeshProUGUI msg;
    public InteractionType interactionType;
    [SerializeField] GameObject objectToHide;

    public InteractableData interactableData;
    private string ActiveKey => $"Item_{interactableData.itemID}_Active";
    private string InventoryKey => $"Item_{interactableData.itemID}_Inventory";


    public enum InteractionType
    {
        item,
        door,
        spwanPoint,
        demoEnd,
        specialItem,
        button,
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

        bool isActive = PlayerPrefs.GetInt(ActiveKey, interactableData.activeInScene ? 1 : 0) == 1;
        bool isInInventory = PlayerPrefs.GetInt(InventoryKey, interactableData.inInventory ? 1 : 0) == 1;

        if (interactableData != null)
        {
            if (!interactableData.activeInScene || !isActive)
            {
                if (interactableData.inInventory || isInInventory)
                {
                    playerInventary.PickUpItem(item);
                }
                Destroy(this.gameObject);
                return;
            }
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
    }

    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (!interactable || interacting || neverInteractable)
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
    IEnumerator ShowMessage(string text)
    {
        msg.text = text;
        msgContainer.DOFade(1, .75f).From(0);
        yield return new WaitForSeconds(2f);
        msgContainer.DOFade(0, .75f);
    }
    public AudioClip sound;
    void PlaySound()
    {
            if (sound != null)
            {
                Vector3 spawnPosition = transform.position;
                GameObject objectSound = new GameObject("TemporaryAudioObject");
                objectSound.transform.position = spawnPosition;

                AudioSource audioSource = objectSound.AddComponent<AudioSource>();
                audioSource.clip = sound;
                audioSource.Play();

                Destroy(objectSound, sound.length);
            }
    }
    public void Interaction(InteractionType interactionType)
    {
        if (interactionType != InteractionType.door || playerInventary.CheckItemRequired(requiredItem.requiredItem))
        {
            PlaySound();
        }
        switch (interactionType)
        {
            case InteractionType.item:
                playerInventary.PickUpItem(item);
                interactableData.activeInScene = false;
                interactableData.inInventory = true;
                PlayerPrefs.SetInt(ActiveKey, 0);
                PlayerPrefs.SetInt(InventoryKey, 1);
                PlayerPrefs.Save();
                this.gameObject.SetActive(false);
                break;
            case InteractionType.specialItem:
                playerInventary.PickUpItem(item);
                objectToHide.SetActive(false);
                interactableData.activeInScene = false;
                interactableData.inInventory = true;
                PlayerPrefs.SetInt(ActiveKey, 0);
                PlayerPrefs.SetInt(InventoryKey, 1);
                PlayerPrefs.Save();
                this.gameObject.SetActive(false);
                break;
            case InteractionType.door:
                if (playerInventary.CheckItemRequired(requiredItem.requiredItem))
                {
                    Debug.Log("abriendo puelta");
                    interactableData.activeInScene = false;
                    PlayerPrefs.SetInt(ActiveKey, 1);
                    PlayerPrefs.Save();
                    this.gameObject.SetActive(false);
                }
                else
                {
                    interacting = false;
                    interactable = true;

                    npcVisuals.ShowInteractionIcon(true);
                }
                break;
            case InteractionType.button:
                neverInteractable = true;
                StartCoroutine(ShowMessage("Algo se ha abierto en la mazmorra..."));


                if (objectToHide != null)
                {
                    InteractableObjectController controller = objectToHide.GetComponent<InteractableObjectController>();
                    controller.interactableData.activeInScene = false;
                    PlayerPrefs.SetInt($"Item_{controller.interactableData.itemID}_Active", 0);
                    PlayerPrefs.Save();
                    objectToHide.SetActive(false);
                }
                interactable = false;
                //this.gameObject.SetActive(false);
                break;
            case InteractionType.spwanPoint:
                StartCoroutine(ShowMessage("Tomas un sorbo.\n <wiggle>(Reaparición reestablecida)"));
                GameManager.Instance.spawnPointChanged = true;
                interactable = false;
                break;
            case InteractionType.demoEnd:
                interactable = false;
                npcVisuals.ShowInteractionIcon(false);
                Destroy(GameManager.Instance.gameObject);
                StartCoroutine(Scene_Manager.Instance.sceneController.ChangeScene(4, 1.5f));
                break;
        }
    }
}
