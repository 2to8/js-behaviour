using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Arena;
using Battle;
using Data;
using GameEngine.Extensions;
using GameEngine.Kernel;
using MainScene.BootScene.Utils;
using Org.BouncyCastle.Asn1.Cmp;
using Presets;
using Sirenix.Utilities;
using Tetris.Blocks;
using Tetris.Managers;
using UnityEditor;
using UnityEngine;
using UnityRoyale;
using Random = UnityEngine.Random;

namespace Tetris
{
    [Serializable]
    public class Grid : IList<Cell[]>
    {
        static Grid m_Instance;

        public static Grid instance {
            get {
                if (m_Instance == null) {
                    m_Instance = new Grid();
                    Level.instance._加载关卡();
                }

                return m_Instance;
            }
            private set => m_Instance = value;
        }

        readonly List<Cell[]> list = new List<Cell[]>();

        public IEnumerator<Cell[]> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Cell[] item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(Cell[] item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Cell[][] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(Cell[] item)
        {
            return list.Remove(item);
        }

        public int Count => list.Count;
        public bool IsReadOnly => false;

        public int IndexOf(Cell[] item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, Cell[] item)
        {
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public Cell[] this[int index] {
            get => index >= 0 && index < Count ? list[index] : null;
            set {
                if (index >= 0 && index < Count)
                    list[index] = value;
            }
        }

        public int extraHeight = 0;
        public int width = 9;
        public int height = 21;

        public int top {
            get => height - half + stackTop;
            set => m_StackTop += value - top;
        }

        public int half = 8;
        int m_StackTop = 0;
        bool m_Inited;

        public int stackTop {
            get => m_StackTop;
            set {
                m_StackTop = value;
                SetPosition();
            }
        }

        public Transform transform => Game.instance.MovableRoot;

        public bool inited {
            get => m_Inited;
            set => m_Inited = value;
        }

        public static Grid Create(LevelData levelData /*, List<CellData[]> data = null*/)
        {
            instance.Fill(levelData /*, data*/);
            return instance.Of(t => t.inited = false);
        }

        public void SetPosition()
        {
            transform.localPosition = Vector3.back * top;
        }

        public Grid Fill(LevelData levelData /*, List<CellData[]> data = null*/)
        {
            //var data = TetrisPreset.instance.Create();
            // if (!this.Any()) {
            Game.instance.MovableRoot.ClearChildTransforms(t =>
                t.gameObject.activeInHierarchy && t.GetComponent<Block>() != null);
            Game.instance.MovableRoot.localPosition = new Vector3(0, 0, -13);
            CardManager.instance.id = 100;
            if (Game.instance.ActorRoot == null) {
                Game.instance.ActorRoot = TetrisUtil.GetDynamicRoot("MapRoot/Movable/Actors").gameObject;
                Game.instance.ActorRoot.transform.localPosition = Vector3.zero;
                Game.instance.ActorRoot.transform.localScale = Vector3.one;
            }

            Game.instance.ActorRoot.transform.ClearChildTransforms(t => t.gameObject.activeInHierarchy);

            // }
            var lineNum = levelData.data.Any() ? levelData.data.Count() : height;
            if (levelData.data.Any()) Clear();
            while (Count < lineNum) {
                var blockPrefabs = TetrisPreset.instance.blockPrefabs;
                var newList = new List<Cell[]>(lineNum);
                for (var i = 0; i < lineNum; i++) newList.Add(new Cell[width]);
                newList.AddRange(this);
                var oldNum = Count;
                Clear();
                AddRange(newList);
                if (!levelData.data.Any()) {
                    // LevelData.instance.actors ??=
                    //     DB.Table<ActorData>().FirstOrDefault(t => t.Id == Level.instance.level);
                    LevelData.instance.actors.Clear();
                    GenMapManager.instance.Generate().map.data.ForEach((x, y, id) => {
                        //Debug.Log($"x={x},y={y},id={id}");
                        if (id >= 0) {
                            var prefab = blockPrefabs[id % 4];
                            var go = Core.Instantiate(prefab, Game.instance.MovableRoot.transform);
                            //Debug.Log($"[Instantiate] {go.name}".ToBlue(), go);
                            go.transform.localPosition = new Vector3(x, 0, stackTop - oldNum + y);
                            go.name = $"{id}[{y},{x}]";
                            this[x, y] = go.GetComponent<Cell>();
                            this[x, y].id = id;
                            this[x, y].colorId = id % 4;
                        }
                        else {
                            var actor = new TheActor();
                            actor.col = x;
                            actor.row = y;
                            actor.actorId = id;
                            var prefab = ActorLogic.instance.WarriorPrefab[
                                Random.Range(0, ActorLogic.instance.WarriorPrefab.Count())];
                            actor.transform = Core.Instantiate(prefab, Game.instance.ActorRoot.transform).transform;
                            actor.transform.localPosition = new Vector3Int(x, 0, y);
                            levelData.actors[(x, y)] = actor;
                        }
                    });
                    levelData._保存表();
                }
                else {
                    var minId = levelData.actors.Values.Min(t => t.actorId);
                    var oldId = minId;
                    for (var row = 0; row < levelData.data.Count; row++)
                    for (var col = 0; col < width; col++) {
                        var cell = levelData.data[row][col];
//                            if (cell == null) {
//                                Debug.Log($"cell is null row={row},col={col}");
//                            }
                        if (cell != null) {
                            if (levelData.actors.ContainsKey((col, row))) levelData.actors.Remove((col, row));
                            var prefab = blockPrefabs[cell.colorId];
                            var go = Core.Instantiate(prefab, Game.instance.MovableRoot.transform);
                            //Debug.Log($"[Instantiate] {go.name}".ToBlue(), go);
                            go.transform.localPosition = new Vector3(col, 0, stackTop - oldNum + row);
                            go.name = $"{cell.id}[{row},{col}]";
                            this[col, row] = go.GetComponent<Cell>();
                            this[col, row].id = cell.id;
                            this[col, row].colorId = cell.colorId;
                        }
                        else {
                            var actor = levelData.actors.ContainsKey((col, row))
                                ? levelData.actors[(col, row)]
                                : levelData.actors[(col, row)] = new TheActor() {
                                    col = col,
                                    row = row,
                                    actorId = minId -= 1
                                };
                            //if (levelData.actors.TryGetValue((col, row), out var actor)) {
                            var prefab = ActorLogic.instance.WarriorPrefab[
                                Random.Range(0, ActorLogic.instance.WarriorPrefab.Count())];
                            actor.transform = Core.Instantiate(prefab, Game.instance.ActorRoot.transform).transform;
                            actor.transform.localPosition = new Vector3Int(col, 0, row);
                            //LevelData.instance.actors.data[(col, y)] = actor;
                            //}
                        }
                    }

                    if (minId != oldId) levelData._保存表();
                }

                Debug.Log(Map.instance.GetPath(), Map.instance);
            }

            Map.instance.MapGen();
            return this;
        }

        void AddRange(List<Cell[]> newList)
        {
            list.AddRange(newList);
        }

        public Cell this[float col, float row] {
            get => this[(int) col, (int) row];
            set => this[(int) col, (int) row] = value;
        }

        public Cell this[Vector3 pos] {
            get => this[pos.x, pos.z];
            set => this[pos.x, pos.z] = value;
        }

        public Cell this[Vector2 pos] {
            get => this[pos.x, pos.y];
            set => this[pos.x, pos.y] = value;
        }

        public Cell this[Vector2Int pos] {
            get => this[pos.x, pos.y];
            set => this[pos.x, pos.y] = value;
        }

        public Cell this[(int x, int y) index] {
            get => this[index.x, index.y];
            set => this[index.x, index.y] = value;
        }

        public Cell this[Cell cell] {
            get => this[cell.col, cell.row];
            set => this[cell.col, cell.row] = value;
        }

        // public new Cell[] this[int row] {
        //     get {
        //         if (row >= 0) {
        //             for (int i = Count - 1; i <= row; i++) {
        //                 Add(new Cell[width]);
        //             }
        //         }
        //         return row >= 0 && row < Count ? base[row] : new Cell[width];
        //     }
        //     set {
        //         if (row >= 0) {
        //             for (int i = Count - 1; i <= row; i++) {
        //                 Add(new Cell[width]);
        //             }
        //
        //             base[row] = value;
        //         }
        //     }
        // }

        public Cell this[int col, int row] {
            get {
                // top 默认为0, 叠加时可能>0 , 整体下移后重置为0, row <= top时计算碰撞
                // if (row > top || col < 0 || col >= width || Count - 1 + row >= height || Count - 1 + row < 0)
                //     return null;
                // return base[Count - 1 + row][col];
                if (Mathf.Clamp(col, 0, width - 1) != col || Mathf.Clamp(row, 0, Count - 1) != row) return null;
                return this[row][col];
            }
            set {
                //Debug.Log($"row: {row} col: {col}");
                if (col >= 0 && col < width) {
                    for (var i = Count; i <= row; i++) //var line = new Cell[width];
                        // for (int j = 0; j < width; j++) {
                        //     line[j] = null;
                        // }
                        Add(new Cell[width]);

                    //Debug.Log($"count: {Count} row: {row}");
                    // if (this[row] == null) {
                    //     this[row] = new Cell[width];
                    // }
                    //Debug.Log($"cols: {this[row].Length}");
                    this[row /*Count + row - 9*/][col] = value;
                }
            }
        }

        public void Init()
        {
            if (!instance.inited) {
                TetrisManager.instance.Init(Game.spawner.Create(Game.instance.blocks));
                instance.inited = true;
            }
        }

        public int FindIndex(Expression<Func<Cell[], bool>> expression)
        {
            return list.FindIndex(t => expression.Compile().Invoke(t));
        }
    }
}