using UnityEngine;

public static class ScreenUtilities
{
    public static Vector2 RecTransformToScreenPosition(RectTransform rt)
    {
        Vector2 screenPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt,
            Vector2.zero,
            null,
            out screenPos
        );

        Vector2 realScreenPos = RectTransformUtility.WorldToScreenPoint(null, rt.position);

        return realScreenPos;
    }
}
