using GameEngine.Views.Contracts;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace GameEngine.Views {

public class PrefabBaseView : BaseView<PrefabBaseView> {

    public enum Type {

        None, IShape, JShape, LShape, OShape, TShape, SShape, ZShape, ShapeBlock, Block,

    }

    public GameObject prefab;
    public Type type;

}

}