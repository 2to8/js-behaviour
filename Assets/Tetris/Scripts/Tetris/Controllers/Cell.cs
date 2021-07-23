using Common;
using NodeCanvas.Tasks.Actions;
using UnityEngine;
using Sirenix.OdinInspector;
using Tetris.Blocks;
using Tetris.Managers;
using UnityEngine.Serialization;
using UnityEngine.Video;

namespace Tetris
{
    public class Cell : View<Cell>
    {
        // 该方块的唯一id
        [ReadOnly]
        public int id;

        public int GetLine(int y)
        {
            return map.bottom - y;
        }

        // 方块的prefabId
        // [ReadOnly]
        public int colorId;
        Block m_Block;
        Block block => m_Block ??= GetComponentInParent<Block>();

        [ShowInInspector]
        public Vector2Int pos => new Vector2Int(col, row);
        // Vector3Int.RoundToInt(transform.localPosition) + /*Vector3Int.RoundToInt(transform.parent.localPosition)+*/
        // (transform.parent == Game.instance.MovableRoot
        //     ? Vector3Int.zero
        //     : Vector3Int.RoundToInt(transform.parent.localPosition) /*+
        //       new Vector3Int(CheckRotate().x, 0, CheckRotate().y)*/);

        [ShowInInspector]
        int state => block?.state ?? -1;

        [ShowInInspector]
        public int line => GetLine(row);

        [ShowInInspector]
        public int px =>
            transform.parent != null && transform.parent != Game.instance.MovableRoot
                ? Vector3Int.RoundToInt(transform.parent.localPosition).x -
                Vector3Int.RoundToInt(Game.instance.MovableRoot.localPosition).x
                : 0;

        [ShowInInspector]
        public int py =>
            transform.parent != null && transform.parent != Game.instance.MovableRoot
                ? Vector3Int.RoundToInt(transform.parent.localPosition).z -
                (transform.parent?.parent == Game.instance.MovableRoot
                    ? 0
                    : Vector3Int.RoundToInt(Game.instance.MovableRoot.localPosition).z)
                : 0;

        [ShowInInspector]
        [ReadOnly]
        public int m_OldState { get; set; } = -2;

        [ShowInInspector]
        [ReadOnly]
        int tx = 0;

        [ShowInInspector]
        [ReadOnly]
        int ty = 0;

        (int x, int y) CheckRotate()
        {
            if (m_OldState != state) {
                tx = offsetX;
                ty = offsetZ;
                for (var i = 0; i < state; i++) {
                    var p = tx;
                    tx = /*1 - (ty - (block.maxEdge - 2)); //*/-ty;
                    ty = p;
                }

                m_OldState = state;
            }

            return (tx, ty);
        }

        [ShowInInspector]
        public int row => py + CheckRotate().y;

        [ShowInInspector]
        public int col => px + CheckRotate().x;

        [ShowInInspector]
        public int offsetX => Vector3Int.RoundToInt(transform.localPosition).x;

        [ShowInInspector]
        public int offsetZ => Vector3Int.RoundToInt(transform.localPosition).z;

        [ShowInInspector]
        public Vector2Int gpos => new Vector2Int(pos.x, 149 - pos.y);

        public Vector2Int coord => new Vector2Int(col, row);

        public Vector3 MoveDown()
        {
            var t = new Vector3Int(0, 0, -1);
            switch (state) {
                case 1:
                    t = new Vector3Int(-1, 0, 0);
                    break;
                case 2:
                    t = new Vector3Int(0, 0, 1);
                    break;
                case 3:
                    t = new Vector3Int(1, 0, 0);
                    break;
            }

            return transform.localPosition = Vector3Int.RoundToInt(transform.localPosition) + t;
        }

        // public static bool operator ==(Cell left, Cell right)
        // {
        //     if (left is null || !left.enabled) return right is null || !right.enabled;
        //
        //     return left.Equals(right);
        // }
        //
        // public static bool operator !=(Cell left, Cell right) => !(left == right);
        void Start() { }
    }
}