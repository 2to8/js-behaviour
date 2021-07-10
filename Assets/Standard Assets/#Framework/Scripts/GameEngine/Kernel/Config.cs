using Sirenix.OdinInspector;

namespace GameEngine.Kernel {

public class ModelBase : SerializedScriptableObject { }

public class Config<T> : ModelBase where T : Config<T> {

    // [Button]
    // protected T Create() {
    //     return Instance;
    // }
    /*[ RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad) ]
    static async void LoadDefaultInstance() {
        m_Instance = await Addressables.LoadAssetAsync<T>($"Config/{typeof(T).Name}.asset").Task;
        Debug.Log($"{typeof(T).Name} loaded");
    }*/

}

}