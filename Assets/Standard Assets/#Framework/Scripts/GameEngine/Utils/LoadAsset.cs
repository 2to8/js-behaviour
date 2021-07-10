using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameEngine.Utils {

public class LoadAsset : MonoBehaviour {

    [SerializeField]
    AssetReference asset;

    [SerializeField]
    Transform replace;

    [SerializeField]
    bool useParent;

    async void Awake()
    {
        if (asset != null) {
            var target = replace ?? transform;
            var go = await asset.InstantiateAsync(target.position, target.rotation, target.parent).Task;
            go.transform.localScale = target.transform.localScale;
            go.transform.SetSiblingIndex(target.GetSiblingIndex());
            go.SetActive(target.gameObject.activeInHierarchy);

            if (replace != null) {
                Destroy(replace.gameObject);
            }
        }
    }

}

}