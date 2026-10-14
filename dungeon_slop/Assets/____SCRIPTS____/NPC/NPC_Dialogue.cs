
using System;
using DG.Tweening;
using Febucci.TextAnimatorForUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class NPC_Dialogue : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private CanvasGroup panelGroup;

    //[Header("Sprite")]
    //[SerializeField] private Image spriteImage;
    //[SerializeField] private CanvasGroup spriteGroup;

    [Header("Text")]
    [SerializeField] private CanvasGroup textGroup;
    [SerializeField] private TypewriterComponent typewriter;

    [Header("Continue Indicator")]
    [SerializeField] private Image continueIndicator;
    [SerializeField] private float blinkDuration = 0.5f;
    [SerializeField] private float blinkMinAlpha = 0.15f;

    [Header("Input System")]
    [SerializeField] private InputActionReference advanceAction;

    [Header("Dialogue")]
    [SerializeField] private float dialogueDelay = 0.25f;

    [Header("Localization")]
    [SerializeField] private StringTable cinematicTable;

    private CinematicDialogueTree currentTree;
    private CinematicDialogueData currentDialogue;
    private int currentDialogueIndex;
    private int currentLineIndex;
    private bool isPlaying;
    private bool waitingForInput;
    private bool isTyping;
    private bool isTransitioning;
    private Sequence dialogueSequence;
    private Tween indicatorTween;

    public event Action OnDialogueCompleted;
    public bool IsPlaying => isPlaying;
    public bool IsWaitingForInput => waitingForInput;

    public void Initialize()
    {
        panelGroup.alpha = 0f;
        //spriteGroup.alpha = 0f;
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

        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        StopIndicator();
        KillSequence();
    }

    private void OnDestroy()
    {
        KillSequence();
    }

    private void LoadTable()
    {
        cinematicTable = LocalizationSettings.StringDatabase.GetTable("NPC_dialogue");
    }

    private void OnLocaleChanged(Locale locale)
    {
        LoadTable();
    }

    public void StartDialogue(CinematicDialogueTree tree)
    {
        if (tree == null || isPlaying || tree.dialogues == null || tree.dialogues.Count == 0)
            return;

        currentTree = tree;
        currentDialogueIndex = 0;
        currentLineIndex = 0;
        isPlaying = true;
        isTransitioning = false;
        waitingForInput = false;
        isTyping = false;

        StopIndicator();
        typewriter.onTextShowed.RemoveListener(OnTextShowed);
        typewriter.onTextShowed.AddListener(OnTextShowed);

        panelGroup.gameObject.SetActive(true);
        panelGroup.alpha = 1f;
        panelGroup.interactable = true;
        panelGroup.blocksRaycasts = true;

        //spriteGroup.alpha = 1f;
        textGroup.alpha = 1f;
        textGroup.gameObject.SetActive(true);

        LoadCurrentDialogue();

        if (currentDialogue != null)
            ShowCurrentLine();
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

        textGroup.gameObject.SetActive(true);
        textGroup.alpha = 1f;

        CinematicDialogueData.DialogueLine line = currentDialogue.lines[currentLineIndex];

        //if (line.changeSprite && line.sprite != null)
        //    spriteImage.sprite = line.sprite;

        string localizedText = line.text;

        if (cinematicTable != null)
        {
            StringTableEntry entry = cinematicTable.GetEntry(line.text);

            if (entry != null)
                localizedText = entry.GetLocalizedString();
        }

        typewriter.ShowText(string.Empty);
        typewriter.ShowText(localizedText);
    }

    private void LoadCurrentDialogue()
    {
        if (currentTree == null || currentDialogueIndex < 0 || currentDialogueIndex >= currentTree.dialogues.Count)
        {
            FinishDialogue();
            return;
        }

        currentDialogue = currentTree.dialogues[currentDialogueIndex];

        if (currentDialogue == null)
        {
            FinishDialogue();
            return;
        }

        currentLineIndex = 0;

        if (currentDialogue.lines != null && currentDialogue.lines.Count > 0)
        {
            CinematicDialogueData.DialogueLine firstLine = currentDialogue.lines[0];

            //if (firstLine.changeSprite && firstLine.sprite != null)
            //    spriteImage.sprite = firstLine.sprite;
        }
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

    private void AdvanceToNextDialogue()
    {
        StopIndicator();
        waitingForInput = false;
        isTyping = false;

        if (currentTree == null)
        {
            FinishDialogue();
            return;
        }

        currentDialogueIndex++;

        if (currentDialogueIndex >= currentTree.dialogues.Count)
        {
            FinishDialogue();
            return;
        }

        isTransitioning = true;
        KillSequence();

        dialogueSequence = DOTween.Sequence()
            .AppendInterval(dialogueDelay)
            .AppendCallback(() =>
            {
                if (!isPlaying || currentTree == null)
                    return;

                typewriter.ShowText(string.Empty);
                LoadCurrentDialogue();

                if (currentDialogue == null || !isPlaying)
                    return;

                isTransitioning = false;
                ShowCurrentLine();
            });
    }

    private void StartIndicator()
    {
        StopIndicator();
        continueIndicator.gameObject.SetActive(true);

        Color color = continueIndicator.color;
        color.a = 1f;
        continueIndicator.color = color;

        indicatorTween = continueIndicator
            .DOFade(blinkMinAlpha, blinkDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
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

    private void FinishDialogue()
    {
        if (!isPlaying)
            return;

        isPlaying = false;
        isTransitioning = false;
        waitingForInput = false;
        isTyping = false;

        StopIndicator();
        KillSequence();

        typewriter.onTextShowed.RemoveListener(OnTextShowed);
        typewriter.ShowText(string.Empty);

        textGroup.alpha = 0f;
        //spriteGroup.alpha = 0f;
        panelGroup.alpha = 0f;
        panelGroup.gameObject.SetActive(false);

        currentTree = null;
        currentDialogue = null;

        OnDialogueCompleted?.Invoke();
    }

    private void KillSequence()
    {
        dialogueSequence?.Kill();
        dialogueSequence = null;
    }
}
