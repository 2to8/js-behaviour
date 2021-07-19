using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tetris
{
    public class MovingPanel : ViewManager<MovingPanel>, IBeginDragHandler, IDragHandler, IPointerUpHandler,
        IEndDragHandler, IPointerDownHandler /*,IPointerUpHandler*/
    {
        [SerializeField]
        ScrollRect m_Panel;

        [ShowInInspector]
        Vector2 m_Location;

        [SerializeField]
        Button m_Button;

        bool isFirstPressed;
        Button btn => m_Button ??= GetComponent<Button>();
        bool isClick;
        bool isDrag;
        public ScrollRect panel => m_Panel ??= GetComponentInParent<ScrollRect>();
        public RectTransform rect => GetComponent<RectTransform>();

        void Start()
        {
            btn.onClick.AddListener(() => {
                if (isClick) Debug.Log($"clicked right: {Input.mousePosition.x > Screen.width / 2}");
            });
            ResetPos();
            isClick = false;
            isDrag = false;
            btn.enabled = true;
        }

        [ButtonGroup("test")]
        public void ResetPos()
        {
            panel.verticalNormalizedPosition = 0.5f;
            panel.horizontalNormalizedPosition = 0.5f;
        }

        public void OnChanged(Vector2 pos)
        {
            m_Location = pos;

            //isClick = false;

            //ResetPos();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Debug.Log("begin drag");
            panel.OnBeginDrag(eventData);
            btn.enabled = false;
            isDrag = true;
            isClick = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            panel.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log("end drag");
            panel.OnEndDrag(eventData);
            isClick = true;
            btn.enabled = true;
            isDrag = false;
            ResetPos();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Debug.Log("mouse up");
            //
            // if (!isDrag) {
            //     Debug.Log($"clicked right: {Input.mousePosition.x > Screen.width / 2}");
            //
            //     //btn.onClick.Invoke();
            // }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //if (!Application.isFocused) {
            // if (!isFirstPressed) {
            //     panel.OnBeginDrag(eventData);
            //     isFirstPressed = true;
            // }

            // }

            //
            //  Debug.Log("mouse down");
        }

        public void OnClick() { }
    }
}