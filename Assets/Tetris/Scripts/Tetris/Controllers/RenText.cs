using Common;
using TMPro;
using UnityEngine;

namespace Tetris
{
    public class RenText : View<RenText>
    {
        Saro.Tween tween;
        TMP_Text text;

        public void RestartTween(int count)
        {
            if (!tween) tween = GetComponent<Saro.Tween>();
            if (!text) text = GetComponent<TMP_Text>();
            text.text = $"Ren\n{count}";
            tween.ResetNormalizedTime();
            tween.enabled = true;
        }
    }
}