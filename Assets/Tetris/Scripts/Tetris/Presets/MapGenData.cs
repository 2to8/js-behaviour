using GameEngine.Attributes;
using GameEngine.Models.Contracts;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Presets
{
    [PreloadSetting,CreateAssetMenu(menuName = "Data/"+nameof(MapGenData))]
    public class MapGenData : DbTable<MapGenData>
    {
        public bool locked;

        [OdinSerialize]
        public int[,] data = new int[9, 21];

        //public  Transform[,] transform { get; set; } = new Transform[9, 21];
        public Vector2Int size => new Vector2Int(data.GetLength(0), data.GetLength(1));

        [OdinSerialize]
        [NonSerialized]
        public List<Color> colors = new List<Color>();

        [ReadOnly]
        public int last;

        [ReadOnly]
        public int current;

        [Button]
        [ButtonGroup]
        void AddCurrent()
        {
            current += 1;
        }

        [Button]
        [ButtonGroup]
        void SubCurrent()
        {
            current -= 1;
        }

        public bool CheckRange(int x, int y)
        {
            return Mathf.Clamp(x, 0, size.x - 1) == x && Mathf.Clamp(y, 0, size.y - 1) == y;
        }

        public int this[int x, int y] {
            get => CheckRange(x, y) ? data[x, y] : 0;
            set {
                if (CheckRange(x, y)) data[x, y] = value;
            }
        }

        public Dictionary<int, Dictionary<int, int>> GetByRowCol {
            get {
                var tmp = new Dictionary<int, Dictionary<int, int>>();
                for (var row = 0; row < data.GetLength(1); row++) {
                    tmp[row] = new Dictionary<int, int>();
                    for (var col = 0; col < data.GetLength(0); col++) tmp[row][col] = data[col, row];
                }

                return tmp.OrderBy(k => k.Key).ToDictionary(o => o.Key, p => p.Value);
            }
        }
    }
}