using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameEngine.Extensions {

/// <summary>
///     RectTransform 的 DOJump 效果位移不对的补丁
///     https://blog.csdn.net/qq_37896049/article/details/85606527
/// </summary>
public static class UIExtend {

    public static Sequence DOJump(this RectTransform target, Vector3 endValue, int numJumps, float duration,
        bool snapping = false)
    {
        if (numJumps < 1) {
            numJumps = 1;
        }

        return DOTween.Sequence()
            .Append(DOTween.To(() => target.anchoredPosition3D, delegate(Vector3 x) {
                    target.anchoredPosition3D = x;
                }, new Vector3(endValue.x, 0f, 0f), duration)
                .SetOptions(AxisConstraint.X, snapping)
                .SetEase(Ease.Linear))
            .Join(DOTween.To(() => target.anchoredPosition3D, delegate(Vector3 x) {
                    target.anchoredPosition3D = x;
                }, new Vector3(0f, 0f, endValue.z), duration)
                .SetOptions(AxisConstraint.Z, snapping)
                .SetEase(Ease.Linear))
            .Join(DOTween.To(() => target.anchoredPosition3D, delegate(Vector3 x) {
                    target.anchoredPosition3D = x;
                }, new Vector3(0f, endValue.y, 0f), duration / (numJumps * 2))
                .SetOptions(AxisConstraint.Y, snapping)
                .SetEase(Ease.OutQuad)
                .SetLoops(numJumps * 2, LoopType.Yoyo))
            .SetTarget(target)
            .SetEase(DOTween.defaultEaseType);
    }

    //根据shader创建新的material
    public static void cloneSelfMaterial(this Image img)
    {
        if (img.material.name.Equals("cloneMat")) { } else {
            var mat = new Material(Shader.Find("Custom/UI/Base"));
            mat.name = "cloneMat";
            img.material = mat;
        }
    }

    public static void setToneIntensity(this Image image, float _toneIntensity)
    {
        _toneIntensity = Mathf.Clamp(_toneIntensity, -1f, 1f);
        image.material.SetFloat("_ToneIntensity", _toneIntensity);
    }

    public static void setSaturation(this Image image, float saturation)
    {
        saturation = Mathf.Clamp(saturation, 0f, 1f);
        image.material.SetFloat("_Saturation", saturation);
    }

    public static void setContrast(this Image image, float Contrast)
    {
        image.material.SetFloat("_Contrast", Contrast);
    }

    public static void setToneColor(this Image image, Color toneColor)
    {
        image.material.SetColor("_ToneColor", toneColor);
    }

    public static void resetDefaultImageEffect(this Image image)
    {
        image.setToneIntensity(0);
        image.setContrast(1);
        image.setSaturation(1);
        image.setToneColor(new Color(1, 1, 1, 1));
    }

}

}