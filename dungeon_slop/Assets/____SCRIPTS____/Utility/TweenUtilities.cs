using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class TweenUtilities : MonoBehaviour
{
    #region Object Blinking
    public Tween ObjectBlinking(CanvasGroup canvasGroup, float blinkSpeed = 1)
    {
        Tween blinkingTween = canvasGroup.DOFade(0, blinkSpeed)
            .SetLoops(-1, LoopType.Yoyo);
        return blinkingTween;
    }
    #endregion

    #region Object Scale
    public Tween ScaleObject(GameObject imagen, float scaleFactor = 1.15f, float scaleTime = .2f)
    {
        Tween scaleObject = imagen.transform.DOScale(new Vector3(scaleFactor, scaleFactor, 1f), scaleTime);
        return scaleObject;
    }
    #endregion

    #region Object Shake
    public Tween ObjectShake(GameObject objectToVibrate)
    {
        Tween vibrationTween = vibrationTween = objectToVibrate.transform
                .DOShakePosition(1f, strength: new Vector3(5f, 5f, 0f), vibrato: 50, randomness: 90, snapping: false, fadeOut: false)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        return vibrationTween;
    }

    public Tween ShakeOnce(GameObject objectToVibrate)
    {
        return objectToVibrate.transform.DOShakePosition(
            0.1f,
            strength: new Vector3(5f, 5f, 0f),
            vibrato: 10,
            randomness: 90,
            snapping: false,
            fadeOut: true
        );
    }
    #endregion

    #region LaunchObject
    public Sequence LaunchObject(GameObject objectToLaunch, float duracionEntrada = 0.30f, float offsetInferior = -900f,float randomnesRotation = 25f)
    {
        ParticleSystem objectSmoke = objectToLaunch.GetComponentInChildren<ParticleSystem>();
        Vector3 posFinal = objectToLaunch.transform.localPosition;
        Vector3 posInicio = posFinal + new Vector3(0, offsetInferior, 0);
        objectToLaunch.transform.localPosition = posInicio;
        objectToLaunch.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-randomnesRotation, randomnesRotation)));
        if(!objectToLaunch.activeSelf) objectToLaunch.SetActive(true);
        Sequence throwSequence = DOTween.Sequence();
        throwSequence.AppendCallback(() => objectSmoke.Play());
        throwSequence.Append(objectToLaunch.transform.DOLocalMove(posFinal, duracionEntrada)
            .SetEase(Ease.OutCirc));
        throwSequence.Join(objectToLaunch.GetComponent<CanvasGroup>().DOFade(1f, duracionEntrada * 0.4f));
        return throwSequence;
    }
    #endregion

    #region FadeObject
    public Tween FadeInObject(CanvasGroup canvasGroup, float fadeTime = .5f)
    {
        canvasGroup.gameObject.SetActive(true);
        return canvasGroup.DOFade(1, fadeTime).From(0);
    }
    public TweenCallback FadeOutObject(CanvasGroup canvasGroup, float fadeTime = .5f)
    {
        return canvasGroup.DOFade(0, fadeTime).From(1).onComplete = () =>
        {
            canvasGroup.gameObject.SetActive(false);
        };     
    }

    #endregion

    public Tween PunchObject(GameObject objectToPunch, float punchStrength = 1.2f, float punchDuration = .2f)
    {
        Tween punchTween = objectToPunch.transform.DOPunchScale(new Vector3(punchStrength, punchStrength, 1f), punchDuration, vibrato: 10, elasticity: 1f);
        return punchTween;
    }

    #region GainNumbers
    public Sequence GainNumbers(GameObject numbersObject, int startValue, int endValue, float duration = 3f)
    {
        TextMeshProUGUI numberText = numbersObject.GetComponent<TextMeshProUGUI>();

        int lastValue = startValue;
        float progress = 0f;

        numberText.text = startValue.ToString();

        Sequence sequence = DOTween.Sequence();

        Tween numberTween = DOTween.To(
            () => progress,
            x => progress = x,
            1f,
            duration
        )
        .SetEase(Ease.OutCubic)
        .OnUpdate(() =>
        {
            int currentValue = Mathf.RoundToInt(
                Mathf.Lerp(startValue, endValue, progress)
            );

            numberText.text = currentValue.ToString();

            // Solo ejecutar efectos cuando el número cambia
            if (currentValue != lastValue)
            {
                lastValue = currentValue;

                // Matar tweens visuales anteriores
                numbersObject.transform.DOKill();

                // Reset
                numbersObject.transform.localScale = Vector3.one;
                numbersObject.transform.localPosition = Vector3.zero;

                // Punch escala
                numbersObject.transform.DOPunchScale(
                    Vector3.one * 0.15f,
                    0.12f,
                    vibrato: 8,
                    elasticity: 0.8f
                );

                // Shake corto
                numbersObject.transform.DOShakePosition(
                    0.08f,
                    new Vector3(3f, 3f, 0f),
                    vibrato: 8,
                    randomness: 90,
                    snapping: false,
                    fadeOut: true
                );
            }
        });

        sequence.Append(numberTween);

        return sequence;
    }
    #endregion
}
