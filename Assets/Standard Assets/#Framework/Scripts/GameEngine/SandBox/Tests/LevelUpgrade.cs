using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace GameEngine.SandBox.Tests {

public class LevelData {

    [TableColumnWidth(20), LabelText("每次费用")]
    public float Cost;

    [TableColumnWidth(20), LabelText("级别")]
    public string Desc;

    [TableColumnWidth(20), LabelText("成功率")]
    public float Rate;

    [TableColumnWidth(20), LabelText("强化次数")]
    public int Times;

    [TableColumnWidth(20), LabelText("总费用")]
    public float TotalCast;

}

public class LevelUpgrade : SerializedMonoBehaviour {

    [TableList(ShowIndexLabels = true), LabelText("装备强化")]
    public List<LevelData> LevelDatas { get; set; } = new List<LevelData>();

    [Button]
    void TestRoll() { }

}

}