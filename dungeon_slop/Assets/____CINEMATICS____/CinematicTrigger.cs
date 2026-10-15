using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    [SerializeField] private CinematicDialogueController controller;
    [SerializeField] private CinematicDialogueTree dialogueTree;

    public bool outro;
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
        if(!outro)
            StartCoroutine(Scene_Manager.Instance.sceneController.ChangeScene(1, 1.5f));

        else
            StartCoroutine(Scene_Manager.Instance.sceneController.ChangeScene(0, 1.5f));
    }
}