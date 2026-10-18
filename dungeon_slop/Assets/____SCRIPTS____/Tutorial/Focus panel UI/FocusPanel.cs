using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class FocusPanel : MonoSingleton<FocusPanel>
{
    [SerializeField]
    Image focusImage;
    [SerializeField]
    bool refreshEveryFrame = true;
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    [Min(0f)]
    float fadeDuration = 0.15f;

    readonly Image[] coverImages = new Image[4];
    RectTransform rectTransform;
    Tween fadeTween;
    Tween focusTween;

    public Image FocusImage => focusImage;
    public RectTransform FocusTransform => focusImage != null ? focusImage.transform as RectTransform : null;

    public override void Awake()
    {
        base.Awake();

        rectTransform = transform as RectTransform;
        EnsureCanvasGroup();
        EnsureCoverImages();
        Refresh();
        SetVisible(false, false);
    }

    void OnEnable()
    {
        EnsureCoverImages();
        Refresh();
    }

    void LateUpdate()
    {
        if (refreshEveryFrame)
        {
            Refresh();
        }
    }

    void OnRectTransformDimensionsChange()
    {
        if (!isActiveAndEnabled) return;

        Refresh();
    }

    public void SetFocus(Image image)
    {
        focusImage = image;
        Refresh();
    }

    public void SetFocus(Vector3 worldPosition, Vector2 size)
    {
        RectTransform focusTransform = FocusTransform;
        if (focusTransform == null) return;

        ConfigureFocusTransform(focusTransform);
        focusTransform.position = worldPosition;
        SetFocusSize(focusTransform, size);

        Refresh();
    }

    public Tween AnimateFocus(Vector3 worldPosition, Vector2 size, float duration, Ease ease = Ease.OutCubic)
    {
        RectTransform focusTransform = FocusTransform;
        if (focusTransform == null) return null;

        focusTween?.Kill();
        ConfigureFocusTransform(focusTransform);

        if (duration <= 0f)
        {
            SetFocus(worldPosition, size);
            return null;
        }

        focusTween = DOTween.Sequence()
            .Join(focusTransform.DOMove(worldPosition, duration).SetEase(ease))
            .Join(DOVirtual.Vector2(focusTransform.rect.size, size, duration, value => SetFocusSize(focusTransform, value)).SetEase(ease))
            .SetUpdate(true)
            .OnUpdate(Refresh)
            .OnComplete(() =>
            {
                Refresh();
                focusTween = null;
            });

        return focusTween;
    }

    void ConfigureFocusTransform(RectTransform focusTransform)
    {
        focusTransform.anchorMin = new Vector2(0.5f, 0.5f);
        focusTransform.anchorMax = new Vector2(0.5f, 0.5f);
        focusTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    void SetFocusSize(RectTransform focusTransform, Vector2 size)
    {
        focusTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Max(0f, size.x));
        focusTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(0f, size.y));
    }

    public void Show()
    {
        SetVisible(true, true);
    }

    public void Hide()
    {
        SetVisible(false, true);
    }

    public void SetVisible(bool visible, bool animate)
    {
        EnsureCanvasGroup();
        if (canvasGroup == null) return;

        fadeTween?.Kill();
        gameObject.SetActive(true);

        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;

        if (!animate || fadeDuration <= 0f)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            gameObject.SetActive(visible);
            return;
        }

        fadeTween = canvasGroup.DOFade(visible ? 1f : 0f, fadeDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                fadeTween = null;
                gameObject.SetActive(visible);
            });
    }

    public void Refresh()
    {
        if (rectTransform == null)
        {
            rectTransform = transform as RectTransform;
        }

        EnsureCoverImages();

        if (rectTransform == null || focusImage == null)
        {
            SetCoverImagesActive(false);
            return;
        }

        RectTransform focusTransform = focusImage.transform as RectTransform;
        if (focusTransform == null)
        {
            SetCoverImagesActive(false);
            return;
        }

        SetCoverImagesActive(true);
        SyncCoverImagesColor();
        ArrangeCoverImages(GetFocusRect(focusTransform));
    }

    void EnsureCanvasGroup()
    {
        if (canvasGroup != null) return;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void EnsureCoverImages()
    {
        for (int i = 0; i < coverImages.Length; i++)
        {
            if (coverImages[i] != null) continue;

            GameObject coverObject = new GameObject($"Focus Cover {i + 1}", typeof(RectTransform), typeof(Image));
            coverObject.transform.SetParent(transform, false);

            Image coverImage = coverObject.GetComponent<Image>();
            coverImage.sprite = null;
            coverImage.raycastTarget = true;

            RectTransform coverTransform = coverObject.transform as RectTransform;
            coverTransform.anchorMin = new Vector2(0.5f, 0.5f);
            coverTransform.anchorMax = new Vector2(0.5f, 0.5f);
            coverTransform.pivot = new Vector2(0.5f, 0.5f);

            coverImages[i] = coverImage;
        }
    }

    void ArrangeCoverImages(Rect focusRect)
    {
        Rect panelRect = rectTransform.rect;

        float left = panelRect.xMin;
        float right = panelRect.xMax;
        float bottom = panelRect.yMin;
        float top = panelRect.yMax;

        float focusLeft = Mathf.Clamp(focusRect.xMin, left, right);
        float focusRight = Mathf.Clamp(focusRect.xMax, left, right);
        float focusBottom = Mathf.Clamp(focusRect.yMin, bottom, top);
        float focusTop = Mathf.Clamp(focusRect.yMax, bottom, top);

        SetCoverRect(coverImages[0], left, focusTop, right, top);
        SetCoverRect(coverImages[1], left, bottom, right, focusBottom);
        SetCoverRect(coverImages[2], left, focusBottom, focusLeft, focusTop);
        SetCoverRect(coverImages[3], focusRight, focusBottom, right, focusTop);
    }

    Rect GetFocusRect(RectTransform focusTransform)
    {
        Vector3[] corners = new Vector3[4];
        focusTransform.GetWorldCorners(corners);

        Vector3 min = rectTransform.InverseTransformPoint(corners[0]);
        Vector3 max = min;

        for (int i = 1; i < corners.Length; i++)
        {
            Vector3 point = rectTransform.InverseTransformPoint(corners[i]);
            min = Vector3.Min(min, point);
            max = Vector3.Max(max, point);
        }

        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    void SetCoverRect(Image image, float xMin, float yMin, float xMax, float yMax)
    {
        RectTransform coverTransform = image.transform as RectTransform;
        float width = Mathf.Max(0f, xMax - xMin);
        float height = Mathf.Max(0f, yMax - yMin);

        image.enabled = width > 0f && height > 0f;
        coverTransform.anchoredPosition = new Vector2(xMin + width * 0.5f, yMin + height * 0.5f);
        coverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        coverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    void SyncCoverImagesColor()
    {
        Color color = focusImage.color;

        for (int i = 0; i < coverImages.Length; i++)
        {
            coverImages[i].color = color;
        }
    }

    void SetCoverImagesActive(bool active)
    {
        for (int i = 0; i < coverImages.Length; i++)
        {
            if (coverImages[i] != null)
            {
                coverImages[i].enabled = active;
            }
        }
    }

    protected override void OnDestroy()
    {
        fadeTween?.Kill();
        fadeTween = null;
        focusTween?.Kill();
        focusTween = null;

        base.OnDestroy();
    }
}
