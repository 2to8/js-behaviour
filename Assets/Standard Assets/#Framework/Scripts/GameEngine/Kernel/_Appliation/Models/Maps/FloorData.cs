using GameEngine.Kernel._Appliation.Views;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace GameEngine.Kernel._Appliation.Models.Maps {

public class FloorData : Model<FloorData> {

    // private Dictionary<int, HexCoordinates> m_lines = new Dictionary<int, HexCoordinates>();

    [ShowInInspector, OdinSerialize, TextBlob(nameof(BNodes))]
    public Dictionary<(int x, int y), CellNode> Nodes { get; set; } = new Dictionary<(int x, int y), CellNode>();

    [ShowInInspector, OdinSerialize, TextBlob(nameof(BRndWeight))]
    public Dictionary<Nodetype, int> RndWeight { get; set; } = new Dictionary<Nodetype, int>();

    [ShowInInspector, OdinSerialize]
    public int x { get; set; }

    [ShowInInspector, OdinSerialize]
    public int y { get; set; }

    [ShowInInspector, OdinSerialize]
    public string guid { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 10);

    [ShowInInspector, /*Ignore,*/ TextBlob(nameof(BCurrent))]
    public (int x, int y) current { get; set; } = (1, 1);

    [OdinSerialize]
    public string BNodes { get; set; }

    [OdinSerialize]
    public string BCurrent { get; set; }

    [OdinSerialize]
    public string BRndWeight { get; set; }

    // protected override void Awake() {
    //     base.Awake();
    //     if(!nodes_blob.IsNullOrWhitespace()) {
    //         nodes = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, Nodetype>>>(nodes_blob);
    //     }
    // }

    //返回值可做为奖品类数组下标，和权重比一一对应即可。
    Nodetype GetRandomNode()
    {
        var total = 0;
        RndWeight.Values.ForEach(t => total += t);
        var rand = Random.Range(1, total + 1);
        var tmp = 0;

        for (var i = 0; i < RndWeight.Count; i++) {
            tmp += RndWeight.Values.ElementAt(i);

            if (rand < tmp) {
                return RndWeight.Keys.ElementAt(i);
            }
        }

        return default;
    }

    public bool Checktype(Nodetype nodetype, params (int x, int y)[] nodes)
    {
        return nodes.Select(tx => Nodes.ContainsKey(tx) && Nodes[tx].nodetype == nodetype).Any();
    }

    [Button]
    public void Clear()
    {
        Nodes.Clear();

        for (var i = 0; i < 9; i++) {
            for (var j = -3; j < 4; j++) {
                Nodes[(i, j + 3)] = new CellNode { nodetype = Nodetype.LineEmpty };

                if (Math.Abs(j) > i + 1) {
                    continue;
                }

                if (i == 0) {
                    // 第一行3个普通攻击
                    Nodes[(i, j + 3)].nodetype = Nodetype.Normal;

                    continue;
                }

                var type = GetRandomNode();

                while ((type == Nodetype.Bonus || type == Nodetype.Restroom || type == Nodetype.Shop) &&
                    Checktype(type, (i - 1, j + 2), (i - 1, j + 3), (i - 1, j + 4), (i, j + 2), (i, j + 1))) {
                    type = GetRandomNode();
                }

                Nodes[(i, j + 3)].nodetype = type;
            }
        }
    }

}

}