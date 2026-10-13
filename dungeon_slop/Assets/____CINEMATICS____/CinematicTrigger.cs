using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    [SerializeField] private CinematicDialogueController controller;
    [SerializeField] private CinematicDialogueTree dialogueTree;

    private void OnEnable()
    {
        if (controller != null)
        {
            controller.OnCinematicCompleted += EndCinematic;
        }
    }
    private void OnDisable()
    {
        if (controller != null)
        {
            controller.OnCinematicCompleted -= EndCinematic;
        }
    }

    private void Start()
    {
        StartCinematic();
    }

    public void StartCinematic()
    {
        controller.PlayDialogueTree(dialogueTree);
    }
    public void EndCinematic()
    {
        StartCoroutine(Scene_Manager.Instance.sceneController.ChangeScene(1, 1.5f));
    }
}