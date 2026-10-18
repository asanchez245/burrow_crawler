using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPhase_ShowPanel : TutorialPhase
{
    [Header("Panel")]
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Button continueButton;

    [Header("Background")]
    [SerializeField] private CanvasGroup background;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.5f;

    public override void Initialize()
    {
        panel.DOKill();
        background.DOKill();

        panel.alpha = 0f;
        panel.gameObject.SetActive(false);

        continueButton.gameObject.SetActive(false);
        continueButton.onClick.RemoveListener(OnContinueClicked);
    }

    public override void StartPhase()
    {
        panel.DOKill();
        background.DOKill();

        continueButton.gameObject.SetActive(false);
        continueButton.onClick.RemoveListener(OnContinueClicked);

        // El background ya está activo.
        if (background.gameObject.activeSelf)
        {
            ShowPanel();
            return;
        }

        // El background todavía no está activo.
        ShowBackgroundAndPanel();
    }

    private void ShowBackgroundAndPanel()
    {
        background.gameObject.SetActive(true);
        background.alpha = 0f;

        panel.gameObject.SetActive(true);
        panel.alpha = 0f;

        Sequence sequence = DOTween.Sequence();

        // Primero aparece el background.
        sequence.Append(
            background.DOFade(1f, fadeDuration)
                .SetEase(Ease.OutSine)
        );

        // Después aparece el panel.
        sequence.Append(
            panel.DOFade(1f, fadeDuration)
                .SetEase(Ease.OutSine)
        );

        sequence.OnComplete(EnableContinueButton);
    }

    private void ShowPanel()
    {
        panel.gameObject.SetActive(true);
        panel.alpha = 0f;

        panel.DOFade(1f, fadeDuration)
            .SetEase(Ease.OutSine)
            .OnComplete(EnableContinueButton);
    }

    private void EnableContinueButton()
    {
        continueButton.gameObject.SetActive(true);
        continueButton.onClick.RemoveListener(OnContinueClicked);
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnContinueClicked()
    {
        continueButton.onClick.RemoveListener(OnContinueClicked);

        CompletePhase();
    }

    public override void EndPhase()
    {
        panel.DOKill();
        background.DOKill();

        continueButton.gameObject.SetActive(false);

        Sequence sequence = DOTween.Sequence();

        // Siempre desaparece primero el panel.
        sequence.Append(
            panel.DOFade(0f, fadeDuration)
                .SetEase(Ease.InSine)
        );

        sequence.AppendCallback(() =>
        {
            panel.gameObject.SetActive(false);
        });

        // Si la siguiente fase no es ShowPanel,
        // también hacemos desaparecer el background.
        if (NextPhase is not TutorialPhase_ShowPanel)
        {
            sequence.Append(
                background.DOFade(0f, fadeDuration)
                    .SetEase(Ease.InSine)
            );

            sequence.AppendCallback(() =>
            {
                background.gameObject.SetActive(false);
            });
        }
    }
}