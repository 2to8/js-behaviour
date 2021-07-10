using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameEngine.Utils {

[Serializable, ShowOdinSerializedPropertiesInInspector]
public class MyCustomDict<T> : Dictionary<string, T>, ISerializationCallbackReceiver where T : Object {

    [SerializeField, HideInInspector]
    List<string> keyData = new List<string>();

    [SerializeField, HideInInspector]
    List<T> valueData = new List<T>();

    // public new T this[string index] {
    //     get {
    //         if ( String.IsNullOrEmpty(index) ) return default;
    //
    //         if ( !ContainsKey(index) ) {
    //             base[index] = Res.Load<T>(index);
    //         }
    //
    //         return base[index];
    //     }
    //     set {
    //         if ( !String.IsNullOrEmpty(index) ) base[index] = value;
    //     }
    // }

    public T this[object index] {
        get {
            var name = GetType(index);

            if (!ContainsKey(name)) {
                base[name] = ResHelper.Load<T>(index);

                // Debug.Log($"{base[name]}");
                //Add(name, Res.Load<T>(index));
            }

            return base[name];
        }
        set => base[GetType(index)] = value;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        Clear();

        for (var i = 0; i < keyData.Count && i < valueData.Count; i++) {
            this[keyData[i]] = valueData[i];
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        keyData.Clear();
        valueData.Clear();

        foreach (var item in this) {
            keyData.Add(item.Key);
            valueData.Add(item.Value);
        }
    }

    static string GetType(object obj)
    {
        if (obj is Type type) {
            return type.FullName;
        }

        if (obj is string s) {
            return s;
        }

        var parts = obj.GetType().FullName.Split('.');

        return parts.Length > 0 ? parts[parts.Length - 1].Replace("+", ".") + "." + obj : obj + "";
    }

}

}