using System.Collections.Generic;
using DG.Tweening;
using Febucci.TextAnimatorForUnity;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPhase_DialoguePanel : TutorialPhase
{
    [Header("Panel")]
    [SerializeField] private CanvasGroup panelGroup;

    [Header("Background")]
    [SerializeField] private CanvasGroup backgroundGroup;

    [Header("Sprite")]
    [SerializeField] private Image spriteImage;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private CanvasGroup spriteGroup;

    [Header("Text")]
    [SerializeField] private CanvasGroup textGroup;
    [SerializeField] private TypewriterComponent typewriter;

    [Header("Continue")]
    [SerializeField] private Button continueButton;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    [SerializeField] private List<string> texts;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.25f;

    private int currentTextIndex;
    private bool phaseFinished;

    public override void Initialize()
    {
        KillTweens();

        currentTextIndex = 0;
        phaseFinished = false;

        panelGroup.alpha = 0f;
        backgroundGroup.alpha = 0f;
        spriteGroup.alpha = 0f;
        textGroup.alpha = 0f;

        panelGroup.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        continueButton.onClick.RemoveListener(OnContinueClicked);

        if (typewriter != null)
        {
            typewriter.onTextShowed.RemoveListener(OnTextShowed);
        }
    }

    public override void StartPhase()
    {
        typewriter.ShowText(string.Empty);

        spriteImage.sprite = selectedSprite;
        
        currentTextIndex = 0;
        phaseFinished = false;

        panelGroup.gameObject.SetActive(true);

        continueButton.gameObject.SetActive(false);
        continueButton.onClick.RemoveListener(OnContinueClicked);

        typewriter.onTextShowed.RemoveListener(OnTextShowed);
        typewriter.onTextShowed.AddListener(OnTextShowed);


        PlayIntro();
    }

    private void PlayIntro()
    {
        panelGroup.alpha = 0f;
        backgroundGroup.alpha = 0f;
        spriteGroup.alpha = 0f;
        textGroup.alpha = 0f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            panelGroup.DOFade(1f, fadeDuration / 2)
                .SetEase(Ease.OutSine)
        );

        sequence.Join(
            backgroundGroup.DOFade(1f, fadeDuration / 2)
                .SetEase(Ease.OutSine)
        );

        sequence.Append(
            spriteGroup.DOFade(1f, fadeDuration)
                .SetEase(Ease.OutSine)
        );

        sequence.Append(
            textGroup.DOFade(1f, fadeDuration/2)
                .SetEase(Ease.OutSine)
        );

        sequence.OnComplete(ShowCurrentText);
    }

    private void ShowCurrentText()
    {
        if (texts == null || texts.Count == 0)
        {
            FinishPhase();
            return;
        }

        continueButton.gameObject.SetActive(false);
        continueButton.onClick.RemoveListener(OnContinueClicked);

        typewriter.ShowText(texts[currentTextIndex]);
    }

    private void OnTextShowed()
    {
        continueButton.gameObject.SetActive(true);

        continueButton.onClick.RemoveListener(OnContinueClicked);
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnContinueClicked()
    {
        continueButton.onClick.RemoveListener(OnContinueClicked);
        continueButton.gameObject.SetActive(false);

        currentTextIndex++;

        if (currentTextIndex < texts.Count)
        {
            ShowCurrentText();
        }
        else
        {
            FinishPhase();
        }
    }

    private void FinishPhase()
    {
        if (phaseFinished)
            return;

        phaseFinished = true;

        PlayOutro();
    }

    private void PlayOutro()
    {
        KillTweens();

        Sequence sequence = DOTween.Sequence();

        // Primero desaparece el texto.
        sequence.Append(
            textGroup.DOFade(0f, fadeDuration)
                .SetEase(Ease.InSine)
        );

        // Después el sprite.
        sequence.Append(
            spriteGroup.DOFade(0f, fadeDuration/2)
                .SetEase(Ease.InSine)
        );

        // Después el background.
        sequence.Append(
            backgroundGroup.DOFade(0f, fadeDuration / 2)
                .SetEase(Ease.InSine)
        );

        // Finalmente el panel.
        sequence.Join(
            panelGroup.DOFade(0f, fadeDuration / 2)
                .SetEase(Ease.InSine)
        );

        sequence.OnComplete(() =>
        {
            panelGroup.gameObject.SetActive(false);

            CompletePhase();
        });
    }

    public override void EndPhase()
    {
        KillTweens();

        continueButton.onClick.RemoveListener(OnContinueClicked);
        continueButton.gameObject.SetActive(false);

        if (typewriter != null)
        {
            typewriter.onTextShowed.RemoveListener(OnTextShowed);
        }

        panelGroup.gameObject.SetActive(false);
    }

    private void KillTweens()
    {
        if (panelGroup != null)
            panelGroup.DOKill();

        if (backgroundGroup != null)
            backgroundGroup.DOKill();

        if (spriteGroup != null)
            spriteGroup.DOKill();

        if (textGroup != null)
            textGroup.DOKill();
    }
}