using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingTextGenerator : MonoBehaviour
{
    bool alternateSide;
    TextMeshProUGUI lastActiveText;
    int accumulatedValue;
    float currentAlpha = 1f;

    [SerializeField] TMP_FontAsset textFont;

    public void SpawnFloatingText(int value, PlayType numberType, float size = 45f, float duration = 1.2f, float offsetY = 40f)
    {
        if (IsActiveTextValid())
        {
            AccumulateAndAnimateValue(value, size);
            return;
        }

        CreateAndAnimateNewText(value, numberType, size, duration, offsetY);
    }

    bool IsActiveTextValid()
    {
        return lastActiveText != null && lastActiveText.gameObject != null && currentAlpha > 0.6f;
    }

    void AccumulateAndAnimateValue(int value, float size)
    {
        int startVal = accumulatedValue;
        accumulatedValue += value;

        DOTween.To(() => startVal, x => {
            startVal = x;
            if (lastActiveText != null) lastActiveText.text = x.ToString();
        }, accumulatedValue, 0.4f).SetEase(Ease.OutQuad);

        lastActiveText.fontSize = CalculateDynamicSize(accumulatedValue, size);
        lastActiveText.transform.DOPunchScale(Vector3.one * 0.4f, 0.2f);
    }

    void CreateAndAnimateNewText(int value, PlayType numberType, float size, float duration, float offsetY)
    {
        accumulatedValue = value;
        currentAlpha = 1f;

        GameObject textObj = new GameObject("FloatingText", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(transform, false);

        RectTransform rect = textObj.GetComponent<RectTransform>();
        TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();

        if (textFont != null) tmp.font = textFont;

        float sideMultiplier = alternateSide ? 1f : -1f;
        alternateSide = !alternateSide;

        float randomX = Random.Range(30f, 90f) * sideMultiplier;
        Vector2 initialPos = new Vector2(randomX, offsetY);
        rect.anchoredPosition = initialPos;

        tmp.alignment = TextAlignmentOptions.Center;
        tmp.raycastTarget = false;
        tmp.text = "0";
        tmp.fontSize = CalculateDynamicSize(value, size);
        tmp.color = GetColorByType(numberType);

        lastActiveText = tmp;

        int displayVal = 0;
        DOTween.To(() => displayVal, x => {
            displayVal = x;
            if (tmp != null) tmp.text = x.ToString();
        }, value, duration * 0.3f).SetEase(Ease.OutQuad);

        textObj.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f);

        float peakX = randomX + (50f * sideMultiplier);
        float targetX = peakX + (30f * sideMultiplier);
        float peakY = offsetY + 130f;
        float targetY = offsetY - 40f;

        Sequence arcSeq = DOTween.Sequence();
        arcSeq.Append(rect.DOAnchorPos(new Vector2(peakX, peakY), duration * 0.5f).SetEase(Ease.OutQuad));
        arcSeq.Append(rect.DOAnchorPos(new Vector2(targetX, targetY), duration * 0.5f).SetEase(Ease.InQuad));

        Sequence fadeSeq = DOTween.Sequence();
        fadeSeq.AppendInterval(duration * 0.4f);
        fadeSeq.Append(tmp.DOFade(0f, duration * 0.4f).OnUpdate(() => {
            if (tmp != null) currentAlpha = tmp.color.a;
        }));

        arcSeq.Join(fadeSeq);
        arcSeq.OnComplete(() => {
            if (lastActiveText == tmp)
            {
                lastActiveText = null;
                currentAlpha = 1f;
            }
            Destroy(textObj);
        });
    }

    float CalculateDynamicSize(int value, float baseSize)
    {
        return baseSize + Mathf.Log(Mathf.Max(value, 1) + 1) * 8f;
    }

    Color GetColorByType(PlayType numberType)
    {
        return numberType switch
        {
            PlayType.Attack => Color.red,
            PlayType.Heal => Color.green,
            PlayType.Defense => Color.blue,
            _ => Color.white
        };
    }
}