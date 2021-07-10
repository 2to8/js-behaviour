using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Common.JSAdapter.Attributes {

public class Attribute<T> : ScriptableObject {

    public string name;
    public T value;

}

[Serializable]
public class FloatAttribute : Attribute<float> { }

[Serializable]
public class AssetReferenceAttribute : Attribute<AssetReference> { }

[Serializable]
public class AssetReferenceGameObjectAttribute : Attribute<AssetReferenceGameObject> { }

[Serializable]
public class AssetReferenceTextureAttribute : Attribute<AssetReferenceTexture> { }

[Serializable]
public class AssetReferenceTexture2DAttribute : Attribute<AssetReferenceTexture2D> { }

[Serializable]
public class AssetReferenceTexture3DAttribute : Attribute<AssetReferenceTexture3D> { }

[Serializable]
public class AssetReferenceSpriteAttribute : Attribute<AssetReferenceSprite> { }

[Serializable]
public class IntAttribute : Attribute<int> { }

[Serializable]
public class StringAttribute : Attribute<string> { }

}