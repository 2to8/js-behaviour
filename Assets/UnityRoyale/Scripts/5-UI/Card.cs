using System.Collections;
using System.Collections.Generic;
using Common;
using Tetris;
using Tetris.Blocks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityRoyale
{
    public class Card : View<Card>, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        public static Card Current => BlockSpawner.instance.currentCard;

        public RectTransform lockIcon;
        [FormerlySerializedAs("Index")]
        public TMP_Text IndexText;

        //[FormerlySerializedAs("cardId")]
        //[HideInInspector]
        //public int slotId;
        public int id { get; set; }
        public Block nextBlock { get; set; }
        public RawImage BlockFace;

        public UnityAction<int, Vector2> OnDragAction;
        public UnityAction<int, Vector2> OnTapDownAction, OnTapReleaseAction;

        [HideInInspector]
        public int cardId;

        [HideInInspector]
        public CardData cardData;

        public Image portraitImage; //Inspector-set reference
        private CanvasGroup canvasGroup;

        public override void Start()
        {
            base.Start();
            canvasGroup = GetComponent<CanvasGroup>();
            lockIcon?.gameObject.SetActive(false);
        }

        //called by CardManager, it feeds CardData so this card can display the placeable's portrait
        public void InitialiseWithData(CardData cData)
        {
            cardData = cData;
            portraitImage.sprite = cardData.cardImage;
            cData.card = this;
        }

        public void OnPointerDown(PointerEventData pointerEvent)
        {
            OnTapDownAction?.Invoke(cardId,  pointerEvent.delta);
        }

        public void OnDrag(PointerEventData pointerEvent)
        {
            OnDragAction?.Invoke(cardId, pointerEvent.delta);
        }

        public void OnPointerUp(PointerEventData pointerEvent)
        {
            OnTapReleaseAction?.Invoke(cardId, pointerEvent.delta);
        }

        public void ChangeActiveState(bool isActive)
        {
            canvasGroup.alpha = (isActive) ? .05f : 1f;
        }
    }
}