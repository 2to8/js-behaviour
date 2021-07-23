using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;
using Admin;
using Consts;
using GameEngine.Extensions;
using MoreTags.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tetris;

namespace UnityRoyale
{
    [SceneBind(SceneName.Main)]
    public class CardManager : Manager<CardManager>
    {
        
        public Camera mainCamera => Camera.main; //public reference
        public LayerMask playingFieldMask;
        public GameObject cardPrefab;
        public DeckData playersDeck;
        public MeshRenderer forbiddenAreaRenderer;
        public UnityAction<CardData, Vector3, Placeable.Faction> OnCardUsed;
        public GameObject prevCard;

        [FoldoutGroup("nextCard")]
        public RectTransform cardCover;

        public UnityAction<Card, int> OnCardCreated;
        bool m_LockCards;

        public bool LockCards {
            get => m_LockCards;
            set {
                m_LockCards = value;
                cards.ForEach(t => {
                    if (t != null && t.gameObject != null && t.enabled) t?.lockIcon.gameObject.SetActive(value);
                });
            }
        }

        [ShowInInspector]
        public int id { get; set; } = -1;

        [Header("UI Elements")]
        public RectTransform backupCardTransform; //the smaller card that sits in the deck

        public RectTransform cardsDashboard; //the UI panel that contains the actual playable cards

        public RectTransform cardsPanel;
        //the UI panel that contains all cards, the deck, and the dashboard (center aligned)

        Card[] m_Cards = new Card[3];

        //public Card[] cards;
        public Card[] cards {
            get => m_Cards ??= new Card[3];
            set => m_Cards = value;
        }

        private bool cardIsActive = false; //when true, a card is being dragged over the play field
        //private GameObject previewHolder;

        private Vector3 inputCreationOffset = new Vector3(0f, 0f, 1f);
        //offsets the creation of units so that they are not under the player's finger

        GameObject m_PreviewHolder;

        GameObject previewHolder =>
            m_PreviewHolder ??= GameObject.Find("/PreviewHolder") ?? new GameObject("PreviewHolder");

        public override void Start()
        {
            base.Start();
            m_PreviewHolder ??= new GameObject("PreviewHolder");
            cards ??= new Card[3]; //3 is the length of the dashboard
        }

        public void LoadDeck(bool allCards = true)
        {
            //gameObject.GetComponents<DeckLoader>().ForEach(t => t.DestroySelf());
            DeckLoader newDeckLoaderComp = gameObject.RequireComponent<DeckLoader>();
            newDeckLoaderComp.count = allCards ? cards.Length : 1;
            newDeckLoaderComp.OnDeckLoaded += DeckLoaded;
            newDeckLoaderComp.LoadDeck(playersDeck);
        }

        //...

        private void DeckLoaded(DeckLoader deckLoader)
        {
            Debug.Log("Player's deck loaded");
            LockCards = false;

            //setup initial cards
//            StartCoroutine(AddCardToDeck(.1f));
//            for (int i = 0; i < cards.Length; i++) {
//                StartCoroutine(PromoteCardFromDeck(i, .4f + i));
//                StartCoroutine(AddCardToDeck(.8f + i));
//            }
            if (deckLoader.count > 1) {
                //setup initial cards
                // 先放一张牌到牌堆里面
                //StartCoroutine(AddCardToDeck(.1f));
                AddCardToDeck(.1f);
                for (var i = 0; i < deckLoader.count; i++) {
                    // 闭包使用的循环变量必须缓存
                    var times = i;

                    // 从牌堆移动一张到卡池
                    //StartCoroutine(
                    PromoteCardFromDeck(times, .4f + times);

                    //);

                    // 再放一张到牌堆里面
                    //StartCoroutine(
                    AddCardToDeck(.8f + times, card => UseCard(deckLoader.count, times));
                }
            }
            else {
                UseCard();
                // 从牌堆移动一张到卡池
                //StartCoroutine(
                // PromoteCardFromDeck(0, .4f);
                // if (Core.Dialog("next", true)) {
                //     AddCardToDeck(.1f);
                // }
            }
        }

