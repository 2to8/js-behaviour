using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Common.JSAdapter {

[CreateAssetMenu(fileName = nameof(AssetReferenceWrap), menuName = "ScriptableObjects/" + nameof(AssetReferenceWrap),
    order = 0)]
public class AssetReferenceWrap : ScriptableObject {

    public AssetReference assetReference;

}

}