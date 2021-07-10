//using GameEngine.Extensions;
////using Main.Menu;
//using Sirenix.Serialization;
//using Sirenix.Utilities;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//
//namespace Utils.Helpers
//{
//    public class SortPages : MonoBehaviour
//    {
//        public static SortPages instance => m_instance ??= FindObjectOfType<SortPages>();
//
//        [SerializeField]
//        HorizontalScrollSnap m_ScrollSnap;
//
//        public static HorizontalScrollSnap ScrollSnap => instance.m_ScrollSnap;
//
//        [SerializeField]
//        List<Transform> pages;
//
//        static SortPages m_instance;
//
//        [SerializeField]
//        ScrollRect m_ScrollRect;
//
//        public ScrollRect scrollRect => m_ScrollRect ??= GetComponent<ScrollRect>();
//
//        void Awake()
//        {
//            m_instance = this;
//            var content = GetComponent<ScrollRect>().content.transform;
//
//            //content.Childs().OrderBy(t => t.name).ForEach((t, i) => t.SetSiblingIndex(i));
//            pages.ForEach((t, i) => {
//                t?.SetSiblingIndex(i);
//            });
//
//            if (Application.isPlaying) {
//                content.Childs(t => !pages.Contains(t)).ForEach(t => t.gameObject.DestroySelf());
//            }
//        }
//
//        // int old;
//        //
//        // public void OnChangeStart()
//        // {
//        //     old = ScrollSnap.CurrentPage;
//        // }
//
//        public void Onchanged(int n)
//        {
//            Debug.Log(MainMenuItem.items.Count());
//            MainMenuItem.items.Values.ForEach(i => {
//                if (i.isActive && i.transform.GetSiblingIndex() != n) {
//                    i.SetNormal();
//                }
//            });
//
//            if (MainMenuItem.items.TryGetValue(n, out var target)) {
//                if (!target.isActive) {
//                    target.SetActive();
//                }
//            }
//        }
//    }
//}