        void UseCard(int count = 1, int times = 1)
        {
            //if(cards)
            if (count > 1 && times < 2) return;
            Debug.Log($"deck loaded: {times}".ToRed());

            // Input.MousePosition 转ui坐标, 相对于当前物体
            var mousePosition = new Vector3(350f, 300f);

            // Vector3 screenPos = Camera.main.WorldToScreenPoint(mousePosition);
            // Vector2 screenPos2D = new Vector2(screenPos.x, screenPos.y);

            //Vector2 anchoredPos;
            // 每次都取第一张牌
            var cardTransform = cards[0].GetComponent<RectTransform>();

            // RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //     cardTransform.parent.GetComponent<RectTransform>(), mousePosition,
            //     /*Camera.main*/cardTransform.GetComponentInParent<Canvas>().worldCamera, out var anchoredPos);

            //buttonTransform.anchoredPosition = anchoredPos;
            TweenCallback action = () => {
                Debug.Log("first card".ToRed());
                Debug.Log($"[State] {Game.instance.State}");
                Status.instance.State = Game.GameState.Gamming;

                // 开始游戏
                Game.tetris.SetCurrentBlock(cards[0]);
                Game.instance.OnFirstBlock.Invoke();

                // 放置第一张牌
                CardReleased(0, mousePosition);
            };
            if (Application.isPlaying) {
                cardTransform.DOAnchorPos( /*new Vector2(125f, 14.5f)*/
                    cardTransform.ScreenToRectPos(mousePosition), .5f).SetEase(Ease.OutQuad).OnComplete(action);
            }
            else {
                cardTransform.anchoredPosition = cardTransform.ScreenToRectPos(mousePosition);
                action.Invoke();
            }
        }

        //moves the preview card from the deck to the active card dashboard
        private IEnumerator PromoteCardFromDeck(int position, float delay = 0f)
        {
            if (Application.isPlaying) {
                yield return new WaitForSeconds(delay);
            }

            backupCardTransform.SetParent(cardsDashboard, true);
            //move and scale into position
            if (Application.isPlaying) {
                backupCardTransform.DOAnchorPos(new Vector2(210f * (position + 1) - 55f /* + 20f*/, 0f),
                    .2f + (.05f * position)).SetEase(Ease.OutQuad);
            }
            else {
                backupCardTransform.anchoredPosition = new Vector2(210f * (position + 1) - 55f, 0f);
            }

            backupCardTransform.localScale = Vector3.one;

            //store a reference to the Card component in the array
            Card cardScript = backupCardTransform.GetComponent<Card>();
            cardScript.cardId = position;
            cards[position] = cardScript;

            //setup listeners on Card events
            cardScript.OnTapDownAction += CardTapped;
            cardScript.OnDragAction += CardDragged;
            cardScript.OnTapReleaseAction += CardReleased;
        }

        //adds a new card to the deck on the left, ready to be used
        private IEnumerator
            AddCardToDeck(float delay = 0f, Action<Card> onLoaded = null) //TODO: pass in the CardData dynamically
        {
            if (Application.isPlaying) {
                yield return new WaitForSeconds(delay);
            }

            //create new card
            cardsPanel.ClearChildTransforms(t => t.gameObject.activeInHierarchy && t.GetComponent<Card>());
            backupCardTransform = Instantiate<GameObject>(cardPrefab, cardsPanel).GetComponent<RectTransform>();
            backupCardTransform.localScale = Vector3.one * /*0.7*/0.85f;

            //send it to the bottom left corner
            backupCardTransform.anchoredPosition = new Vector2( /*180f*/125f, -300f);
            if (!Application.isPlaying)
                backupCardTransform.anchoredPosition = new Vector2(125f, 14.5f);
            else
                backupCardTransform.DOAnchorPos(new Vector2( /*180f*/125f, /*0f*/14.5f), .2f).SetEase(Ease.OutQuad);

            //populate CardData on the Card script
            Card cardScript = backupCardTransform.GetComponent<Card>();
            var cardData = playersDeck.GetNextCardFromDeck();
            cardScript.InitialiseWithData(cardData);

            // todo:sh 生成新的方块并设置显示位置
            id += 1;
            cardScript.id = id;
            instance.OnCardCreated?.Invoke(cardScript, cards.Length);
            //grid.Init();
            Game.spawner.NextSlot(cardScript);
            Debug.Log($"Generate new Card : {id} => {cardScript.nextBlock.id}".ToBlue());
            onLoaded?.Invoke(cardScript);
        }

        private void CardTapped(int cardId, Vector2 pos = default)
        {
            if (LockCards) {
                Debug.Log($"locked: {LockCards}");
                return;
            }

            //cards[cardId].GetComponent<RectTransform>().SetAsLastSibling();
            forbiddenAreaRenderer.enabled = true;
        }

