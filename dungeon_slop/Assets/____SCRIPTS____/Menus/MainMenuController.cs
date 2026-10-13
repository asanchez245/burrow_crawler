using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Scene_Controller sceneController;
    public void StartGame()
    {
        StartCoroutine(Scene_Manager.Instance.sceneController.ChangeScene(3, 1.5f));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
