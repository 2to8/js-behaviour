using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Tetris.Main
{
    public class GotoScene : ViewManager<GotoScene>
    {
        [SerializeField]
        AssetReference sceneRef;

        public void Execute()
        {
            
            if (sceneRef != null && sceneRef.RuntimeKeyIsValid())
                sceneRef.LoadSceneAsync().Completed += handle => {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                        handle.Result.Scene.GetRootGameObjects().ForEach(g => g.CheckTagsFromRoot());
                    Addressables.ResourceManager.Acquire(handle);
                    Addressables.Release(handle);
                };
        }
    }
}