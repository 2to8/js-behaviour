using GameEngine.Kernel._Appliation.Views.RaidView;
using System;

namespace GameEngine.Kernel._Appliation.Models.Levels.RaidLevel {

[Serializable]
public class RaidCell {

    public bool Active;
    public bool Display = true;
    public int index;
    public int Stars;
    public RaidType type = RaidType.Normal;

}

}