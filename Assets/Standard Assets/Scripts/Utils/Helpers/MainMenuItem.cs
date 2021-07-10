//using DG.Tweening;
//using GameEngine.Extensions;
//using UnityEngine;
//using Sirenix.OdinInspector;
//using Sirenix.Utilities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using TMPro;
//using UnityEngine.Serialization;
//using UnityEngine.UI;
//using Utils.Helpers;
//
//namespace Main.Menu
//{
//    public class MainMenuItem : SerializedMonoBehaviour
//    {
//        [SerializeField]
//        Mask m_Mask;
//
//        Mask mask => m_Mask ??= GetComponent<Mask>();
//
//        Vector2 normalPos;
//        Vector2 activePos;
//        public static Dictionary<int, MainMenuItem> items = new Dictionary<int, MainMenuItem>();
//        //static bool inited;
//        public bool isActive;
//        int old = -1;
//        int id => transform.GetSiblingIndex();
//
//        void Awake()
//        {
//            try {
//                normalPos = iconNormal.rectTransform.anchoredPosition;
//                activePos = iconActive.rectTransform.anchoredPosition;
//            } catch (Exception e) {
//                Debug.Log(e.Message + " " + gameObject.name.ToRed(), gameObject);
//            }
//
//            Debug.Log($"{transform.GetSiblingIndex()}".ToGreen());
//            items[id] = this;
//
//            isActive = id == 2;
//
//            // if (!inited) {
//            //     inited = true;
//            //     SortPages.ScrollSnap.OnSelectionChangeStartEvent.AddListener(() => {
//            //         old = SortPages.ScrollSnap.CurrentPage;
//            //     });
//            //     SortPages.ScrollSnap.OnSelectionPageChangedEvent.AddListener(n => {
//            //         // if (old >= 0 && items.TryGetValue(old, out var t)) {
//            //         //     t.SetNormal();
//            //         // }
//            //         items.Values.ForEach(i => {
//            //             if (i.isActive && i.transform.GetSiblingIndex() != n) {
//            //                 i.SetNormal();
//            //             }
//            //         });
//            //
//            //         if (items.TryGetValue(n, out var target)) {
//            //             if (!target.isActive) {
//            //                 target.SetActive();
//            //             }
//            //         }
//            //     });
//            // }
//        }
//
//        [SerializeField]
//        public Button normal;
//
//        [SerializeField]
//        public Button current;
//
//        [FormerlySerializedAs("icon"), SerializeField]
//        public Image iconNormal;
//
//        [SerializeField]
//        public Image iconActive;
//
//        [SerializeField]
//        public TMP_Text caption;
//
//        void Reset()
//        {
//            m_Mask ??= GetComponent<Mask>();
//            iconActive ??= transform.Find("IconActive")?.GetComponent<Image>();
//            caption ??= GetComponentInChildren<TMP_Text>(true);
//            iconNormal ??= transform.Find("Icon")?.GetComponent<Image>();
//            current ??= transform.Find("Hover")?.GetComponent<Button>();
//            normal ??= transform.Find("Normal")?.GetComponent<Button>();
//
//            //normal = ((Image)GetComponent<Button>().targetGraphic)?.sprite;
//        }
//
//        public void Click()
//        {
//            Debug.Log(SortPages.ScrollSnap.CurrentPage.ToYellow());
//
//            if (SortPages.ScrollSnap.CurrentPage != transform.GetSiblingIndex()) {
//                // if (items.TryGetValue(SortPages.ScrollSnap.CurrentPage, out var old)) {
//                //     Debug.Log(old.gameObject.name);
//                //     old.SetNormal();
//                // }
//                items.Values.Where(t => t != this).ForEach(t => t.SetNormal());
//                SortPages.ScrollSnap.GoToScreen(transform.GetSiblingIndex());
//                SetActive();
//            }
//        }
//
//        [ButtonGroup("test")]
//        public void SetActive()
//        {
//            isActive = true;
//
//            //mask.enabled = true;
//            normal.gameObject.SetActive(false);
//
//            current.gameObject.SetActive(true);
//            caption.gameObject.SetActive(true);
//            iconActive.gameObject.SetActive(true);
//            iconNormal.gameObject.SetActive(false);
//
//            if (Application.isPlaying) {
//                iconActive.rectTransform.anchoredPosition = normalPos;
//                iconActive.rectTransform.DOAnchorPos(activePos, .5f).SetEase(Ease.OutQuad).OnComplete(() => { });
//            }
//        }
//
//        [ButtonGroup("test")]
//        public void SetNormal()
//        {
//            if (!isActive) return;
//            isActive = false;
//
//            //mask.enabled = false;
//            normal.gameObject.SetActive(true);
//            current.gameObject.SetActive(false);
//            caption.gameObject.SetActive(false);
//            iconNormal.gameObject.SetActive(true);
//            iconActive.gameObject.SetActive(false);
//
//            if (Application.isPlaying) {
//                iconNormal.rectTransform.anchoredPosition = activePos;
//                iconNormal.rectTransform.DOAnchorPos(normalPos, .5f).SetEase(Ease.OutQuad).OnComplete(() => { });
//            } else { }
//        }
//    }
//}