using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace GameEngine.SandBox.Tests {

// http://www.sirenix.net/odininspector/faq/39/how-can-i-make-my-data-in-odin-editor-windows-persistent
// You can also specify a location for your persistent data to be inside an Editor folder in order to prevent the scriptable object from ever being included in your game.
[GlobalConfig("Assets/Resources/MyConfigFiles/"), ShowOdinSerializedPropertiesInInspector]
public class MyConfig<T> : GlobalConfig<T>, ISerializationCallbackReceiver, ISupportsPrefabSerialization
    where T : GlobalConfig<T>, new() {

    [SerializeField, HideInInspector]
    SerializationData serializationData;

    public int SomePersistentField;

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        UnitySerializationUtility.DeserializeUnityObject(this, ref serializationData);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        UnitySerializationUtility.SerializeUnityObject(this, ref serializationData);
    }

    SerializationData ISupportsPrefabSerialization.SerializationData {
        get => serializationData;
        set => serializationData = value;
    }

}

}