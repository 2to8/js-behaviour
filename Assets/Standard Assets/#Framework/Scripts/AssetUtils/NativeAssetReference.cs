using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// Implicitly convertable version of AssetReference, that can be stored in components.
/// Holds the RuntimeKey used to find it again at runtime through Addressables.
/// </summary>
public struct NativeAssetReference : IComponentData, IEquatable<NativeAssetReference>, IKeyEvaluator
{
    // For reference, these are the types I've been playing with:
    //NativeString64 (64 bytes long, 30 characters).
    //NativeString512 (512 bytes, 254 chars)
    //NativeString4096 (4096 bytes, 2046 chars)
    //https://forum.unity.com/threads/strings-are-usable-in-component-data-now.644647/
    public FixedString512 RuntimeKey;

    public int Hash;

    public static implicit operator AssetReference(NativeAssetReference instance)
    {
        return new AssetReference(instance.RuntimeKey.ToString());
    }

    public static implicit operator NativeAssetReference(AssetReference instance)
    {
        Debug.Assert(instance.RuntimeKeyIsValid(), $"The AssetReference is not valid. Key={instance.RuntimeKey}");

        var key = (string)instance.RuntimeKey;
        var nativeKey = new FixedString512(key);
        var result = new NativeAssetReference
        {
             RuntimeKey = nativeKey,
             Hash = CreateHash(key)
        };
        return result;
    }

    public bool IsValid => Hash != 0;

    public static int CreateHash(string input)
    {
        var hash = 0;
        foreach (var t in input)
            hash = (hash << 5) + hash + t;
        return hash;
    }

    public bool Equals(NativeAssetReference other) => Hash == other.Hash;

    public override bool Equals(object obj) => obj is NativeAssetReference other && Equals(other);

    public override int GetHashCode() => Hash;

    public override string ToString() => $"{Hash}, Key={RuntimeKey.ToString()}";

    object IKeyEvaluator.RuntimeKey => RuntimeKey.ToString();

    bool IKeyEvaluator.RuntimeKeyIsValid() => IsValid;

}