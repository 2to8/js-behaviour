using GameEngine.Attributes;
using GameEngine.Extensions;
using GameEngine.Models.Contracts;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using MainScene.Menu;
using NodeCanvas.Tasks.Actions;
using Tetris.Blocks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityRoyale;

namespace Tetris
{
    /// <summary>
    /// 7-bag random spawner
    /// </summary>
    ///
    [PreloadSetting]
    public class BlockSpawner : DbTable<BlockSpawner>
    {
        [SerializeField]
        Block[] m_bag;

        public GameObject[] Prefabs = new GameObject[4];
        LinkedList<Block> m_Next;
        public int totalBlock { get; set; } = -1;

        LinkedList<Block> m_BlockBag {
            get => m_Next ??= new LinkedList<Block>();
            set => m_Next = value;
        }

        LinkedList<Block> m_NextView;
        UnityAction OnStartFall;

        LinkedList<Block> m_SlotQueue {
            get => m_NextView ??= new LinkedList<Block>();
            set => m_NextView = value;
        }

        public BlockSpawner Create(Block[] blocks)
        {
            m_bag = blocks;
            m_BlockBag.Clear();
            m_SlotQueue.Clear();

            // two bags
            RandomGenerator();
            RandomGenerator();
            return this;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            //CardManager.instance.OnCardCreated += NextSlot;
        }

        public void Init()
        {
            totalBlock = -1;
            m_BlockBag.Clear();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            //CardManager.instance.OnCardCreated -= NextSlot;
        }

        /// <summary>
        /// 方块已放置
        /// 取出并生成下一个形状放到顶部
        /// </summary>
        /// <returns></returns>
        public void NextBlock(Card card)
        {
            Grid.instance.Init();
            // if (Game.tetris.grid == null) {
            //     Debug.Log($"Start Game".ToBlue());
            //
            //     //Game.instance.StartGame();
            //
            //     //Game.instance.state = Game.GameState.Gamming;
            //
            //     // if (m_VFX) {
            //     //     m_tetris.Init(m_Spawner.NewBlockSpawner(blocks), m_VFX);
            //     // } else {
            //     Game.tetris.Init(NewBlockSpawner(Game.instance.blocks));
            //
            //     //}
            // }

            //return;

            // if (m_BlockBag.Count <= 7) {
            //     // 少于等于7个则重新添加7个
            //     RandomGenerator();
            // }

            // OnStartFall = () => {
            Game.tetris.SetCurrentBlock(card);

            //Game.currentBlock = block;

            // return block;
            // };

            //return m_SlotQueue.First.Value;
        }

        Card m_CurrentCard;

        public Card currentCard {
            get => m_CurrentCard;
            set => m_CurrentCard = value;
        }

        /// <summary>
        /// 把7个形状打乱放到序列里面
        /// </summary>
        void RandomGenerator()
        {
            Shuffle();
            for (var i = 0; i < m_bag.Length; i++) // 第几个方块, 用来mod 5获取应该放置的预览位置
                // totalBlock += 1;

                //Debug.Log($"total block: {totalBlock}".ToRed());

                //var block = BlockPool.Instance.TryGetBlock(m_bag[i]);
                if (m_bag[i] != null)
                    m_BlockBag.AddLast(m_bag[i]);
        }

        public void StartFall()
        {
            OnStartFall?.Invoke();
        }

        #region Next Preview

        // [ShowInInspector]
        // public float startPosX = 11.3f;
        //
        // [ShowInInspector]
        // public float startPosY = 16.5f;

        [SerializeField]
        float leftOffset = -.25f;

        [SerializeField]
        public int distance = 2;

        [SerializeField]
        float sizeScale = .5f;

