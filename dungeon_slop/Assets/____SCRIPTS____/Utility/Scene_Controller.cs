using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Controller : MonoSingleton<Scene_Controller>
{
    public GameObject fadePanel;


    public override void Awake()
    {
        DontDestroyOnLoad(gameObject);
        transform.SetAsLastSibling();
        SceneManager.sceneLoaded += OnSceneLoaded;

        FadeScene(true, 1f);
    }
    public Tween FadeScene(bool fadeIn, float? time = null)
    {
        if (fadePanel != null)
        {
            fadePanel.SetActive(true);
        }
        CanvasGroup panelGroup = fadePanel.GetComponent<CanvasGroup>();
        panelGroup.DOKill();

        int endvalue = fadeIn ? 0 : 1;
        float fadeTime = time ?? 1.5f;

        return panelGroup.DOFade(endvalue, fadeTime).OnComplete(() =>
        {
            if (fadeIn)
            {
                fadePanel.SetActive(false);
            }
        });
    }

    public IEnumerator ChangeScene(int sceneToLoad, float timeToSwitch)
    {
        FadeScene(false, timeToSwitch);
        yield return new WaitForSeconds(timeToSwitch + 0.25f);
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeScene(true, 1);
    }
}
