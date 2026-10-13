using DG.Tweening;
using Febucci.TextAnimatorForUnity;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class CinematicDialogueController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private CanvasGroup panelGroup;

    [Header("Sprite")]
    [SerializeField] private Image spriteImage;
    [SerializeField] private CanvasGroup spriteGroup;

    [Header("Text")]
    [SerializeField] private CanvasGroup textGroup;
    [SerializeField] private TypewriterComponent typewriter;

    [Header("Continue Indicator")]
    [SerializeField] private Image continueIndicator;
    [SerializeField] private float blinkDuration = 0.5f;
    [SerializeField] private float blinkMinAlpha = 0.15f;

    [Header("Input System")]
    [SerializeField] private InputActionReference advanceAction;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float dialogueDelay = 0.25f;

    [Header("Localization")]
    public StringTable cinematicTable;


    private CinematicDialogueTree currentTree;
    private CinematicDialogueData currentDialogue;
    private int currentDialogueIndex;
    private int currentLineIndex;
    private bool isPlaying;
    private bool isTransitioning;
    private bool waitingForInput;
    private bool isTyping;
    private Sequence animationSequence;
    private Tween indicatorTween;

    public event Action OnCinematicCompleted;
    public bool IsPlaying => isPlaying;
    public bool IsWaitingForInput => waitingForInput;

    private void Awake()
    {
        panelGroup.alpha = 0f;
        spriteGroup.alpha = 0f;
        textGroup.alpha = 0f;
        panelGroup.gameObject.SetActive(false);
        continueIndicator.gameObject.SetActive(false);
        typewriter.onTextShowed.RemoveListener(OnTextShowed);
    }

    private void OnEnable()
    {
        if (advanceAction != null && advanceAction.action != null)
        {
            advanceAction.action.performed += OnAdvanceInput;
            advanceAction.action.Enable();
        }

        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        LoadTable();
    }

    private void OnDisable()
    {
        if (advanceAction != null && advanceAction.action != null)
            advanceAction.action.performed -= OnAdvanceInput;
        if (typewriter != null)
            typewriter.onTextShowed.RemoveListener(OnTextShowed);
        StopIndicator();
        KillTweens();

        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;

    }

    void LoadTable()
    {
        cinematicTable = LocalizationSettings.StringDatabase.GetTable("Cinematics");
    }

    void OnLocaleChanged(Locale locale)
    {
        LoadTable();
        //cambiar el texto acutal al del nuevo idioma si eso
    }

    private void OnDestroy()
    {
        KillTweens();
    }

    public void PlayDialogueTree(CinematicDialogueTree tree)
    {
        if (tree == null || isPlaying || isTransitioning || tree.dialogues == null || tree.dialogues.Count == 0)
            return;

        currentTree = tree;
        currentDialogueIndex = 0;
        currentLineIndex = 0;
        isPlaying = true;
        isTransitioning = true;
        waitingForInput = false;
        StopIndicator();
        typewriter.onTextShowed.RemoveListener(OnTextShowed);
        typewriter.onTextShowed.AddListener(OnTextShowed);
        panelGroup.gameObject.SetActive(true);
        LoadCurrentDialogue();
        if (currentDialogue != null)
            PlayIntro();
    }

    private void LoadCurrentDialogue()
    {
        if (currentTree == null || currentDialogueIndex < 0 || currentDialogueIndex >= currentTree.dialogues.Count)
        {
            FinishCinematic();
            return;
        }

        currentDialogue = currentTree.dialogues[currentDialogueIndex];
        if (currentDialogue == null)
        {
            FinishCinematic();
            return;
        }

        currentLineIndex = 0;
        ApplyDialogueVisuals();
    }

    private void ApplyDialogueVisuals()
    {
        if (currentDialogue.lines == null || currentDialogue.lines.Count == 0)
            return;

        CinematicDialogueData.DialogueLine firstLine = currentDialogue.lines[0];
        if (firstLine.changeSprite && firstLine.sprite != null)
            spriteImage.sprite = firstLine.sprite;
    }

    private void PlayIntro()
    {
        KillTweens();
        panelGroup.gameObject.SetActive(true);
        panelGroup.alpha = 0f;
        spriteGroup.alpha = 0f;
        textGroup.alpha = 0f;
        isTransitioning = true;

        animationSequence = DOTween.Sequence();
        animationSequence.Append(panelGroup.DOFade(1f, fadeDuration / 2).SetEase(Ease.OutSine));
        animationSequence.Append(spriteGroup.DOFade(1f, fadeDuration).SetEase(Ease.OutSine));
        animationSequence.Append(textGroup.DOFade(1f, fadeDuration / 2).SetEase(Ease.OutSine));
        animationSequence.OnComplete(() =>
        {
            animationSequence = null;
            isTransitioning = false;
            ShowCurrentLine();
        });
    }

    private void PlayOutro(Action onComplete, bool betweenDialogues = false)
    {
        KillTweens();
        StopIndicator();
        isTransitioning = true;
        animationSequence = DOTween.Sequence();
        animationSequence.Append(textGroup.DOFade(0f, fadeDuration).SetEase(Ease.InSine));
        animationSequence.AppendCallback(() => typewriter.ShowText(string.Empty));
        animationSequence.Append(spriteGroup.DOFade(0f, fadeDuration / 2).SetEase(Ease.InSine));
        animationSequence.Join(panelGroup.DOFade(0f, fadeDuration / 2).SetEase(Ease.InSine));
        if (betweenDialogues)
            animationSequence.AppendInterval(dialogueDelay);
        animationSequence.OnComplete(() =>
        {
            animationSequence = null;
            panelGroup.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    private void AdvanceToNextDialogue()
    {
        StopIndicator();
        waitingForInput = false;

        if (currentTree == null)
        {
            FinishCinematic();
            return;
        }

        currentDialogueIndex++;
        if (currentDialogueIndex >= currentTree.dialogues.Count)
        {
            FinishCinematic();
            return;
        }

        isTransitioning = true;
        PlayOutro(() =>
        {
            if (!isPlaying || currentTree == null)
                return;
            LoadCurrentDialogue();
            if (currentDialogue != null)
                PlayIntro();
        }, true);
    }

    private void ShowCurrentLine()
    {
        if (!isPlaying || currentDialogue == null)
            return;

        if (currentDialogue.lines == null || currentLineIndex >= currentDialogue.lines.Count)
        {
            AdvanceToNextDialogue();
            return;
        }

        StopIndicator();
        waitingForInput = false;
        isTyping = true;

        CinematicDialogueData.DialogueLine line = currentDialogue.lines[currentLineIndex];

        if (line.changeSprite && line.sprite != null)
            spriteImage.sprite = line.sprite;

        string localizedText = cinematicTable.GetEntry(line.text).GetLocalizedString();

        typewriter.ShowText(localizedText);
    }

    private void OnTextShowed()
    {
        if (!isPlaying || isTransitioning)
            return;

        isTyping = false;
        waitingForInput = true;
        StartIndicator();
    }

    private void OnAdvanceInput(InputAction.CallbackContext context)
    {
        if (!isPlaying || isTransitioning)
            return;

        if (isTyping)
        {
            typewriter.SkipTypewriter();
            return;
        }

        if (!waitingForInput)
            return;

        waitingForInput = false;
        StopIndicator();

        currentLineIndex++;

        if (currentDialogue != null && currentDialogue.lines != null && currentLineIndex < currentDialogue.lines.Count)
        {
            ShowCurrentLine();
            return;
        }

        AdvanceToNextDialogue();
    }

    private void StartIndicator()
    {
        StopIndicator();
        continueIndicator.gameObject.SetActive(true);
        Color color = continueIndicator.color;
        color.a = 1f;
        continueIndicator.color = color;
        indicatorTween = continueIndicator.DOFade(blinkMinAlpha, blinkDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }

    private void StopIndicator()
    {
        if (indicatorTween != null)
        {
            indicatorTween.Kill();
            indicatorTween = null;
        }

        if (continueIndicator == null)
            return;

        continueIndicator.DOKill();
        continueIndicator.gameObject.SetActive(false);
        Color color = continueIndicator.color;
        color.a = 1f;
        continueIndicator.color = color;
    }

    private void FinishCinematic()
    {
        if (!isPlaying)
            return;
        waitingForInput = false;
        isTransitioning = true;
        StopIndicator();
        PlayOutro(CloseCinematic);
    }

    private void CloseCinematic()
    {
        isPlaying = false;
        isTransitioning = false;
        waitingForInput = false;
        currentTree = null;
        currentDialogue = null;
        typewriter.onTextShowed.RemoveListener(OnTextShowed);
        OnCinematicCompleted?.Invoke();
    }

    private void KillTweens()
    {
        animationSequence?.Kill();
        animationSequence = null;
        if (panelGroup != null)
            panelGroup.DOKill();
        if (spriteGroup != null)
            spriteGroup.DOKill();
        if (textGroup != null)
            textGroup.DOKill();
    }
}
