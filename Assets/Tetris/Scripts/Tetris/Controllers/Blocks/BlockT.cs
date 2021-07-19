using GameEngine.Extensions;
using UnityEngine;

namespace Tetris.Blocks
{
    public class BlockT : Block
    {
        // 1 0 1
        // 0 0 0
        // 1 0 1
        Vector2Int[] GetPoints(bool withParent = true)
        {
            return new Vector2Int[4] {
                transform.XZ(withParent, 1, 1),
                transform.XZ(withParent, 1, -1),
                transform.XZ(withParent, -1, 1),
                transform.XZ(withParent, -1, -1)

                // new Vector2Int(TetrisUtil.Float2Int(transform.position.x + 1),
                //     TetrisUtil.Float2Int(transform.position.y + 1)),
                // new Vector2Int(TetrisUtil.Float2Int(transform.position.x + 1),
                //     TetrisUtil.Float2Int(transform.position.y - 1)),
                // new Vector2Int(TetrisUtil.Float2Int(transform.position.x - 1),
                //     TetrisUtil.Float2Int(transform.position.y + 1)),
                // new Vector2Int(TetrisUtil.Float2Int(transform.position.x - 1),
                //     TetrisUtil.Float2Int(transform.position.y - 1)),
            };
        }

        public bool IsTSpin(out bool isMini)
        {
            var points = GetPoints();
            var c1 = 0; // tspin
            var c2 = 0; // mini
            for (var i = 0; i < points.Length; i++) // out of range check
                if (points[i].x >= TetrisManager.Width || points[i].x < 0 ||
                    points[i].y >= TetrisManager.Height + TetrisManager.ExtraHeight || points[i].y < 0) {
                    c1++;
                    c2++;
                }
                else if (TetrisManager.instance.grid[points[i].x, points[i].y] != null) {
                    c1++;
                }

            isMini = c2 >= 2 ? true : false;
            if (c1 < 3) return false;
            return true;
        }

#if UNITY_EDITOR
        public override void DrawGizmos()
        {
            base.DrawGizmos();

            // draw block T corners
            Gizmos.color = Color.green;
            var points = GetPoints(false);
            for (var i = 0; i < points.Length; i++)
                Gizmos.DrawCube(transform.position + new Vector3Int(points[i].x, 0, points[i].y),
                    new Vector3(.6f, .6f));
        }
#endif
    }
}