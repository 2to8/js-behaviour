
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NodeCanvas.Framework
{
    public class AssetRefs : SerializedMonoBehaviour
    {
        [LabelText("Main")]
        [SerializeField] private AssetReference _sqrARef;
        [SerializeField] private List<AssetReference> _references = new List<AssetReference>();

        public AssetReference main => _sqrARef;
        public List<AssetReference> all => _references;
    }
}
