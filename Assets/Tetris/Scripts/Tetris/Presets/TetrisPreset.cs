using GameEngine.Attributes;
using GameEngine.Extensions;
using GameEngine.Kernel;
using Presets;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Consts;
using Tetris;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityRoyale;

namespace Arena
{
    [SceneBind(SceneName.Main)]
    public class TetrisPreset : Manager<TetrisPreset>
    {
        public List<GameObject> blockPrefabs = new List<GameObject>();

        [FormerlySerializedAs("mapParent")]
        public Transform mapRoot;

        //public LinkedList<Cell[]> lines = new LinkedList<Cell[]>();
        int m_Top;

        [ShowInInspector]
        [PropertyRange(0, 21)]
        [HideLabel]
        [Title("Top", horizontalLine: false)]
        int top {
            get => m_Top;
            set {
                m_Top = value;
                mapRoot.localPosition = new Vector3(0, 0, -value);
            }
        }

        public MapGenData map;
        public int baseLines = 2;
        public Dictionary<int, GameObject> prefabs = new Dictionary<int, GameObject>();

        public GameObject NextPrefab()
        {
            return blockPrefabs[new System.Random().Next(blockPrefabs.Count())];
        }

#if UNITY_EDITOR

        [ButtonGroup("gen")]
        public MapGenData Create(Action<int, int, int> action = null)
        {
            GenClear();
            var manager = GenMapManager.instance;
            prefabs.Clear();
            //lines.Clear();
            manager.Of(t => t.originMapData = map).Generate().map.data.ForEach((x, y, value) => {
                action?.Invoke(x, y, value);

                // Debug.Log($"row: {y} col: {x} value: {value}");
                //
                // while (y > lines.Count - 1) {
                //     var t = new Cell[9];
                //
                //     for (int i = 0; i < 9; i++) {
                //         t[i] = new Cell();
                //     }
                //     lines.AddLast(t);
                // }
                // var cell = lines.getNodeAt(y).Value[x];
                // Assert.IsNotNull(cell, $"lines:{lines.Count}");
                // cell.sharpIndex = value;
                if (value > 0) {
                    var prefab = blockPrefabs[value % 4];

                    // prefabs.TryGetValue(value, out var gameObject)
                    // ? gameObject
                    // : prefabs[value] = NextPrefab();
                    //Debug.Log(PrefabUtility.IsPartOfPrefabAsset(prefab));
                    //Debug.Log(prefab.name);
                    //Debug.Log(PrefabUtility.FindPrefabRoot(prefab));
                    var go = Core.Instantiate(prefab, mapRoot.position, Quaternion.identity, mapRoot);
                    Debug.Log($"[Instantiate] {go.name}".ToBlue(), go);
                    go.transform.localPosition = new Vector3(x, 0, y);
                    go.name = $"{x},{y}";

                    //cell.transform = go.transform;

                    // map.transform[col, row] = go.transform;

                    // go.transform.localScale = prefab.transform.localScale;
                }
            });
            return map;
        }

        //[Button, ButtonGroup("gen")]
        void GenClear()
        {
            mapRoot.ClearChildTransforms(t => t.gameObject.activeInHierarchy);
        }

#endif
    }
}