        private void CardDragged(int cardId, Vector2 dragAmount)
        {
            if (LockCards) {
                Debug.Log($"locked: {LockCards}");
                return;
            }

            cards[cardId].transform.Translate(dragAmount);

            //raycasting to check if the card is on the play field
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            bool planeHit = Physics.Raycast(ray, out hit, Mathf.Infinity, playingFieldMask);
            if (planeHit) {
                m_OutSide = true;
                if (!cardIsActive) {
                    cardIsActive = true;
                    previewHolder.transform.position = hit.point;
                    cards[cardId].ChangeActiveState(true); //hide card

                    //retrieve arrays from the CardData
                    PlaceableData[] dataToSpawn = cards[cardId].cardData.placeablesData;
                    Vector3[] offsets = cards[cardId].cardData.relativeOffsets;

                    //spawn all the preview Placeables and parent them to the cardPreview
                    for (int i = 0; i < dataToSpawn.Length; i++) {
                        GameObject newPlaceable = GameObject.Instantiate<GameObject>(dataToSpawn[i].associatedPrefab,
                            hit.point + offsets[i] + inputCreationOffset, Quaternion.identity, previewHolder.transform);
                    }
                }
                else {
                    //temporary copy has been created, we move it along with the cursor
                    previewHolder.transform.position = hit.point;
                }
            }
            else {
                if (cardIsActive) {
                    cardIsActive = false;
                    cards[cardId].ChangeActiveState(false); //show card
                    ClearPreviewObjects();
                }
            }
        }

        public bool m_OutSide {
            get { return m_OutSide; }
            set { m_OutSide = value; }
        }

        private void CardReleased(int cardId, Vector2 pos = default)
        {
            if (LockCards) {
                Debug.Log($"locked: {LockCards}".ToRed());
                return;
            }

            //raycasting to check if the card is on the play field
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay( /*Input.mousePosition*/
                pos == default ? Input.mousePosition : new Vector3(pos.x, pos.y));
            // 270,258
            Debug.Log(Input.mousePosition.ToString().ToYellow());
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playingFieldMask)) {
                if (OnCardUsed != null)
                    OnCardUsed(cards[cardId].cardData, hit.point + inputCreationOffset,
                        Placeable.Faction.Player); //GameManager picks this up to spawn the actual Placeable
                ClearPreviewObjects();
                if (prevCard != null && prevCard != cards[cardId].gameObject) {
                    Debug.Log($"destroy prevCard {prevCard}".ToBlue());
                    //if (Application.isPlaying) {
                    Core.Destroy(prevCard); //remove the card itself
                    // }
                }

                //
                //Destroy(cards[cardId].gameObject); //remove the card itself
                prevCard = cards[cardId].gameObject;
                prevCard.SetActive(false);
                StartCoroutine(PromoteCardFromDeck(cardId, .2f));
                StartCoroutine(AddCardToDeck(.6f, card => {
                    // 新生成卡牌即下一回合, 清除锁定标志
                    m_OutSide = false;
                    LockCards = false;
                }));
            }
            else {
                // 不释放卡牌则替换当前方块为卡牌方块, 每回合只允许替换一次
                HoldCard(cardId);
                //cards[cardId].GetComponent<RectTransform>().DOAnchorPos(new Vector2(210f /*220f */ * (cardId + 1) - 55f, 0f), .2f).SetEase(Ease.OutQuad);
            }

            forbiddenAreaRenderer.enabled = false;
        }

        public void HoldCard(int slotId)
        {
            Game.tetris.HoldBlock(cards[slotId]);
            if (Application.isPlaying)
                cards[slotId].GetComponent<RectTransform>().DOAnchorPos(new Vector2(210f * (slotId + 1) - 55f, 0f), .2f)
                    .SetEase(Ease.OutQuad).OnComplete(() => {
                        // 锁定卡槽
                        if (m_OutSide) {
                            //LockCards = true;
                        }
                    });
            else //if (Core.Dialog("设置卡位置")) {
                cards[slotId].GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(210f * (slotId + 1) - 55f, 0f);
            //}
        }

        //happens when the card is put down on the playing field, and while dragging (when moving out of the play field)
        private void ClearPreviewObjects()
        {
            //destroy all the preview Placeables
            if (previewHolder == null) return;
            for (int i = 0; i < previewHolder.transform.childCount; i++) {
                Destroy(previewHolder.transform.GetChild(i).gameObject);
            }
        }
    }
}