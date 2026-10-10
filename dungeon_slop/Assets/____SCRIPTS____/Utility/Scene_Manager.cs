using UnityEngine;

public class Scene_Manager : MonoSingleton<Scene_Manager>
{
    public Scene_Controller sceneController;

    public override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }
}
