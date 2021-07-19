using GameEngine.Attributes;
using GameEngine.Extensions;
using GameEngine.Kernel;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Consts;
using Tetris;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Presets
{
    /// <summary>
    /// https://meatfighter.com/tetrisprinteralgorithm/
    /// </summary>
    [SceneBind(SceneName.Main)]
    public class GenMapManager : ViewManager<GenMapManager>
    {
        public MapGenData map;

        [FormerlySerializedAs("config")]
        public MapGenData originMapData;

        public Transform targetView;

        // [ButtonGroup("New Gen"),FoldoutGroup("new")]
        void resetData()
        {
            map.current = 0;
            map.data = new int[map.size.x, map.size.y];
            map.last = 0;
        }

        // [ButtonGroup("New Gen")]
        void TestButtonColor()
        {
            var btn = targetView.GetComponentsInChildren<GenMapButton>().ForEach(btn => btn.Start());

            //btn.GetComponent<Image>().color = Color.cyan;
        }

        [SerializeField]
        public int defaultWeight = 2;

        public int maxLineEmpty = 5;

        //[ButtonGroup("New Gen")]
        void Test()
        {
            Generate(true);
        }

        public GenMapManager Generate(bool showUI = false)
        {
            if (originMapData == null) originMapData = Core.FindOrCreatePreloadAsset<MapGenData>();
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(originMapData),
                map ??= ScriptableObject.CreateInstance<MapGenData>());
            var tmp = map.GetByRowCol;
            var rnd = new System.Random(DateTime.Now.Millisecond);
            tmp.ForEach(kv => {
                var list = kv.Value.Select(k => k.Value).ToList();
                var n = new System.Random().Next(1, defaultWeight);
                while (list.Count(t => t <= 0) < n) {
                    //Debug.Log(rnd);
                    var id = list.Where(d => d > 0).ToList()[rnd.Next(list.Count(d => d > 0))];
                    for (var i = 0; i < map.size.x; i++)
                    for (var j = 0; j < map.size.y; j++)
                        if (map.data[i, j] == id) {
                            map.data[i, j] = -id;
                            tmp[j][i] = -id;
                        }

                    list = kv.Value.Select(k => k.Value).ToList();
                }
            });
            if (checkFail(out var maxlines)) {
                //Debug.Log($"fail max: {maxlines}".ToRed());
                Generate(showUI);
            }
            else {
                if (showUI)
                    targetView?.GetComponentsInChildren<GenMapButton>().ForEach(btn => { btn.Start(); });
            }

            return this;
        }

        //
        // [ButtonGroup("New Gen")]
        void CheckTest()
        {
            Debug.Log($"fail: {checkFail(out var ret)} lines: {ret}");
        }

        bool checkFail(out int maxlines)
        {
            var fail = false;
            var ret = 0;
            var tmp = map.GetByRowCol;
            tmp.ForEach(row => {
                row.Value.Where(col => col.Value < 0).Select(k => k.Value).Distinct().ForEach(id => {
                    var ids = new List<int> {
                        id
                    };
                    var min = row.Key;
                    var max = row.Key;
                    int uptime = 0, downtime = 0;
                    Assert.IsTrue(ids.Any());
                    for (var y = row.Key; y < map.size.y; y++) {
                        uptime += 1;
                        for (var x = 0; x < map.size.x; x++) {
                            if (map.data[x, y] < 0 && !ids.Contains(map.data[x, y]) &&
                                (x - 1 >= 0 && ids.Contains(map.data[x - 1, y]) ||
                                    x + 1 < map.size.x && ids.Contains(map.data[x + 1, y]) ||
                                    y - 1 >= 0 && ids.Contains(map.data[x, y - 1]) ||
                                    y + 1 < map.size.y && ids.Contains(map.data[x, y + 1])))
                                ids.Add(map.data[x, y]);
                            if (map.data[x, y] < 0 && ids.Contains(map.data[x, y])) max = y;
                        }
                    }

                    for (var y = row.Key - 1; y >= 0; y--) {
                        downtime += 1;
                        for (var x = 0; x < map.size.x; x++) {
                            if (map.data[x, y] < 0 && !ids.Contains(map.data[x, y]) &&
                                (x - 1 >= 0 && ids.Contains(map.data[x - 1, y]) ||
                                    x + 1 < map.size.x && ids.Contains(map.data[x + 1, y]) ||
                                    y + 1 < map.size.y && ids.Contains(map.data[x, y + 1]) ||
                                    y - 1 >= 0 && ids.Contains(map.data[x, y - 1])))
                                ids.Add(map.data[x, y]);
                            if (map.data[x, y] < 0 && ids.Contains(map.data[x, y])) min = y;
                        }

                        // Debug.Log($"down: {downtime} {string.Join(", ",data.data.To)}");
                    }

                    if (ret < max - min + 1) ret = max - min + 1;
                    if (max - min + 1 > maxLineEmpty) //ret = max - min + 1;
                        fail = true;

                    //Debug.Log($"line: {row.Key} id: {id} max: {max} min: {min} offset: {max - min + 1} up/down: {uptime}/{downtime} ids: {string.Join(", ", ids)}");
                });
            });
            maxlines = ret;
            return fail;
        }
    }
}