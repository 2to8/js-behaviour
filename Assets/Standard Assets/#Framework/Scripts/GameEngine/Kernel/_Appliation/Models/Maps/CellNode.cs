using GameEngine.Kernel._Appliation.Views;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

namespace GameEngine.Kernel._Appliation.Models.Maps {

[Serializable]
public class CellNode {

    [ShowInInspector, OdinSerialize]
    public bool isCurrent { get; set; }

    [ShowInInspector, OdinSerialize]
    public int x { get; set; }

    [ShowInInspector, OdinSerialize]
    public int y { get; set; }

    [ShowInInspector, OdinSerialize]
    public Nodetype nodetype { get; set; }

}

}