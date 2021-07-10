#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace Editors
{
#if UNITY_EDITOR
    public static partial class My
    {
        // public static void ExecuteCommand(string inputVideo)
        // {
        //     var thread = new Thread(delegate() {
        //         Command(inputVideo);
        //     });
        //     thread.Start();
        // }
        [MenuItem("Tests/Shell Command")]
        static void Command()
        {
            WSL("npm run deploy");
        }

        public static void WSL(string cmd)
        {
            var processInfo = new ProcessStartInfo("wsl.exe", cmd);
           // processInfo.WorkingDirectory = Path.GetDirectoryName(Application.dataPath) + "";
            var process = Process.Start(processInfo);
            process!.WaitForExit();
            process.Close();

            //EditorUtility.DisplayDialog("close", "close", "OK");
        }

        [MenuItem("GameObject/== Move To Last ==", false, 0)]
        static void MoveToLast()
        {
            var go = Selection.activeGameObject;

            if (go != null) {
                go.transform.SetAsLastSibling();
            }
        }

        // static async Task TestCommand()
        // {
        //     var operation = EditorShell.Execute("wsl.exe npm run deploy");
        //     operation.onExit += exit => { };
        //     operation.onLog += (EditorShell.LogType LogType, string log) => {
        //         Debug.Log(log);
        //     };
        //     int exitCode = await operation; //support async/await
        //
        //     //Application.OpenURL("wsl.exe npm run deploy");
        // }

        [InitializeOnLoadMethod]
        static void DoPrefabStageListener()
        {
            // Open Prefab editing interface callback
            PrefabStage.prefabStageOpened += OnPrefabStageOpened;

            // Callback before Prefab is saved
            PrefabStage.prefabSaving += OnPrefabSaving;

            // Callback after Prefab is saved
            PrefabStage.prefabSaved += OnPrefabSaved;

            // Close Prefab editing interface callback
            PrefabStage.prefabStageClosing += OnPrefabStageClosing;
        }

        static void OnPrefabStageClosing(PrefabStage obj) { }

        static void OnPrefabSaved(GameObject obj) { }

        static void OnPrefabSaving(GameObject obj) { }

        static void OnPrefabStageOpened(PrefabStage obj) { }

        //....
    }
#endif
}

#endif