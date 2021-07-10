using GameEngine.Kernel._Appliation.Views.RaidView;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace GameEngine.Kernel._Appliation.Models.Levels {

public class LevelNode : Model<LevelNode> {

    [ShowInInspector, OdinSerialize]
    public int Level { get; set; }

    [ShowInInspector, OdinSerialize]
    public int index { get; set; }

    [ShowInInspector, OdinSerialize]
    public bool Active { get; set; }

    [ShowInInspector, OdinSerialize]
    public RaidType type { get; set; } = RaidType.None;

}

}