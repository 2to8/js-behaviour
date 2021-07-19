using GameEngine.Attributes;
using GameEngine.Extensions;
using GameEngine.Models.Contracts;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Tetris.Blocks;
using Tetris.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityRoyale;

namespace Tetris
{
    [PreloadSetting]
    public class TetrisManager : DbTable<TetrisManager>
    {
        // public Vector3 holdPlace;
        // public Vector3 nextPos;
        public bool UpdateNextPost;

        // [SerializeField]
        int m_Height { get; set; } = 20;

        public static int Height {
            get => instance.m_Height;
            set => instance.m_Height = value;
        }

        //public HashSet<GameObject> CellPrefabs = new HashSet<GameObject>() { null };

        [OdinSerialize]
        public Dictionary<BlockType, Vector2[]> blockFaceOffset = new Dictionary<BlockType, Vector2[]>();

        [SerializeField]
        float cardFaceHeight;

        //SerializeField]
        int m_Width { get; set; } = 9;

        public static int Width {
            get => instance.m_Width;
            set => instance.m_Width = value;
        }

        //[SerializeField]
        int m_ExtraHeight { get; set; } = 1;

        public static int ExtraHeight {
            get => instance.m_ExtraHeight;
            set => instance.m_ExtraHeight = value;
        }

        public float DeltaNormal = 1f;
        public float DeltaSoft = .03f;

        //public Transform[,] grid { get; set; }
        // Grid m_Grid;
        public Grid grid => Grid.instance;

        //public static Logger logger = new Logger();
        public int level = 1;
        public int score = 0;
        public int line = 0;
        public float time;
        public static Action<int> OnScoreChanged;
        public static Action<int> OnLevelChanged;
        public static Action<int> OnGoalChanged;
        public static Action<float> OnTimeChanged;
        public static Action OnGameOver;
        public static Action<Vector2> OnHardDrop;
        public static Action<int> OnLineClear;

        // auto fall delta time
        public float FallDeltaTime {
            get {
                switch (moveDelta) {
                    case MoveDelta.Normal: return DeltaNormal / level;
                    case MoveDelta.SoftDrop: return DeltaSoft / level;
                }

                return 0f;
            }
        }

        float lastFallTime;

        // check landed
        float waitLandTime = .4f;
        float lastWaitTime;
        bool isLanding = false;

        [NonSerialized]
        public bool landed = false;

        // special clear check
        bool landedWithRotate = false;
        bool isTSpin = false;
        bool isMini = false;
        bool lastClearIsSpecial = false;
        int ren = 0;
        MoveDelta moveDelta = MoveDelta.Normal;

        //Block m_CurrentBlock;

        public Block currentBlock {
            get => Game.currentBlock;
            set => Game.currentBlock = value;
        }

        public static float cardFaceOffset => instance.cardFaceHeight;
        public bool gameOver = false;
        Block holded;
        bool holdedThisTurn;
        BlockSpawner m_spawner => Game.spawner;
        VFX m_VFX;

        // block land point preview
        // [SerializeField]
        //Transform preview => Game.instance.PreviewRoot;

        enum MoveDelta
        {
            Normal,
            SoftDrop,
            HardDrop
        }

        // public void Init(BlockSpawner spawner)
        // {
        //     gameOver = false;
        //
        //     //grid = grid ??= new Grid();
        //     m_spawner = spawner;
        //
        //     // m_spawner.InitNextChainSlot();
        // }

        public void Init(BlockSpawner spawner, VFX vfx = null)
        {
            gameOver = false;

            //grid = grid ??= new Grid();
            Game.spawner = spawner;

            // m_spawner.InitNextChainSlot();
            //Init(spawner);
            m_VFX = vfx ?? m_VFX;
        }

        public void LandingPoint(Block block)
        {
            while (block.MoveDown()) ;
        }