        /// <summary>
        /// 初始化槽位, 生成5个block用作牌面
        /// </summary>
        /// <param name="count"></param>
        [Button]
        public Block GenerateSlotBlock(int id, Block head = null)
        {
            Grid.instance.Init();
            //m_SlotQueue.Clear();

            //for (var i = 0; i < count; i++) {
            if (head == null) {
                if (m_BlockBag.Count <= m_bag.Length) {
                    RandomGenerator();
                    RandomGenerator();
                }

                head = m_BlockBag.First.Value;
                m_BlockBag.RemoveFirst();
            }

            //
            Debug.Log($"m_bag: {m_bag.Length} m_BlockBag: {m_BlockBag.Count} head: {head != null}");

            //Debug.Log(Core.Instantiate(head).GetType().FullName);
            Assert.IsNotNull(head, "head != null");
            var viewGO = Instantiate(head, this.GetDynamicRoot("MapRoot/NextPreview"));
            viewGO.SetColor();
            Debug.Log($"[Instantiate] {viewGO.name}".ToBlue(), viewGO);
            viewGO.id = id;
            viewGO.transform.parent.ClearChildTransforms(t =>
                t.gameObject != viewGO.gameObject && t.GetComponent<Block>() != null &&
                t.GetComponent<Block>().id % 5 == id % 5);
            SetBlockPosition(viewGO);
            viewGO.transform.localScale = Vector3.one;
            return viewGO;

            // m_SlotQueue.AddLast(viewGO);

            //head = head.Next;
            //}
        }

        // public void CreateCardFace(Card card, int id)
        // {
        //     NextSlot(card,);
        // }

        // public void SetSlotPos()
        // {
        //     m_next_view.ForEach((viewGO, i) => {
        //         SetBlockPosition(viewGO);
        //     });
        // }

        /// <summary>
        ///
        /// </summary>
        /// <param name="count"></param>
        public void NextSlot(Card card, Block head = null)
        {
            //return;

            // var count = 5;

            //if (!m_SlotQueue.Any()) {
            //    GenerateSlotBlock();
            //}

            // view list - remove the first node
            // 取备用队列第一个
            card.nextBlock = GenerateSlotBlock(card.id, head); //m_SlotQueue.First.Value; //.gameObject.DestroySelf();

            // 从备用队列里删除
            //m_SlotQueue.RemoveFirst();
            card.nextBlock.SetFace(card.BlockFace);
            card.IndexText.text = $"{card.nextBlock.id}";
            Debug.Log($"{card.nextBlock.id}".ToYellow());

            // // view list - head node
            // var head2 = m_SlotQueue.First;
            //
            // // next list - head node
            // var head = m_BlockQueue.First;
            //
            // while (head2 != null) {
            //     // move up the block
            //     head2.Value.SingleUp(distance);
            //
            //     head = head.Next;
            //     head2 = head2.Next;
            // }

            // add new block to end
            //var viewGO = Core.Instantiate(head.Value).Of(t => t.setDynamicRoot("Next"));

            // if (viewGO is BlockO || viewGO is BlockI) {
            //     viewGO.transform.localPosition =
            //         new Vector3(startPosX + leftOffset + distance * (count - 1), 0, startPosY);
            // } else {
            //     viewGO.transform.localPosition = new Vector3(startPosX + distance * (count - 1), 0, startPosY);
            // }

            //SetBlockPosition(viewGO);

            //viewGO.transform.localScale = new Vector3(sizeScale, sizeScale);
            //m_SlotQueue.AddLast(viewGO);
        }

        void SetBlockPosition(Block viewGO)
        {
            if (viewGO is BlockO || viewGO is BlockI)
                viewGO.transform.localPosition = new Vector3(leftOffset, 0, 5 * (viewGO.id % 5) * -1);
            else
                viewGO.transform.localPosition = new Vector3(0, 0, 5 * (viewGO.id % 5) * -1);
        }

        #endregion

        void Shuffle()
        {
            m_bag = m_bag.OrderBy(a => Guid.NewGuid()).ToArray();
            Debug.Log(m_bag.Select(t => t.blockType).Join().ToGreen());

            // for (var i = 0; i < m_bag.Length - 1; i++) {
            //     Swap(ref m_bag[i], ref m_bag[RandomInRange(i, m_bag.Length)]);
            // }
        }

        System.Random rnd = new System.Random();

        int RandomInRange(int min, int max)
        {
            return rnd.Next(min, max);
        }

        void Swap<T>(ref T a, ref T b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }

        /// <summary>
        /// 释放卡牌就是放置方块到场景
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="position"></param>
        /// <param name="type"></param>
        public void OnCardUsed(CardData cardData, Vector3 position, Placeable.Faction type)
        {
            NextBlock(cardData.card);
            Game.instance.State = Game.GameState.Gamming;

            //m_Spawner.StartFall();
        }
    }
}