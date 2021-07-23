using GameEngine.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Tetris.Managers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils.Scenes;
using Random = UnityEngine.Random;

public enum BlockType
{
    I = 0,
    J,
    L,
    O,
    S,
    T,
    Z
}

namespace Tetris.Blocks
{
    public class Block : View<Block> /*,Saro.IPoolable<Block>*/
    {
        public BlockType blockType = BlockType.I;
        public int rotateType;
        public int maxEdge = 4;
        public int colorId = -1;

        [ShowInInspector]
        public int x => Vector3Int.RoundToInt(transform.localPosition).x;

        [ShowInInspector]
        public int y => Vector3Int.RoundToInt(transform.localPosition).z;

        [OdinSerialize]
        [NonSerialized]
        public HashSet<Cell> cells = new HashSet<Cell>() {null};

        public static Dictionary<BlockType, HashSet<Block>> all = new Dictionary<BlockType, HashSet<Block>> {
            [BlockType.I] = new HashSet<Block>(),
            [BlockType.J] = new HashSet<Block>(),
            [BlockType.L] = new HashSet<Block>(),
            [BlockType.O] = new HashSet<Block>(),
            [BlockType.S] = new HashSet<Block>(),
            [BlockType.T] = new HashSet<Block>(),
            [BlockType.Z] = new HashSet<Block>()
        };

        public void SetColor(int cid = -1)
        {
            colorId = cid < 0 ? colorId < 0 ? Random.Range(0, 4) : colorId : cid;
            Assert.IsTrue(Game.spawner.Prefabs.Length == 4, "Game.spawner.Prefabs.Length == 4");
            var prefab = Game.spawner.Prefabs[colorId];
            for (var i = 0; i < 4; i++) {
                var go = Instantiate(prefab, transform).transform;
                new RectTransformData(pieces[i]).PushToTransform(go);
                pieces[i].DestroySelf();
                pieces[i] = go;
            }

            cells.Clear();
            pieces.ForEach(t => cells.Add(t.GetComponent<Cell>()));
            transform.ClearChildTransforms(t => !t.gameObject.activeInHierarchy);
        }

        [SerializeField]
        public Transform[] pieces = new Transform[4];

        [ShowInInspector]
        public Vector2 faceOffset {
            get {
                if (!TetrisManager.instance.blockFaceOffset.TryGetValue(blockType, out var offset) || offset == null)
                    TetrisManager.instance.blockFaceOffset[blockType] = offset = new Vector2[4];
                return offset[rotateType];
            }
            set {
                if (faceOffset != value) {
                    TetrisManager.instance.blockFaceOffset[blockType][rotateType] = value;
                    all[blockType].Where(t => t != null).ForEach(block => block.SetFace());
                }
            }
        }

        [SerializeField]
        protected Vector3 rotatePoint;

        RawImage faceImage { get; set; }

        [ShowInInspector]
        public int top =>
            cells.Where(c => c != null).OrderByDescending(c => c.row).Select(c => c.row - 13).FirstOrDefault();

        [ShowInInspector]
        public int buttom => cells.Where(c => c != null).OrderBy(c => c.row).Select(c => c.row - 13).FirstOrDefault();

        // [SerializeField]
        // public Vector2Int[] rotateOffset = new Vector2Int[4];
        //
        // [FormerlySerializedAs("currentDir"),SerializeField]
        // public int direction;
        Vector3 axis = new Vector3(0, 0, 1);

        Vector3Int[] m_direction = new Vector3Int[] {
            // left
            new Vector3Int(-1, 0, 0),

            // right
            new Vector3Int(1, 0, 0),

            // down
            new Vector3Int(0, 0, -1),

            // up
            new Vector3Int(0, 0, 1)
        };

        //      0
        //   3     1
        //      2
        //
        // 0 - spawn state
        // 1 - rotate right
        // 2 - 2 successive rotations in either direction form spawn
        // 3 - rotate left
        [ShowInInspector]
        public int state { get; set; } = 0;

        [ShowInInspector]
        [ReadOnly]
        public int id { get; set; } = -1;

        [ShowInInspector]
        public int blockId { get; set; }

        // rotate right
        public virtual bool ClockwiseRotation()
        {
            axis = transform.up;
            transform.RotateAround(transform.TransformPoint(rotatePoint), axis, -90);
            transform.localPosition = new Vector3(x, 0, y);
            var _state = state + 1 > 3 ? 0 : state + 1;

            // kick wall
            if (!ValidChild()) {
                if (WallKickTest(_state, out var result)) {
                    Debug.Log(result);
                    SingleMove(result.x, result.y);
                    state = _state;
                    return true;
                }
                else {
                    transform.RotateAround(transform.TransformPoint(rotatePoint), axis, 90);
                    return false;
                }
            }

            Debug.LogFormat("{0} , {1}", state, _state);
            state = _state;
            return true;
        }

