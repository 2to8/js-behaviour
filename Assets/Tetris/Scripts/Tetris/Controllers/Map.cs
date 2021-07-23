using System;
using System.Collections.Generic;
using Consts;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Tetris.Managers
{
    [SceneBind(SceneName.Main)]
    public class Map : Manager<Map>
    {
        //[OdinSerialize]
        //public Dictionary<string, List<Vector2Int>> list = new Dictionary<string, List<Vector2Int>>();
        public GameObject mapRoot;
        public int bottom = 149;
        Grid grid => Grid.instance;

        [PropertyOrder(10)]
        public int[,] data = new int[9, 150];

        [ButtonGroup("1")]
        public void MapGen()
        {
            Clear();
            bottom = 149;
            Debug.Log($"rows: {grid.Count}");
            for (var row = 0; row < grid.Count; row++)
            for (var col = 0; col < grid.width; col++)
                if (grid[col, row] != null)
                    data[col, bottom - row] = grid[col, row].id;
        }

        public static void SetTop()
        {
            var offset = instance.CheckTop() - 129;
            //var offset = top - 129;
            // if (Core.Dialog($"moved lines: {offset} top: {top}" )) return;
            Game.instance.MovableRoot.localPosition = new Vector3Int(0, 0, offset - 13);
        }

        public int CheckTop()
        {
            for (var i = 0; i < 150; i++)
            for (var j = 0; j < 9; j++)
                if (data[j, i] != 0)
                    return i;
            return 149;
        }

        [ButtonGroup("1")]
        void Clear()
        {
            data = new int[9, 150];
        }
    }
}