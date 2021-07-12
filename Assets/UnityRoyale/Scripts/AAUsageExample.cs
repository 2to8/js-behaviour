using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityRoyale
{
    [Name("AAUsageExample"), Category("✫ App"), Description("UnityRoyale.AAUsageExampleAction")]
    public partial class AAUsageExampleAction : ActionTask<Transform>
    {
        public BBParameter<AssetReferenceGameObject> refObject;
        public BBParameter<AssetReference> scene;
    }

    public partial class AAUsageExample : MonoBehaviour
    {
        public AssetReferenceGameObject refObject;
        public AssetReference scene;

        void Start()
        {
            refObject.InstantiateAsync(Vector3.zero, Quaternion.identity, null).Completed += asyncOp => {
                Debug.Log(asyncOp.Result.name + " loaded.");
            };
        }

        // private void OnAssetInstantiated(IAsyncOperation<GameObject> asyncOp)
        // {
        // 	Debug.Log(asyncOp.Result.name + " loaded.");
        // }
    }
}