        public void ClearFallBlock()
        {
            TetrisUtil.GetDynamicRoot("MapRoot/Block").ClearChildTransforms();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="card"></param>
        public void SetCurrentBlock(Card card)
        {
            // if (gameOver ) {
            //     Debug.Log("[GAMEOVER]".ToRed());
            //
            //     return;
            // }
            Assert.IsNotNull(card.nextBlock, "card.nextBlock != null");
            //currentBlock = card.nextBlock; //m_spawner.NextBlock();
            // 实例化牌堆第一张并在移动区顶端显示
            if (card.nextBlock.blockId <= 0)
                card.nextBlock.blockId = card.nextBlock.id > 0 ? card.nextBlock.id : Level.instance.blockId += 1;
            this.GetDynamicRoot("MapRoot/Block").ClearChildTransforms(t => t.gameObject.activeInHierarchy);
            var block = Instantiate(card.nextBlock);
            block.id = block.blockId = card.nextBlock.blockId;
            //Core.Dialog(block.id.ToString());
            Debug.Log($"{block.name} [block id]{block.id}".ToBlue(), block.gameObject);
            currentBlock = block;
            var bt = block.transform;
            ClearFallBlock();
            bt.setDynamicRoot("MapRoot/Block");
            // bt.parent.ClearChildTransforms(t => t.gameObject != block.gameObject);
            BlockSpawner.instance.currentCard = card;

            // var block = Core.Instantiate(card.nextBlock)
            //     .Of(b => {
            //         b.gameObject.setDynamicRoot("Block");
            //         b.transform.parent.ClearChildTransforms(t => t.gameObject != b.gameObject);
            //
            //         //b.transform.ClearChildTransforms(t => t.gameObject != b.gameObject);
            //     });

            //var block = m_next.First.Value;
            //m_BlockBag.RemoveFirst();

            //block.transform.position = new Vector3(Tetris.Width / 2,0, Tetris.Height);
            bt.localScale = Vector3.one;
            bt.localPosition = new Vector3Int(4, 0, 17);
            // Core.Dialog($"{bt.localPosition} bottom: {block.buttom}");
            // if (block.buttom < 16) {
            //     bt.localPosition += new Vector3Int(0, 0, 16 - block.buttom);
            // }

            //block.transform.localRotation = Quaternion.Euler(90f, 0, 0);

            // var n = 16;
            //
            // block.cells.Where(co => co.transform.getIntZ(true) < 16)
            //     .ForEach(co => {
            //         n = Mathf.Min(co.transform.getIntZ(true), n);
            //     });
            //
            // if (n < 16) {
            //     transform.localPosition += new Vector3(0, 0, 16 - n);
            // }
            Debug.Log($"draw card: {block.name}".ToGreen(), block.gameObject);

            //Debug.Log(currentBlock.gameObject.name, currentBlock.gameObject);

            //if (!Application.isPlaying) return;

            // check game over
            if (!currentBlock.MoveDown() && !currentBlock.ValidChild()) {
                // Debug.Log($"check down x={grid.GetLength(0)} y={grid.GetLength(1)}", currentBlock);
                //
                // foreach (Transform child in currentBlock.pieces) {
                //     var x = child.getIntX(true); // TetrisUtil.Float2Int(child.transform.position.x);
                //     var y = child.getIntZ(true); // TetrisUtil.Float2Int(child.transform.position.y);
                //     Debug.Log($"x={x}, y={y}");
                //
                //     if (x < grid.GetLength(0) && x >= 0 && y < grid.GetLength(1) && y >= 0 && grid[x, y] != null) {
                Debug.Log("[GameOver]".ToRed());
                gameOver = true;
                OnGameOver?.Invoke();
                //currentBlock = null;
                return;

                //     }
                // }
            }

            if (block.buttom < 16) bt.localPosition += new Vector3Int(0, 0, 16 - block.buttom);
            Debug.Log("[Falling]".ToGreen());
            Debug.Log($"{Game.instance.State}".ToBlue());
            Game.instance.State = Game.GameState.Gamming;
            Preview.instance.InitPreview();
            Preview.instance.UpdatePreview();

            //m_spawner.NextSlot();
            holdedThisTurn = false;
            landedWithRotate = false;
            isTSpin = false;
        }

        #region Block Control

        [SerializeField]
        public float holdedViewPosX = -2.4f;

        [SerializeField]
        public float holdedViewPosY = 16;

        [SerializeField]
        float holdedViewPosOffset = .3f;

        [FormerlySerializedAs("K_HardDrop")]
        public KeyCode[] k_HardDrop = {KeyCode.Space};

        [FormerlySerializedAs("K_Down")]
        public KeyCode[] k_Down = {KeyCode.DownArrow};

        [FormerlySerializedAs("K_Left")]
        public KeyCode[] k_Left = {KeyCode.LeftArrow};

        [FormerlySerializedAs("K_Right")]
        public KeyCode[] k_Right = {KeyCode.RightArrow};

        [FormerlySerializedAs("K_AntiClockwiseRotation")]
        public KeyCode[] k_AntiClockwiseRotation = {KeyCode.Z};

        [FormerlySerializedAs("K_ClockwiseRotation")]
        public KeyCode[] k_ClockwiseRotation = {KeyCode.UpArrow};

        [FormerlySerializedAs("K_Hold")]
        public KeyCode[] k_Hold = {KeyCode.RightShift};

        [SerializeField]
        float sizeScale = .6f;

        Vector3 originSize;
        Block backupBlock;

        /// <summary>
        /// 当前方块替换为卡牌上的方块
        /// </summary>
        /// <param name="card"></param>
        public void HoldBlock(Card card)
        {
            if (currentBlock == null) return;
            if (holdedThisTurn && Application.isPlaying) return;
            if (m_spawner == null) {
                Debug.Log($"spawner is null".ToYellow());
                return;
            }

            var oldCard = m_spawner.currentCard;
            var old = card.nextBlock;
            var id = card.id;
            // m_spawner.currentCard.gameObject.SetActive(true);
            card.nextBlock = oldCard.nextBlock;
            //return;
            card.id = oldCard.id;
            m_spawner.NextSlot(card, card.nextBlock);
            oldCard.nextBlock = old;
            oldCard.id = id;
            m_spawner.NextBlock(oldCard);

            // originSize = currentBlock.transform.localScale;
            //
            // if (holded != null) {
            //     var tmp = currentBlock;
            //     currentBlock = holded;
            //     holded = tmp;
            //
            //     currentBlock.transform.localPosition = new Vector3(Width / 2, Height);
            //
            //     // currentBlock.transform.localScale = originSize;
            //
            //     DestroyPreview();
            //     InitPreview();
            //     UpdatePreview();
            // } else {
            //     DestroyPreview();
            //     holded = currentBlock;
            //     NextBlock(card);
            // }
            m_VFX?.PlayClip(Consts.ClipHold);

            // reset block state
            moveDelta = MoveDelta.Normal;
            if (Application.isPlaying) holdedThisTurn = true;

            //SetHold();
            // holded.transform.localScale = new Vector3(sizeScale, sizeScale);
            // holded.transform.rotation = Quaternion.Euler(0, 0, 0);
            // holded.ResetState();
        }

        // public void SetHold()
        // {
        //     // if (holded == null) {
        //     //     return;
        //     // }
        //     //
        //     // if (holded is BlockO || holded is BlockI) {
        //     //     holded.transform.localPosition = new Vector3(holdedViewPosX - holdedViewPosOffset, 0, holdedViewPosY);
        //     // } else {
        //     //     holded.transform.localPosition = new Vector3(holdedViewPosX, 0, holdedViewPosY);
        //     // }
        // }

        public void MoveRight()
        {
            if (currentBlock == null || landed) return;
            if (currentBlock.MoveRight()) {
                landedWithRotate = false;
                lastWaitTime = 0;
                m_VFX?.PlayClip(Consts.ClipMove);
                Preview.instance.UpdatePreview();
            }
        }

        public void MoveLeft()
        {
            if (currentBlock == null || landed) return;
            if (currentBlock.MoveLeft()) {
                landedWithRotate = false;
                lastWaitTime = 0;
                m_VFX?.PlayClip(Consts.ClipMove);
                Preview.instance.UpdatePreview();
            }
        }

        public void ClockwiseRotation()
        {
            if (currentBlock == null || landed) return;
            if (currentBlock.ClockwiseRotation()) {
                landedWithRotate = true;
                lastWaitTime = 0;
                m_VFX?.PlayClip(Consts.ClipRotate);
                Preview.instance.UpdatePreview();
            }
        }

        public void AntiClockwiseRotation()
        {
            if (currentBlock == null || landed) return;
            if (currentBlock.AntiClockwiseRotation()) {
                landedWithRotate = true;
                lastWaitTime = 0;
                m_VFX?.PlayClip(Consts.ClipRotate);
                Preview.instance.UpdatePreview();
            }
        }

        public void SoftDrop()
        {
            if (currentBlock == null || landed) return;
            moveDelta = MoveDelta.SoftDrop;
        }

        public void NormalDrop()
        {
            if (currentBlock == null || landed) return;
            moveDelta = MoveDelta.Normal;
        }

        public void HardDrop(int amount = 1)
        {
            if (currentBlock == null || landed) return;
            landedWithRotate = false;
            while (currentBlock.MoveDown(amount)) ;
            landed = true;
            if (m_VFX) {
                m_VFX.VFX_HardDrop(currentBlock.transform.position);
                m_VFX.PlayClip(Consts.ClipHardDrop);
            }
        }

        public void Fall(float deltaTime)
        {
            if (gameOver && Application.isPlaying) {
                Debug.Log("GAMEOVER".ToRed());
                return;
            }

            time += deltaTime;
            OnTimeChanged?.Invoke(time);
            if (currentBlock == null) return;
            if (!landed) {
                if (!isLanding) {
                    if (lastFallTime >= FallDeltaTime) {
                        isLanding = !currentBlock.MoveDown();
                        lastFallTime = 0;
                    }
                    else {
                        lastFallTime += deltaTime;
                    }
                }
                else {
                    if (lastWaitTime >= waitLandTime) {
                        landed = true;
                        lastWaitTime = 0;
                        m_VFX?.PlayClip(Consts.ClipSoftDrop);
                    }
                    else {
                        isLanding = !currentBlock.MoveDown();
                        lastWaitTime += Time.deltaTime;
                    }
                }
            }
            else {
                // trigger vfx before check(), because the currentBlock reference has been cleared after check() method.
                m_VFX?.PlayClip(Consts.ClipLanding);
                //if (Core.Dialog("修改current block的parent", false)) {
                //}
                if (Core.Dialog("中断?", false)) return;
                Preview.instance.DestroyPreview();
                if (Core.Dialog("test current block", false)) return;
                if (gameOver && Application.isPlaying) {
                    Debug.Log("GameOver".ToRed());
                    return;
                }

                AddToGrid();

                //if (Core.Dialog("begin CheckFullLine()?", true)) return;//{
                CheckFullLine();
                //}
                moveDelta = MoveDelta.Normal;
                isLanding = false;
                landed = false;
                //if (Core.Dialog("begin LoadDeck?", true)) return;
                CardManager.instance.LoadDeck(false);
            }
        }

        void MoveCurrentToMap(int offset = 0)
        {
            var t = currentBlock.transform;
            var pos = t.localPosition;
            t.SetParent(Game.instance.MovableRoot);
            t.localScale = Vector3.one;
            t.localPosition += new Vector3(0, 0, offset);
            t.SetAsFirstSibling();
            currentBlock.pieces.ForEach(t => t.gameObject.SetActive(true));
        }

        #endregion

        #region Check

        public void AddToGrid()
        {
            Debug.Log($"current blockId: {currentBlock.blockId} id: {currentBlock.id}".ToGreen() +
                currentBlock.cells.Select(cell => $"({cell.line},{cell.col})").Join().ToBlue());
            var top = 129;
            // currentBlock.cells.ForEach((cell, i) => {
            //     // var row = Vector3Int.RoundToInt(cell.transform.localPosition).z +
            //     //           Vector3Int.RoundToInt(cell.transform.parent.localPosition).z;
            //     // grid[row][cell.col] = cell;
            //     // Debug.Log($"{i}={cell.row},{row}");
            //
            // });
            //Core.Dialog($"id: {currentBlock.id} top: {top}  bottom: {Map.instance.bottom}");
            MoveCurrentToMap();
            currentBlock.cells.ForEach((cell, i) => {
                if (cell.gpos.y < top) top = cell.gpos.y;
                var row = cell.row;
                //Vector3Int.RoundToInt(cell.transform.localPosition).z + Vector3Int.RoundToInt(cell.transform.parent.localPosition).z;
                grid[cell.col, row] = cell;
                Map.instance.data[cell.col, cell.gpos.y] = currentBlock.blockId;
                Debug.Log($"{i}={cell.row},{row}");
                //Map.instance.data[cell.col, cell.line] = currentBlock.blockId;
                // if (cell.line < top) {
                //     top = cell.line;
                // }
            });
            Map.SetTop();
            // var offset = Map.instance.CheckTop() - 129;
            // Game.instance.MovableRoot.localPosition = new Vector3Int(0, 0, offset - 13);
            //
            //var offset = top - 129;
            // if (Core.Dialog($"moved lines: {offset} top: {top}" )) return;
            //Map.instance.bottom = 149 - offset;
            //Map.instance.bottom = 149 + offset;
        }
        /*{
            var res = true;

            foreach (Transform child in currentBlock.pieces) {
                var x = child.getIntX(true);
                var y = child.getIntZ(true);

                //
                // if (y < Height) {
                //     res = false;
                // }
                //
                // if (child.GetComponent<BoxCollider>()
                //     && x < grid.GetLength(0)
                //     && x >= 0
                //     && y < grid.GetLength(1)
                //     && y >= 0) {
                grid[x, y] = child;

                // }
            }

            // check game over
            // if (res) {
            //     OnGameOver?.Invoke();
            //     gameOver = true;
            //     currentBlock = null;
            // }
        }
        */

        // which line should clear
        List<int> m_rows = new List<int>(21);

        public void CheckFullLine()
        {
            // check T-Spin
            if (currentBlock is BlockT blockT) {
                isTSpin = blockT.IsTSpin(out isMini) && landedWithRotate;
                Debug.LogFormat("[BlockT] Rotated {0}, special {1}", landedWithRotate, isTSpin);
            }

            foreach (var child in currentBlock.cells) {
                var y = child.row; // child.Z(true, 0);
                //TetrisUtil.Float2Int(child.transform.position.y);
                if (!m_rows.Contains(y)) m_rows.Add(y);
            }

            var s = currentBlock.cells.Select((t, i) => $"{i}={t.row}").Join();
            Debug.Log(s);

            // lock block move
            //currentBlock = null;

            //if (Core.Dialog($"currentBlock, rows: {m_rows.Count} -> {s}")) return;
            // if (Core.Dialog("下落测试")) return;
            for (var i = m_rows.Count - 1; i >= 0; i--)
                if (!HasLine(m_rows[i])) {
                    m_rows.RemoveAt(i);
                }
                else {
                    ClearLine(m_rows[i]);

                    //OnLineClear?.Invoke(m_rows[i]);
                    m_VFX?.VFX_LineClear(m_rows[i]);
                }

            Debug.Log(grid.Select((t, i) => $"{i}={t.Where(tt => tt != null).Count()}").Join());
            if (m_rows.Count > 0) {
                m_rows.Sort();
                // Core.Dialog($"lines: {m_rows.Count}");

                // update score, etc.
                UpdateData(m_rows.Count);
                ren++;
                if (ren > 0) m_VFX?.UpdateTextRen(ren);
                Timer.Register(.3f, () => {
                    for (var i = m_rows.Count - 1; i >= 0; i--) DownLine(m_rows[i]);

                    //if (Core.Dialog("[620] SetCurrentBlock(CardManager.instance.cards[0])")) return;
                    CardManager.instance.LoadDeck(false);
                    //SetCurrentBlock(CardManager.instance.cards[0]);
                    m_rows.Clear();
                }).Start();
            }
            else {
                //if (Core.Dialog("SetCurrentBlock(CardManager.instance.cards[0])")) return;
                CardManager.instance.LoadDeck(false);
                //SetCurrentBlock(CardManager.instance.cards[0]);
                m_rows.Clear();

                // clear combo data
                ren = -1;
                m_VFX?.HideTextRen();
            }
        }

        public bool HasLine(int row)
        {
            for (var i = 0; i < Width; i++)
                if (grid[i, row] == null)
                    return false;
            return true;
        }

        public void ClearLine(int row)
        {
            //if (Core.Dialog($"Clear line: {row}")) return;
            row = Mathf.Abs(row);
            for (var i = 0; i < Width; i++)
                if (grid[i, row] != null) {
                    Core.Destroy(grid[i, row].gameObject);

                    //Grid[i, row].gameObject.SetActive(false);
                    grid[i, row] = null;
                    Debug.DrawLine(new Vector2(0, row), new Vector2(10, row), Color.red, 2);
                }
        }

        public void DownLine(int row)
        {
            row = Mathf.Abs(row);
            Debug.Log($"[DownLine] row={row}");
            var t = 149 - row;
            //Core.Dialog($"downline={row}");
            for (var i = row; i < 150; i++)
            for (var j = 0; j < Width; j++) {
                Map.instance.data[j, 149 - i] = 149 - i - 1 >= 0 ? Map.instance.data[j, 149 - i - 1] : 0;
                if (Map.instance.data[j, 149 - i] > 0 && t < row) t = row;
                grid[j, i] = grid[j, i + 1];
                if (grid[j, i] == null) //Map.instance.data[j, 149 - i] = 0;
                    continue;
                grid[j, i].MoveDown();
                t = Mathf.Max(grid[j, i].row, t);
                if (false) {
                    if (grid[j, i] == null) //Map.instance.data[j, 149 - i] = 0;
                        continue;
                    /*
                        if (t != i) {
                            t = i;
                            Debug.Log(i);
                        }
                        */
                    grid[j, i - 1] = grid[j, i];
                    grid[j, i] = null;
                    grid[j, i - 1].MoveDown();
                }
            }

            //grid.FindIndex(t => t.Any(tn => tn != null));  
            // Core.Dialog(t.ToString(), t != 129);

            //
            // var offset = t - 129;
            //
            //var offset = Map.instance.CheckTop() - 129;
            //Game.instance.MovableRoot.localPosition = new Vector3Int(0, 0, offset - 13);
            //
            Map.SetTop();
            //Game.instance.PreviewRoot.localPosition += new Vector3Int(0, 0, 1);
            //Map.instance.bottom = 149 - offset;
            /*Map.instance.bottom += 3;
            Game.instance.MovableRoot.localPosition = Vector3Int.RoundToInt(Game.instance.MovableRoot.localPosition) +
                                                      new Vector3Int(0, 0, 2);*/
        }

        #endregion

        public void UpdateData(int count)
        {
            var special = isTSpin || count == 4;
            if (lastClearIsSpecial && special) m_VFX?.TextVFX_B2B();
            lastClearIsSpecial = special;
            var points = 0;
            if (isTSpin) {
                if (isMini)
                    switch (count) {
                        case 1:
                            points = 200 * level;
                            m_VFX?.TextVFX_TSpinMiniSingle();
                            break;
                        case 2:
                            points = 1200 * level;
                            m_VFX?.TextVFX_TSpinMiniDouble();
                            break;
                        default: break;
                    }
                else
                    switch (count) {
                        case 1:
                            points = 800 * level;
                            m_VFX?.TextVFX_TSpinSingle();
                            break;
                        case 2:
                            points = 1200 * level;
                            m_VFX?.TextVFX_TSpinDouble();
                            break;
                        case 3:
                            points = 1600 * level;
                            m_VFX?.TextVFX_TSpinTriple();
                            break;
                        default: break;
                    }

                m_VFX?.PlayClip(Consts.ClipSpecial);
            }
            else {
                switch (count) {
                    case 1:
                        points = 100 * level;
                        m_VFX?.PlayClip(Consts.ClipSingle);
                        break;
                    case 2:
                        points = 300 * level;
                        m_VFX?.PlayClip(Consts.ClipDouble);
                        break;
                    case 3:
                        points = 500 * level;
                        m_VFX?.PlayClip(Consts.ClipTriple);
                        break;
                    case 4:
                        points = 800 * level;
                        m_VFX?.PlayClip(Consts.ClipTetris);
                        m_VFX?.TextVFX_Tetris();
                        break;
                    default: break;
                }
            }

            if (special) points = (int) (points * 1.5f);
            score += points;
            OnScoreChanged?.Invoke(score);
            if (score / ((level + 1) * (level + 1) * (level + 1)) > 1000) {
                level++;
                OnLevelChanged?.Invoke(level);
            }

            line += count;
            OnGoalChanged?.Invoke(line);
        }

#if UNITY_EDITOR
        public void DrawGizmos()
        {
            //return;
            //Gizmos.color = Color.white;

            // if (grid != null) {
            //     for (var i = 0; i < grid.GetLength(0); i++) {
            //         for (var j = 0; j < grid.GetLength(1); j++) {
            //             if (grid[i, j] != null) {
            //                 Gizmos.DrawCube(grid[i, j].transform.position, new Vector2(.9f, .9f));
            //             }
            //         }
            //     }
            // }

            //if (currentBlock != null) {
            currentBlock?.DrawGizmos();

            //}
        }
#endif
    }
}