        // rotate left
        public virtual bool AntiClockwiseRotation()
        {
            axis = transform.up;
            transform.RotateAround(transform.TransformPoint(rotatePoint), axis, 90);
            transform.localPosition = new Vector3(x, 0, y);
            var _state = state - 1 < 0 ? 3 : state - 1;
            if (!ValidChild()) {
                if (WallKickTest(_state, out var result)) {
                    Debug.Log(result);
                    SingleMove(result.x, result.y);
                    state = _state;
                    return true;
                }
                else {
                    transform.RotateAround(transform.TransformPoint(rotatePoint), axis, -90);
                    return false;
                }
            }

            Debug.LogFormat("{0} , {1}", state, _state);
            state = _state;
            return true;
        }

        public bool MoveLeft(int amount = 1)
        {
            transform.localPosition += (Vector3Int) m_direction[0] * amount;
            if (!ValidChild()) {
                transform.localPosition -= (Vector3Int) m_direction[0] * amount;
                return false;
            }

            return true;
        }

        public bool MoveRight(int amount = 1)
        {
            transform.localPosition += (Vector3Int) m_direction[1] * amount;
            if (!ValidChild()) {
                transform.localPosition -= (Vector3Int) m_direction[1] * amount;
                return false;
            }

            return true;
        }

        public bool MoveDown(int amount = 1)
        {
            transform.localPosition += (Vector3Int) (m_direction[2] * amount);
            if (!ValidChild()) {
                transform.localPosition -= (Vector3Int) (m_direction[2] * amount);
                return false;
            }

            return true;
        }

        public void SingleMove(int x, int y)
        {
            transform.localPosition += new Vector3(x, 0, y);
        }

        public void SingleUp(int amount = 1)
        {
            transform.localPosition += (Vector3Int) m_direction[3] * amount;
        }

        public void ResetState()
        {
            state = 0;
        }

        public bool DisableBlock()
        {
            var res = transform.childCount > 0 ? false : true;
            if (res) gameObject.DestroySelf();
            return res;
        }

        protected virtual Vector2Int[] GetWallKickData(int next)
        {
            switch (state) {
                case 0:
                    if (next == 1) return WallKickData.Other[0];
                    if (next == 3) return WallKickData.Other[7];
                    break;
                case 1:
                    if (next == 0) return WallKickData.Other[1];
                    if (next == 2) return WallKickData.Other[2];
                    break;
                case 2:
                    if (next == 1) return WallKickData.Other[3];
                    if (next == 3) return WallKickData.Other[4];
                    break;
                case 3:
                    if (next == 2) return WallKickData.Other[5];
                    if (next == 0) return WallKickData.Other[6];
                    break;
                default: break;
            }

            return null;
        }

        bool WallKickTest(int next, out Vector2Int result)
        {
            var data = GetWallKickData(next);
            for (var i = 0; i < data.Length; i++)
                if (!ValidChild(data[i])) {
                    continue;
                }
                else {
                    result = data[i];
                    return true;
                }

            result = Vector2Int.zero;
            return false;
        }

        public bool ValidChild()
        {
            return ValidChild(Vector2.zero);
        }
        // {
        //     foreach (Transform child in transform) {
        //         var x = child.getIntX(true);
        //         var y = child.getIntZ(true);
        //
        //         if (!Valid(x, y)) {
        //             Debug.Log($"[BlockCheck] {this.name} x: {x},Y: {y} not valid".ToRed(), this.gameObject);
        //             return false;
        //         }
        //     }
        //
        //     return true;
        // }

        bool ValidChild(Vector2 move)
        {
            foreach (var child in cells) {
                var x = child.col + Vector2Int.RoundToInt(move).x;
                // child.getIntX(true) + (int) move.x; //TetrisUtil.Float2Int(child.position.x + move.x);
                var y = child.row + Vector2Int.RoundToInt(move).y;
                // child.getIntZ(true) + (int) move.y; //TetrisUtil.Float2Int(child.position.y + move.y);
                if (!Valid(x, y, child)) {
                    var status = cells.Select(t => $"({t.line},{t.col})").Join();
                    var ase = cells.Select(t => $"({t.row},{t.col})").Join();
                    Debug.Log(
                        $"[BlockCheck] {name.Replace("(Clone)", "")} x: {x},Y: {y} not valid " + status.ToRed() +
                        ase.ToBlue(), gameObject);
                    return false;
                }
            }

            return true;
        }

        // Grid grid => Grid.instance;
        // Map map => Map.instance;

        bool Valid(int x, int y, Cell cell)
        {
            if (x >= grid.width || x < 0 || y >= 149 || y < 0) {
                Debug.Log($"xy not valid, x={x},y={y}");
                return false;
            }

            var line = cell.GetLine(y);
            if (map.data[x, line] > 0) {
                Debug.Log($"grid count: {grid.Count} line: {line} col: {x} row: {y} not valid", grid[x, y]?.gameObject);
                return false;
            }

            //            Debug.Log($"x: {x} y: {map.bottom - grid.Count - y + 8}");
            return true;
        }

        // bool Valid(Vector2Int position) => Valid(position.x, position.y);
#if UNITY_EDITOR
        public virtual void DrawGizmos()
        {
            // draw pivot
            if (this == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(rotatePoint), .15f);
        }
#endif
        public void SetFace(RawImage image = null)
        {
            faceImage = image ??= faceImage;
            var rect = faceImage.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-15, -1070 + 200 * (id % 5));
        }
    }
}