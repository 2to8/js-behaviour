using GameEngine.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameEngine.Helpers {

[Serializable]
public class SceneData {

    //public AssetReferenceWrap asset;
    public AssetReference Reference;

    [Reference(nameof(Reference))]
    public string Name;

    [SerializeField]
    public List<string> Tags = new List<string>();

}

}