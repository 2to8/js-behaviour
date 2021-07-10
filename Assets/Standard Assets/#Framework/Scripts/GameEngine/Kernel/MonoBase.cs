using Sirenix.OdinInspector;
using UniRx.Async;
using UnityEngine;

namespace GameEngine.Kernel {

public class MonoBase : SerializedMonoBehaviour {

    // protected static AsyncOperationHandle<GameObject> AppLoaderHander;
    protected static UniTask AppLoaderHander;
    public static GameObject AppRoot;
    protected static object _lock = new object();
    public static bool IsDestroying { get; set; }

    void OnApplicationQuit()
    {
        IsDestroying = true;
    }

}

}