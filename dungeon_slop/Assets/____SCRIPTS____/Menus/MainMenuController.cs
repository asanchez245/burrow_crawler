using UnityEngine;

public class MainMenuController : TweenUtilities
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

    public void IncreaseButtonSize(GameObject button)
    {
        ScaleObject(button);
    }
    public void DecreaseButtonSize(GameObject button)
    {
        ScaleObject(button, 1);
    }
}
