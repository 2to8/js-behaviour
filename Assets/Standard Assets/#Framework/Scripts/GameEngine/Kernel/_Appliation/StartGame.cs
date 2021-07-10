using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameEngine.Kernel._Appliation {

[Serializable]
public class StartGame : MonoBehaviour {

    [FormerlySerializedAs("LoadGame"), SerializeField]
    public UnityEvent loadGame;

    [FormerlySerializedAs("NextScene"), SerializeField]
    AssetReference nextScene;

    void Start()
    {
        loadGame?.Invoke();
    }

    public void DoStart()
    {
        nextScene?.LoadSceneAsync();
    }

}

}