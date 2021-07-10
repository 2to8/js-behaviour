using GameEngine.Kernel._Appliation.Models.Levels.ViewModel;
using GameEngine.Kernel._Appliation.Views.RaidView;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace GameEngine.Kernel._Appliation.Models.Levels {

[CreateAssetMenu(fileName = "LevelData", menuName = "App/LevelData", order = 0)]
public class LevelData : Model<LevelData> {

    [HideInInspector, SerializeField]
    int m_Gold;

    [HideInInspector, SerializeField]
    int m_Power;

    [ShowInInspector, OdinSerialize]
    public string CurrentVersion { get; set; }

    [ShowInInspector, OdinSerialize]
    public int Level { get; set; }

    [ShowInInspector, OdinSerialize]
    public NodeCount levelType { get; set; } = NodeCount.Raid;

    [ShowInInspector, OdinSerialize]
    public int CurrentLine { get; set; }

    [ShowInInspector] // 资金
    public int Gold { get => m_Gold; set => LevelGoldAttr.Val(m_Gold = value); }

    [ShowInInspector] // 电力
    public int Power { get => m_Power; set => LevelPowerAttr.Val(m_Power = value); }

}

}