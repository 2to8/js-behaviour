using GameEngine.Extensions;
using MoreTags;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils.Scenes {

public class PrefabReference : SerializedMonoBehaviour {

    public RectTransformData data;
    public string path;
    public GameObject prefab;

#if UNITY_EDITOR
    public static void SavePrefabToReference(Transform target)
    {
        return;
        Debug.Log($"Saving Prefab {target.name}".ToRed(),target.gameObject);
        var go = new GameObject(target.name);
        SceneManager.MoveGameObjectToScene(go, target.gameObject.scene);
        var instance = go.RequireComponent<PrefabReference>();
        instance.data = new RectTransformData().Tap(t => t.PullFromTransform(target));
        go.transform.SetParent(target.parent);
        instance.data.PushToTransform(go.transform);
        var index = target.GetSiblingIndex();
        instance.path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(target);
        instance.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(instance.path);

        if (target.TryGetComponent(typeof(Tags), out var tags)) {
            Instantiate(tags, go.transform);
        }

        //PrefabUtility.GetOutermostPrefabInstanceRoot(target);
        DestroyImmediate(target.gameObject);
        go.transform.SetSiblingIndex(index);
    }
#endif
}

}