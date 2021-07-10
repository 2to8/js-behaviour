using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

public class TestSO1 : ScriptableObject
{
    public string Value = "Test Value";
}

public partial class AddressableLabels
{
    public const string Default = "default";
    public const string AutoLoad = "autoload";
    public const string Instantiate = "instantiate";
}

[Serializable]
public class AssetReferenceScriptableObject : AssetReferenceT<ScriptableObject>
{
    public AssetReferenceScriptableObject(string guid) : base(guid) { }
}

public class AddressablesLoader : MonoBehaviour
{
    public AssetReferenceGameObject TestPrefab;
    public AssetReferenceScriptableObject TestScriptableObject;

    void Awake()
    {
        Addressables.ResourceManager.CreateChainOperation(AssetCache.LoadAsync(AddressableLabels.AutoLoad),
            InstantiateQuery);
        AssetCache.LoadCompleted += SomeTestsAndUsageExamples;
    }

    private AsyncOperationHandle<IList<Object>> InstantiateQuery(AsyncOperationHandle<IList<Object>> loadResults)
    {
        // Need to first check if any asset has the label because LoadAssetsAsync will throw a key not found exception.
        return Addressables.ResourceManager.CreateChainOperation(
            Addressables.LoadResourceLocationsAsync(AddressableLabels.Instantiate),
            resourceLocations => DoInstantiate(resourceLocations, loadResults));
    }

    private AsyncOperationHandle<IList<Object>> DoInstantiate(
        AsyncOperationHandle<IList<IResourceLocation>> resourceLocations,
        AsyncOperationHandle<IList<Object>> loadResults)
    {
        if (resourceLocations.Result.Count != 0) {
            return Addressables.LoadAssetsAsync<Object>(AddressableLabels.Instantiate, InstantiateAddressable);
        }

        return loadResults;
    }

    private void InstantiateAddressable(Object o)
    {
        Debug.Log($"Instantiating: {o.name}");
        Instantiate(o);
    }

    private void SomeTestsAndUsageExamples()
    {
        if (TestScriptableObject != null) {
            // Get the directly referenced test SO
            var testScriptableObject = AssetCache.GetAsset<TestSO1>(TestScriptableObject);
            Debug.Log($"{nameof(TestScriptableObject)}: {testScriptableObject.Value}");
        }

        if (TestPrefab == null) return;

        // Get the directly referenced test GO
        var testPrefab = AssetCache.GetAsset<GameObject>(TestPrefab);
        var obj = Instantiate(testPrefab);
        obj.name += $" ({nameof(TestPrefab)})";

        // AssetReference embedded in instantiated prefab
        AssetReference embeddedRef = obj.GetComponent<SODependencyHolder>()?.SO;

        if (embeddedRef == null) return;

        // Embedded AssetReference are not populated automatically
        // Each would need to be loaded Async by the default system.
        Debug.Assert(embeddedRef.Asset == null);

        // However its value has already been cached.
        Debug.Log($"Embedded Reference Cached/Sync: '{AssetCache.GetAsset<TestSO1>(embeddedRef).Value}'");

        // Default way would be to get it async
        embeddedRef.LoadAssetAsync<TestSO1>().Completed += handle =>
            Debug.Log($"Embedded Reference Async: {handle.Result.Value}");
    }
}

public static class AssetCache
{
    private static Dictionary<string, CachedAssetResource> _assetReferences =
        new Dictionary<string, CachedAssetResource>();

    public static AsyncOperationHandle DownloadResourcesHandle { get; private set; }

    private static AsyncOperationHandle<IList<Object>> _loadChain;

    private static Dictionary<int, string> _keysByIndex = new Dictionary<int, string>();

    public delegate void AssetCacheEventHandler();

    public static event AssetCacheEventHandler LoadCompleted;

    public static AsyncOperationHandle<IList<Object>> LoadAsync(object key)
    {
        DownloadResourcesHandle = Addressables.DownloadDependenciesAsync(key);

        return Addressables.ResourceManager.CreateChainOperation(DownloadResourcesHandle, LoadAssets);
    }

    private static AsyncOperationHandle<IList<Object>> LoadAssets(AsyncOperationHandle arg)
    {
        var toLoad = new List<IResourceLocation>();
        var index = 0;

        foreach (var loc in Addressables.ResourceLocators) {
            foreach (object objKey in loc.Keys) {
                if (!(objKey is string key)) continue;

                if (!Guid.TryParse(key, out Guid keyGuid)) continue;

                if (!loc.Locate(key, typeof(UnityEngine.Object), out var locationsFromKey)) continue;

                // Everything except those with a RuntimeKey have already been excluded;
                var location = locationsFromKey[0];
                var entry = new CachedAssetResource {
                    RunTimeKeyGuid = keyGuid,
                    RunTimeKeyString = key,
                    ResourceLocation = location
                };

                toLoad.Add(location);

                _assetReferences[location.ToString()] = entry;
                _assetReferences[key] = entry;
                _keysByIndex[index] = key;
                index++;

                Debug.Log($"Processed Addressable: Key={key}, Path={location}");
            }
        }

        _loadChain = Addressables.LoadAssetsAsync<UnityEngine.Object>(toLoad, OnItemsLoadCompleted);
        _loadChain.Completed += LoadChain_Completed;

        return _loadChain;
    }

    private static void LoadChain_Completed(AsyncOperationHandle<IList<Object>> obj)
    {
        // Assign the values of everything loaded to the asset map
        for (int i = 0; i < obj.Result.Count; i++) {
            var resource = _assetReferences[_keysByIndex[i]];
            resource.Asset = obj.Result[i];
        }

        var handler = LoadCompleted;
        handler?.Invoke();

        Debug.Log($"LoadChain_Completed: TotalAssets={obj.Result.Count}");
    }

    private static void OnItemsLoadCompleted(Object obj)
    {
        Debug.Log($"Loaded Asset: {obj.name}");
    }

    public class CachedAssetResource
    {
        public IResourceLocation ResourceLocation;
        public Object Asset;
        public Guid RunTimeKeyGuid;
        public string RunTimeKeyString;
    }

    public static T GetAsset<T>(AssetReference assetReference) where T : UnityEngine.Object
    {
        return GetAsset<T>(assetReference.RuntimeKey);
    }

    public static T GetAsset<T>(object runtimeKey) where T : UnityEngine.Object
    {
        var key = EvaluateKey(runtimeKey);

        if (_assetReferences.TryGetValue(key, out var assetResource)) {
            if (assetResource.Asset == null)
                throw new InvalidKeyException("Asset has not been preloaded; try loading it Async instead");

            return UnsafeUtility.As<Object, T>(ref assetResource.Asset);
        }

        return default;
    }

    public static AsyncOperationHandle<T> GetAssetAsync<T>(object runtimeKey) where T : UnityEngine.Object
    {
        var key = EvaluateKey(runtimeKey);

        if (_assetReferences.TryGetValue(key, out var assetReference)) {
            return Addressables.LoadAssetAsync<T>(assetReference.ResourceLocation);
        }

        return default;
    }

    public static IResourceLocation GetResourceLocation(object runtimeKey)
    {
        var key = EvaluateKey(runtimeKey);

        if (_assetReferences.TryGetValue(key, out var assetReference)) {
            return assetReference.ResourceLocation;
        }

        return default;
    }

    public static string EvaluateKey(object obj)
    {
        return (string)(obj is IKeyEvaluator evaluator ? evaluator.RuntimeKey : obj);
    }
}