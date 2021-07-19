using DG.Tweening;
using FlowCanvas.Nodes;
using GameEngine.Extensions;
using UnityEngine;
using Sirenix.OdinInspector;

namespace MainScene.Menu
{
    public class FadeButton : SerializedMonoBehaviour
    {
        [SerializeField]
        CanvasGroup cg;

        Tweener t;

        [SerializeField]
        float Max = 1f;

        [SerializeField]
        float Speed = 0.2f;

        [SerializeField]
        float Min = 0.5f;

        void OnEnable()
        {
            cg ??= gameObject.RequireComponent<CanvasGroup>();
            if (cg != null) ToLittle();
        }

        void OnDisable()
        {
            cg?.DOFade(1f, 0);
        }

        void ToBig()
        {
            t = cg?.DOFade(Max, Speed).OnComplete(ToLittle);
        }

        void ToLittle()
        {
            t = cg?.DOFade(Min, Speed).OnComplete(ToBig);
        }
    }
}