// using GameEngine.Extensions;
// using GameEngine.Models.Contracts;
// using Sirenix.Utilities;
// using System.Collections.Generic;
// using System.Linq;
// using Tetris.Blocks;
// using UnityEngine;
// using UnityRoyale;
//
// namespace Tetris
// {
//     public partial class TetrisManager : DbTable<TetrisManager>
//     {
//         public void Init(BlockSpawner spawner, VFX vfx = null)
//         {
//             gameOver = false;
//
//             //grid = grid ??= new Grid();
//             m_spawner = spawner;
//
//             // m_spawner.InitNextChainSlot();
//            //Init(spawner);
//             m_VFX = vfx ?? m_VFX;
//         }
//
//     #region Block Control
//
//         /// <summary>
//         /// 当前方块替换为卡牌上的方块
//         /// </summary>
//         /// <param name="card"></param>
//         public void HoldBlock(Card card)
//         {
//             if (currentBlock == null) {
//                 return;
//             }
//
//             if (holdedThisTurn) {
//                 return;
//             }
//
//             //var old = currentBlock;
//             m_spawner.currentCard.gameObject.SetActive(true);
//             card.id = m_spawner.currentCard.id;
//             m_spawner.NextSlot(card, currentBlock);
//             m_spawner.NextBlock(card);
//
//             // originSize = currentBlock.transform.localScale;
//             //
//             // if (holded != null) {
//             //     var tmp = currentBlock;
//             //     currentBlock = holded;
//             //     holded = tmp;
//             //
//             //     currentBlock.transform.localPosition = new Vector3(Width / 2, Height);
//             //
//             //     // currentBlock.transform.localScale = originSize;
//             //
//             //     DestroyPreview();
//             //     InitPreview();
//             //     UpdatePreview();
//             // } else {
//             //     DestroyPreview();
//             //     holded = currentBlock;
//             //     NextBlock(card);
//             // }
//
//             m_VFX?.PlayClip(Consts.ClipHold);
//
//             // reset block state
//             moveDelta = MoveDelda.Normal;
//
//             holdedThisTurn = true;
//
//             //SetHold();
//             // holded.transform.localScale = new Vector3(sizeScale, sizeScale);
//             // holded.transform.rotation = Quaternion.Euler(0, 0, 0);
//             // holded.ResetState();
//         }
//
//         // public void SetHold()
//         // {
//         //     // if (holded == null) {
//         //     //     return;
//         //     // }
//         //     //
//         //     // if (holded is BlockO || holded is BlockI) {
//         //     //     holded.transform.localPosition = new Vector3(holdedViewPosX - holdedViewPosOffset, 0, holdedViewPosY);
//         //     // } else {
//         //     //     holded.transform.localPosition = new Vector3(holdedViewPosX, 0, holdedViewPosY);
//         //     // }
//         // }
//
//         public void MoveRight()
//         {
//             if (currentBlock == null || landed) {
//                 return;
//             }
//
//             if (currentBlock.MoveRight()) {
//                 landedWithRotate = false;
//                 lastWaitTime = 0;
//                 m_VFX?.PlayClip(Consts.ClipMove);
//                 UpdatePreview();
//             }
//         }
//
//         public void MoveLeft()
//         {
//             if (currentBlock == null || landed) {
//                 return;
//             }
//
//             if (currentBlock.MoveLeft()) {
//                 landedWithRotate = false;
//                 lastWaitTime = 0;
//                 m_VFX?.PlayClip(Consts.ClipMove);
//                 UpdatePreview();
//             }
//         }
//
//         public void ClockwiseRotation()
//         {
//             if (currentBlock == null || landed) {
//                 return;
//             }
//
//             if (currentBlock.ClockwiseRotation()) {
//                 landedWithRotate = true;
//                 lastWaitTime = 0;
//                 m_VFX?.PlayClip(Consts.ClipRotate);
//                 UpdatePreview();
//             }
//         }
//
//         public void AntiClockwiseRotation()
//         {
//             if (currentBlock == null || landed) {
//                 return;
//             }
//
//             if (currentBlock.AntiClockwiseRotation()) {
//                 landedWithRotate = true;
//                 lastWaitTime = 0;
//                 m_VFX?.PlayClip(Consts.ClipRotate);
//                 UpdatePreview();
//             }
//         }
//
//         public void SoftDrop()
//         {
//             if (currentBlock == null || landed) {
//                 return;
//             }
//             moveDelta = MoveDelda.SoftDrop;
//         }
//
//         public void NormalDrop()
//         {
//             if (currentBlock == null || landed) {
//                 return;
//             }
//             moveDelta = MoveDelda.Normal;
//         }
//
//         public void HardDrop()
//         {
//             if (currentBlock == null || landed) {
//                 return;
//             }
//             landedWithRotate = false;
//
//             while (currentBlock.MoveDown()) {
//                 ;
//             }
//             landed = true;
//
//             if (m_VFX) {
//                 m_VFX.VFX_HardDrop(currentBlock.transform.position);
//                 m_VFX.PlayClip(Consts.ClipHardDrop);
//             }
//         }
//
//         public void Fall(float deltaTime)
//         {
//             if (gameOver) {
//                 return;
//             }
//
//             time += deltaTime;
//             OnTimeChanged?.Invoke(time);
//
//             if (currentBlock == null) {
//                 return;
//             }
//
//             if (!landed) {
//                 if (!isLanding) {
//                     if (lastFallTime >= FallDeltaTime) {
//                         isLanding = !currentBlock.MoveDown();
//                         lastFallTime = 0;
//                     } else {
//                         lastFallTime += deltaTime;
//                     }
//                 } else {
//                     if (lastWaitTime >= waitLandTime) {
//                         landed = true;
//                         lastWaitTime = 0;
//                         m_VFX?.PlayClip(Consts.ClipSoftDrop);
//                     } else {
//                         isLanding = !currentBlock.MoveDown();
//                         lastWaitTime += Time.deltaTime;
//                     }
//                 }
//             } else {
//                 // trigger vfx before check(), because the currentBlock reference has been cleared after check() method.
//                 m_VFX?.PlayClip(Consts.ClipLanding);
//
//                 DestroyPreview();
//
//                 AddToGrid();
//
//                 if (gameOver) {
//                     return;
//                 }
//
//                 Check();
//
//                 moveDelta = MoveDelda.Normal;
//                 isLanding = false;
//                 landed = false;
//             }
//         }
//
//     #endregion
//
//     #region Check
//
//         public void AddToGrid() => currentBlock?.cells.ForEach(cell => grid[cell.coord] = cell);
//         /*{
//             var res = true;
//
//             foreach (Transform child in currentBlock.pieces) {
//                 var x = child.getIntX(true);
//                 var y = child.getIntZ(true);
//
//                 //
//                 // if (y < Height) {
//                 //     res = false;
//                 // }
//                 //
//                 // if (child.GetComponent<BoxCollider>()
//                 //     && x < grid.GetLength(0)
//                 //     && x >= 0
//                 //     && y < grid.GetLength(1)
//                 //     && y >= 0) {
//                 grid[x, y] = child;
//
//                 // }
//             }
//
//             // check game over
//             // if (res) {
//             //     OnGameOver?.Invoke();
//             //     gameOver = true;
//             //     currentBlock = null;
//             // }
//         }
//         */
//
//         // which line should clear
//         List<int> m_rows = new List<int>(Height + ExtraHeight);
//
//         public void Check()
//         {
//             // check T-Spin
//             if (currentBlock is BlockT blockT) {
//                 isTSpin = blockT.IsTSpin(out isMini) && landedWithRotate;
//                 Debug.LogFormat("roateted {0}, special {1}", landedWithRotate, isTSpin);
//             }
//
//             foreach (Transform child in currentBlock.pieces) {
//                 var y = child.getIntZ(true); //TetrisUtil.Float2Int(child.transform.position.y);
//
//                 if (!m_rows.Contains(y)) {
//                     m_rows.Add(y);
//                 }
//             }
//
//             // lock block move
//             currentBlock = null;
//
//             for (var i = m_rows.Count - 1; i >= 0; i--) {
//                 if (!HasLine(m_rows[i])) {
//                     m_rows.RemoveAt(i);
//                 } else {
//                     ClearLine(m_rows[i]);
//
//                     //OnLineClear?.Invoke(m_rows[i]);
//                     m_VFX?.VFX_LineClear(m_rows[i]);
//                 }
//             }
//
//             if (m_rows.Count > 0) {
//                 m_rows.Sort();
//
//                 // update score, etc.
//                 UpdateData(m_rows.Count);
//                 ren++;
//
//                 if (ren > 0) {
//                     m_VFX?.UpdateTextRen(ren);
//                 }
//
//                 Timer.Register(.3f, () => {
//                         for (var i = m_rows.Count - 1; i >= 0; i--) {
//                             DownLine(m_rows[i]);
//                         }
//
//                         SetCurrentBlock(CardManager.instance.cards[0]);
//
//                         m_rows.Clear();
//                     })
//                     .Start();
//             } else {
//                 SetCurrentBlock(CardManager.instance.cards[0]);
//                 m_rows.Clear();
//
//                 // clear combo data
//                 ren = -1;
//                 m_VFX?.HideTextRen();
//             }
//         }
//
//         public bool HasLine(int row)
//         {
//             for (var i = 0; i < Width; i++) {
//                 if (grid[i, row] == null) {
//                     return false;
//                 }
//             }
//
//             return true;
//         }
//
//         public void ClearLine(int row)
//         {
//             row = Mathf.Abs(row);
//
//             for (var i = 0; i < Width; i++) {
//                 if (grid[i, row] != null) {
//                     Destroy(grid[i, row].gameObject);
//
//                     //Grid[i, row].gameObject.SetActive(false);
//                     grid[i, row] = null;
//
//                     Debug.DrawLine(new Vector2(0, row), new Vector2(10, row), Color.red, 2);
//                 }
//             }
//         }
//
//         public void DownLine(int row)
//         {
//             row = Mathf.Abs(row);
//
//             for (var i = row; i < Height + ExtraHeight; i++) {
//                 for (var j = 0; j < Width; j++) {
//                     if (grid[j, i] == null) {
//                         continue;
//                     }
//
//                     grid[j, i - 1] = grid[j, i];
//                     grid[j, i] = null;
//                     grid[j, i - 1].MoveDown();
//                 }
//             }
//         }
//
//     #endregion
//
//     #region Block Preview
//
//         public void InitPreview()
//         {
//             // m_preview = Core.Instantiate(currentBlock).Of(t => t.setDynamicRoot("Preview"));
//             //
//             // foreach (Transform child in m_preview.transform) {
//             //     var sr = child.GetComponent<SpriteRenderer>();
//             //
//             //     if (sr) {
//             //         sr.color = new Color(1, 1, 1, .3f);
//             //         sr.sortingOrder = 15;
//             //     }
//             // }
//             //preview = GameManager.instance.PreviewRoot.GetComponent<Block>();
//             currentBlock?.pieces.Where(t => t != null)
//                 .ForEach((t, i) => {
//                     if (preview.pieces[i] != null) {
//                         preview.pieces[i].localPosition = t.localPosition;
//                     }
//                 });
//             preview.gameObject.SetActive(true);
//         }
//
//         public void UpdatePreview()
//         {
//             // if (!preview) {
//             //     return;
//             // }
//             preview.transform.localPosition = currentBlock.transform.localPosition;
//             preview.transform.rotation = currentBlock.transform.rotation;
//             LandingPoint(preview);
//         }
//
//         public void DestroyPreview()
//         {
//             // if (preview != null) {
//             preview.gameObject.SetActive(false);
//
//             //Destroy(m_preview.gameObject);
//             //}
//         }
//
//         public void LandingPoint(Block block)
//         {
//             while (block.MoveDown()) {
//                 ;
//             }
//         }
//
//     #endregion
//
//         public void UpdateData(int count)
//         {
//             var special = isTSpin || count == 4;
//
//             if (lastClearIsSpecial && special) {
//                 m_VFX.TextVFX_B2B();
//             }
//
//             lastClearIsSpecial = special;
//
//             var points = 0;
//
//             if (isTSpin) {
//                 if (isMini) {
//                     switch (count) {
//                         case 1 :
//                             points = 200 * level;
//                             m_VFX.TextVFX_TSpinMiniSingle();
//
//                             break;
//                         case 2 :
//                             points = 1200 * level;
//                             m_VFX.TextVFX_TSpinMiniDouble();
//
//                             break;
//                         default : break;
//                     }
//                 } else {
//                     switch (count) {
//                         case 1 :
//                             points = 800 * level;
//                             m_VFX.TextVFX_TSpinSingle();
//
//                             break;
//                         case 2 :
//                             points = 1200 * level;
//                             m_VFX.TextVFX_TSpinDouble();
//
//                             break;
//                         case 3 :
//                             points = 1600 * level;
//                             m_VFX.TextVFX_TSpinTriple();
//
//                             break;
//                         default : break;
//                     }
//                 }
//
//                 m_VFX.PlayClip(Consts.ClipSpecial);
//             } else {
//                 switch (count) {
//                     case 1 :
//                         points = 100 * level;
//                         m_VFX.PlayClip(Consts.ClipSingle);
//
//                         break;
//                     case 2 :
//                         points = 300 * level;
//                         m_VFX.PlayClip(Consts.ClipDouble);
//
//                         break;
//                     case 3 :
//                         points = 500 * level;
//                         m_VFX.PlayClip(Consts.ClipTriple);
//
//                         break;
//                     case 4 :
//                         points = 800 * level;
//                         m_VFX.PlayClip(Consts.ClipTetris);
//                         m_VFX.TextVFX_Tetris();
//
//                         break;
//                     default : break;
//                 }
//             }
//
//             if (special) {
//                 points = (int)(points * 1.5f);
//             }
//             score += points;
//
//             OnScoreChanged?.Invoke(score);
//
//             if (score / ((level + 1) * (level + 1) * (level + 1)) > 1000) {
//                 level++;
//                 OnLevelChanged?.Invoke(level);
//             }
//
//             line += count;
//             OnGoalChanged?.Invoke(line);
//         }
//     }
// }

