using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utils.Scenes
{
    [Serializable]
    public class RectTransformData
    {
        [FormerlySerializedAs("index")]
        public int Index;

        public string Name;

        public Vector3 LocalPosition;
        public Vector2 AnchoredPosition;
        public Vector2 SizeDelta;
        public Vector2 AnchorMin;
        public Vector2 AnchorMax;
        public Vector2 Pivot;
        public Vector3 Scale;
        public Quaternion Rotation;
        public RectTransformData() { }

        public RectTransformData(GameObject go)
        {
            PullFromTransform(go.GetComponent<RectTransform>() ?? go.transform);
        }

        public RectTransformData(Transform transform)
        {
            PullFromTransform(transform);
        }

        public RectTransformData(RectTransform transform)
        {
            PullFromTransform(transform);
        }

        public void PullFromTransform(Transform transform)
        {
            // var rect = transform.GetComponentInParent<Canvas>();
            // if ( rect != null) {
            //     PullFromTransform(transform.GetComponent<RectTransform>());
            // }
            // else {
                Index = transform.GetSiblingIndex();
                LocalPosition = transform.localPosition;
                Scale = transform.localScale;
                Rotation = transform.localRotation;
                Name = transform.name;
                //}
        }

        public void PushToTransform(GameObject go)
        {
            PushToTransform(go.GetComponent<RectTransform>() ?? go.transform);
        }

        public void PushToTransform(Transform transform)
        {
            // if (transform.TryGetComponent<RectTransform>(out var rectTransform) && rectTransform != null) {
            //     PushToTransform(rectTransform);
            // }
            // else {
                transform.SetSiblingIndex(Index);
                transform.localPosition = LocalPosition;
                transform.localScale = Scale;
                transform.localRotation = this.Rotation;
                transform.name = this.Name;
                //}
        }

        public void PullFromTransform(RectTransform transform)
        {
            this.Index = transform.GetSiblingIndex();
            this.LocalPosition = transform.localPosition;
            this.AnchorMin = transform.anchorMin;
            this.AnchorMax = transform.anchorMax;
            this.Pivot = transform.pivot;
            this.AnchoredPosition = transform.anchoredPosition;
            this.SizeDelta = transform.sizeDelta;
            this.Rotation = transform.localRotation;
            this.Scale = transform.localScale;
        }

        public void PushToTransform(RectTransform transform)
        {
            transform.SetSiblingIndex(Index);
            transform.localPosition = this.LocalPosition;
            transform.anchorMin = this.AnchorMin;
            transform.anchorMax = this.AnchorMax;
            transform.pivot = this.Pivot;
            transform.anchoredPosition = this.AnchoredPosition;
            transform.sizeDelta = this.SizeDelta;
            transform.localRotation = this.Rotation;
            transform.localScale = this.Scale;
        }
    }
}