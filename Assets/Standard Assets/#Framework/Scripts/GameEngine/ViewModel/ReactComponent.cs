using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.ViewModel {

[Serializable]
public class ReactComponentData {

    public string sender;
    public string target;

}

public class ReactComponent : MonoBehaviour {

    // public AssetReferenceTexture m_AssetReferenceTexture;
    // public AssetReferenceGameObject m_AssetReferenceGameObject;
    // public AssetReferenceSprite m_AssetReferenceSprite;
    // public AssetLabelReference m_AssetLabelReference;

    [OdinSerialize]
    Dictionary<string, List<ReactComponentData>> Data = new Dictionary<string, List<ReactComponentData>>();

}

}