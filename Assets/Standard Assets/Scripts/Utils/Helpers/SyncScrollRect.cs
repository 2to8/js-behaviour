//using UnityEngine;
//using Sirenix.OdinInspector;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//
//namespace Utils.Helpers
//{
//    /// <summary>
//    /// 解决2个层级的ScrollView嵌套但是方向不同的情况
//    /// </summary>
//    public class SyncScrollRect : SerializedMonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
//    {
//        [SerializeField]
//        ScrollRect m_ScrollRect;
//
//        [SerializeField]
//        List<ScrollRect> rects = new List<ScrollRect>() { null };
//
//        [SerializeField]
//        bool displayDebug;
//
//        public ScrollRect scrollRect {
//            get {
//                var rect = rects.FirstOrDefault(t => t?.gameObject.activeInHierarchy == true);
//
//                if (rect != null) {
//                    return rect;
//                }
//
//                return m_ScrollRect ??= GetComponent<ScrollRect>();
//            }
//        }
//
//        /// <summary>
//        /// 上层的ScrollRect
//        /// </summary>
//        ScrollRect parentScrollRect = null;
//
//        Vector2 oldpos;
//
//        //事件传递
//        public void OnBeginDrag(PointerEventData eventData)
//        {
//            isCheckEnd = false;
//
//            if (displayDebug) {
//                Debug.Log("start drag");
//            }
//
//            if (shouldSendEvent) {
//                parentScrollRect.OnBeginDrag(eventData);
//                oldpos = eventData.position;
//            } else {
//                scrollRect.OnBeginDrag(eventData);
//            }
//        }
//
//        //事件传递
//        public void OnDrag(PointerEventData eventData)
//        {
//            if (shouldSendEvent) {
//                parentScrollRect.OnDrag(eventData);
//            } else {
//                scrollRect.OnDrag(eventData);
//            }
//
//            if (isCheckEnd) {
//                return;
//            }
//            isCheckEnd = true;
//            oldPos = Input.mousePosition;
//            StartCoroutine(Check());
//        }
//
//        //事件传递
//        public void OnEndDrag(PointerEventData eventData)
//        {
//            if (displayDebug) {
//                Debug.Log("end drag");
//            }
//
//            if (shouldSendEvent) {
//                parentScrollRect.OnEndDrag(eventData);
//
//                if (oldpos.x < eventData.position.x) {
//                    parentScrollRect.GetComponent<HorizontalScrollSnap>().PreviousScreen();
//                } else {
//                    parentScrollRect.GetComponent<HorizontalScrollSnap>().NextScreen();
//                }
//            } else {
//                scrollRect.OnEndDrag(eventData);
//            }
//        }
//
//        void Start()
//        {
//            parentScrollRect = SortPages.instance.scrollRect;
//        }
//
//        //事件过滤
//        bool shouldSendEvent = false;
//        Vector3 oldPos = Vector3.zero;
//
//        bool isCheckEnd = false;
//
//        IEnumerator Check()
//        {
//            yield return new WaitForSeconds(Time.deltaTime);
//            Vector3 temp = Input.mousePosition - oldPos;
//            shouldSendEvent = temp.x * temp.x > temp.y * temp.y;
//            scrollRect.enabled = !shouldSendEvent;
//
//            // if (shouldSendEvent) {
//            //     Debug.Log("sync", gameObject);
//            // }
//
//            //yield return null;
//        }
//    }
//}