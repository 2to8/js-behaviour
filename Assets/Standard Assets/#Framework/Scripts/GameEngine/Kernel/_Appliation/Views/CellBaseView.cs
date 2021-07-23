using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Kernel._Appliation.Views {

public class CellBaseView : BaseView<CellBaseView> {

    public MeshRenderer bgTile;

    //public Dictionary<Nodetype, MeshRenderer> Nodes = new Dictionary<Nodetype, MeshRenderer>();
    //public Dictionary<SeaType,MeshRenderer> Lines = new Dictionary<SeaType, MeshRenderer>();
    public MeshRenderer lineLeftTile;
    public MeshRenderer lineRightTile;

    [OdinSerialize]
    public Dictionary<Nodetype, MeshRenderer> meshRenderers = new Dictionary<Nodetype, MeshRenderer>();

    public MeshRenderer nodeTile;

    [OdinSerialize]
    public Dictionary<Nodetype, SpriteRenderer> spriteRenderers = new Dictionary<Nodetype, SpriteRenderer>();

    public LineType lineType { get; set; }
    public Nodetype nodeType { get; set; }

}

}