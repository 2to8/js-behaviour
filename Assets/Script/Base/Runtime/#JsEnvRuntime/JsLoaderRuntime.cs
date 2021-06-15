using Puerts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Base.Runtime
{
    public class JsLoaderRuntime : ILoader
    {
        public string ReadFile(string filePath, out string debugPath)
        {
            debugPath = Application.dataPath + $"/Res/JavaScript/{filePath}.txt";
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            debugPath = debugPath.Replace('/', '\\');
#endif
            if (filePath.StartsWith("puerts/")) {
                debugPath = Application.dataPath + $"/ThirdParty/Puerts/Src/Resources/{filePath}.txt";
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                debugPath = debugPath.Replace('/', '\\');
#endif
                //Debug.Log(debugPath);
                var asset = Resources.Load<TextAsset>(filePath);
                if (!asset) {
                    Debug.LogError($"JsEnvRuntime, file not found: {filePath}\n{debugPath}");
                }

                return asset.text;
            }

            //Debug.Log(debugPath);
            var operation = Addressables.LoadAssetAsync<TextAsset>($"{appJsPath}/{filePath}.txt");
            operation.WaitForCompletion();
            if (!operation.Result) {
                Debug.LogError($"JsEnvRuntime, file not found: {filePath}\n{debugPath}");
            }

            return operation.Result.text;
        }

        public bool FileExists(string filePath)
        {
            return true;
        }

        private readonly string appJsPath = "Assets/Res/JavaScript";
    }
}