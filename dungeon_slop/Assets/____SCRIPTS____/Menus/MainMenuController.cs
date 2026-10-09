using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Scene_Controller sceneController;
    public void StartGame()
    {
        sceneController.StartCoroutine(sceneController.ChangeScene(1, 1.5f));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
