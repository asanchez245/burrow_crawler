using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Visuals : MonoBehaviour
{
    [SerializeField] Image interactionIcon;

    [Header("Yoyo Tween")]
    [SerializeField] private float blinkDuration = 0.5f;
    [SerializeField] private float blinkMinAlpha = 0.15f;
    private Tween indicatorTween;



    public void Initialize()
    {
        interactionIcon.gameObject.SetActive(false);
    }

    internal void ShowInteractionIcon(bool showing)
    {
        Debug.Log("hoka");
        interactionIcon.DOKill();
        if (showing)
        {
            interactionIcon.gameObject.SetActive(true);
            interactionIcon.DOFade(1, .25f).From(0).OnComplete(() =>
            {
                StartIndicator();
            });
        }
        else
        {
            StopIndicator();
        }
    }

    private void StartIndicator()
    {
        StopIndicator();
        interactionIcon.gameObject.SetActive(true);
        Color color = interactionIcon.color;
        color.a = 1f;
        interactionIcon.color = color;
        indicatorTween = interactionIcon.DOFade(blinkMinAlpha, blinkDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }

    public void StopIndicator()
    {
        if (indicatorTween != null)
        {
            indicatorTween.Kill();
            indicatorTween = null;
        }

        if (interactionIcon == null)
            return;

        interactionIcon.DOKill();
        interactionIcon.gameObject.SetActive(false);
        Color color = interactionIcon.color;
        color.a = 1f;
        interactionIcon.color = color;
    }
}
