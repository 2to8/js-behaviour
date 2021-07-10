using System.Collections.Generic;
using UnityEngine;

namespace MoreTags {

[AddComponentMenu("MoreTags/Prefab Tags", 0)]

//[ExecuteInEditMode]
public class PrefabTags : MonoBehaviour {

    public List<string> Tags = new List<string>();

    void Awake()
    {
        gameObject.AddTag(Tags.ToArray());
        RemoveSelf();
    }

#if UNITY_EDITOR
    void Reset()
    {
        Invoke("RemoveSelf", 0);
    }
#endif
    void RemoveSelf()
    {
    #if UNITY_EDITOR
        DestroyImmediate(this);
    #else
            Destroy(this);
    #endif
    }

}

}