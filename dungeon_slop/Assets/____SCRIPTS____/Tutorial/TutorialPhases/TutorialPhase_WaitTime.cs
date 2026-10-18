using UnityEngine;

public class TutorialPhase_WaitTime : TutorialPhase
{

    [Header("Duration")]
    [SerializeField] private float duration = 2f;

    [Header("Optional GameObject")]
    [SerializeField] private GameObject gameObjectToShow;
    [SerializeField] private bool hideOnEnd = true;

    private float timer;

    public override void Initialize()
    {
        timer = 0f;

        if (gameObjectToShow != null)
        {
            gameObjectToShow.SetActive(false);
        }
    }

    public override void StartPhase()
    {
        timer = 0f;

        if (gameObjectToShow != null)
        {
            gameObjectToShow.SetActive(true);
        }
    }

    private void Update()
    {
        if (timer >= duration)
            return;

        timer += Time.deltaTime;

        if (timer >= duration)
        {
            CompletePhase();
        }
    }

    public override void EndPhase()
    {
        if (hideOnEnd && gameObjectToShow != null)
        {
            gameObjectToShow.SetActive(false);
        }
    }
}
