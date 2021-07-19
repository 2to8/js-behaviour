// using Arena;
// using GameEngine.Attributes;
// using GameEngine.Models.Contracts;
// using Presets;
// using Sirenix.OdinInspector;
// using Sirenix.Utilities;
// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace UnityRoyale
// {
//     [Serializable]
//     public enum CellType
//     {
//         Stone1 = 0, Stone2, Wood1, Wood2, None, Warrior,
//     }
//
//     [Serializable]
//     public class Cell
//     {
//         public CellType field = CellType.None;
//         public Transform transform;
//         public bool isEmpty;
//         public int blockIndex;
//         public int sharpIndex;
//         public int craftIndex;
//     }
//
//     [PreloadSetting]
//     public class GridData : DbTable<GridData>
//     {
//         // area, row, col
//         readonly LinkedList<Cell[]> area = new LinkedList<Cell[]>();
//
//         public int top => (int)GridManager.instance.Movable.transform.localPosition.z;
//         public static LinkedList<Cell[]> map => TetrisPreset.instance.lines;
//
//         [FormerlySerializedAs("topmax")]
//         public int topMin = -13;
//
//         public MapGenData genData;
//
//         public int colNum = 9;
//
//         //public List<Cell[]> fallArea = new List<Cell[]>();
//
//     // #if UNITY_EDITOR
//     //
//     //     [Button]
//     //     public void AddArea()
//     //     {
//     //         //var add = new List<Cell[]>();
//     //         TetrisPreset.instance.Create();
//     //         map.ForEach(t => {
//     //             area.AddLast(t);
//     //         });
//     //
//     //         // genData.GetByRowCol.ForEach(tk => {
//     //         //     var line = new Cell[9];
//     //         //     tk.Value.ForEach(k => {
//     //         //         line[k.Key] = new Cell() {
//     //         //             transform = genData.transform[k.Value, genData.data[k.Value, k.Key]],
//     //         //             blockIndex = genData.data[k.Value, k.Key]
//     //         //         };
//     //         //     });
//     //         //     add.Add(line);
//     //         // });
//     //         // area.AddLast(add);
//     //     }
//     // #endif
//
//         // public void initGrid()
//         // {
//         //     area.Clear();
//         //     area.AddFirst(fallArea);
//         //     AddArea();
//         // }
//
//         // public Transform this[int x, int y] {
//         //     get { }
//         //     set { }
//         // }
//     }
// }

