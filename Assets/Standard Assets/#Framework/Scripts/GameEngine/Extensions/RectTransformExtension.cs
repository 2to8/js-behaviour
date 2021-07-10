using UnityEngine;

namespace GameEngine.Extensions {

public static class RectTransformExtension {

    // places rect transform to have the same dimensions as 'other', even if they don't have same parent.
    // Relatively non-expensive.
    // NOTICE - also modifies scale of your rectTransf to match the scale of other
    public static void MatchOther(this RectTransform rt, RectTransform other)
    {
        var myPrevPivot = rt.pivot;
        myPrevPivot = other.pivot;
        rt.position = other.position;
        rt.localScale = other.localScale;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, other.rect.width);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, other.rect.height);

        //rectTransf.ForceUpdateRectTransforms(); - needed before we adjust pivot a second time?
        rt.pivot = myPrevPivot;
    }

    public static float Height(this RectTransform rt, float height = float.NaN)
    {
        if (!float.IsNaN(height)) {
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        return rt.rect.height;
    }

    public static float Width(this RectTransform rt, float width = float.NaN)
    {
        if (!float.IsNaN(width)) {
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        return rt.rect.width;
    }

    public static float Top(this RectTransform rt, float top = float.NaN)
    {
        if (!float.IsNaN(top)) {
            rt.offsetMax = new Vector2(rt.offsetMax.x, top);
        }

        return rt.offsetMax.y;
    }

    public static float Bottom(this RectTransform rt, float bottom = float.NaN)
    {
        if (!float.IsNaN(bottom)) {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }

        return rt.offsetMin.y;
    }

    //public static Vector2 Pos(this RectTransform rt) => rt.anchoredPosition;

    public static Vector2 Pos(this RectTransform rt, Vector2 pos)
    {
        rt.anchoredPosition = pos;

        return rt.anchoredPosition;
    }

    public static Vector2 Pos(this RectTransform rt, float x = float.NaN, float y = float.NaN)
    {
        if (!float.IsNaN(x) || !float.IsNaN(y)) {
            rt.anchoredPosition = new Vector2(float.IsNaN(x) ? rt.anchoredPosition.x : x,
                float.IsNaN(y) ? rt.anchoredPosition.y : y);
        }

        return rt.anchoredPosition;
    }

    public static float PosX(this RectTransform rt, float x = float.NaN)
    {
        if (!float.IsNaN(x)) {
            rt.anchoredPosition = new Vector2(x, rt.anchoredPosition.y);
        }

        return rt.anchoredPosition.x;
    }

    public static float PosY(this RectTransform rt, float y = float.NaN)
    {
        if (!float.IsNaN(y)) {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, y);
        }

        return rt.anchoredPosition.y;
    }

    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

}

}