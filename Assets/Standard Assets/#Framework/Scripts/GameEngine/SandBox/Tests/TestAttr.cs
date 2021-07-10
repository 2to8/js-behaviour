using GameEngine.Attributes;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameEngine.SandBox.Tests {

[Inject(typeof(Button), "TestAttr"), Serializable]
public class TestAttr : MonoBehaviour, IPointerDownHandler {

    [SerializeField]
    AssetReference lua;

    [SerializeField]
    TextAsset luaAsset;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down..");
    }

    [Button]
    async void Start()
    {
        var test = await lua.LoadAssetAsync<TextAsset>().Task;
        Debug.Log(test.text);

        // Addressables.LoadAssetAsync<TextAsset>("Lua/HelloWorld.lua").Completed += t =>
        // {
        //     Debug.Log(t.Result.text);
        // };

        //Debug.Log("stared");
    }

}

}