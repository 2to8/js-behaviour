using Editors;
using Sirenix.Utilities;
using System.Collections;
using System.IO;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolbarExtender;
using Utils.Scenes;

#if UNITY_EDITOR
namespace Helpers
{
    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        static SceneSwitchLeftButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);
            ToolbarExtender.RightToolbarGUI.Add(OnRightToolbarGUI);
        }

        public static void OnUpdateBuild(bool prompt = true)
        {
            new AssetLabelReference();
            var path = ContentUpdateScript.GetContentStateDataPath(prompt);
            if (!string.IsNullOrEmpty(path)) {
                ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings, path);
            }
        }

        static void CleanAndVersionAddressablesFolder(string addressablesPath, string target = null)
        {
            if (!Directory.Exists(addressablesPath)) {
                Directory.CreateDirectory(addressablesPath);
            }

            //Clean up the addressable folder where assets are generated
            var di = new DirectoryInfo(addressablesPath);
            foreach (var file in di.GetFiles()) {
                file.Delete();
            }

            foreach (var dir in di.GetDirectories()) {
                dir.Delete(true);
            }

            //Create a versioning file which is basically just an empty file with necessary information in name
            //File.Create(IOUtility.CombinePaths(addressablesPath, $"{VersionTag}_{target}_{DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss", CultureInfo.InvariantCulture)}.versioning")).Close();
        }

        static void OnRightToolbarGUI()
        {
            if (GUILayout.Button(new GUIContent("Debug", "测试Prefab"), GUILayout.Width(50), GUILayout.Height(22))) {
                if (EditorUtility.DisplayDialog("暂停中...", "已暂停", "OK")) //显示对话框
                {
                    //testScript.MyTestA(); //点击第三个参数 “Yes”，调用该方法
                }

                //SceneHelper.StartScene("Assets/ToolbarExtender/Example/Scenes/Scene1.unity");
            }

            if (GUILayout.Button(new GUIContent("R", "重新生成 Prefab"), GUILayout.Width(30), GUILayout.Height(22))) {
                //if (ScenAction.TryGetValue(scene.path, out var action)) {
                SceneUtil.GetAllLoadedScenes().ForEach(scene => scene.RestorePrefabs());
            }

            if (GUILayout.Button(new GUIContent("U", "发布代码到测试服务器"), GUILayout.Width(30), GUILayout.Height(22))) {
                My.WSL("npm run deploy");

                //EditorSceneManager.OpenScene("Assets/Scenes/Arena.unity");

                // if (EditorUtility.DisplayDialog("暂停中...", "已暂停", "OK")) //显示对话框
                // {
                //     //testScript.MyTestA(); //点击第三个参数 “Yes”，调用该方法
                // }

                //SceneHelper.StartScene("Assets/ToolbarExtender/Example/Scenes/Scene1.unity");
            }

            GUILayout.FlexibleSpace();
        }

        public static void RunAddressableBuild()
        {
            ///{EditorUserBuildSettings.activeBuildTarget}
            CleanAndVersionAddressablesFolder(Application.dataPath + $"/../ServerData/public/data/1.0");
            OnUpdateBuild(false);
        }

        static IEnumerator DoBuild()
        {
            OnUpdateBuild(false);
            yield return null;
        }

        static IEnumerator DoRedisSync()
        {
            My.WSL(" artisan redis:sync");
            yield return null;
        }

        static void OnLeftToolbarGUI()
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("构建", "Build Addressable"), GUILayout.Width(50),
                GUILayout.Height(22))) {
                Core.isBuilding = true;
                if (EditorUtility.DisplayDialog("build", EditorUserBuildSettings.activeBuildTarget.ToString(), "yes",
                    "no")) {
                    CleanAndVersionAddressablesFolder(Application.dataPath +
                        $"/../ServerData/public/data/1.0/{EditorUserBuildSettings.activeBuildTarget}");
                    EditorCoroutineUtility.StartCoroutineOwnerless(DoRedisSync());
                    EditorCoroutineUtility.StartCoroutineOwnerless(DoBuild());
                }

                Debug.Log(EditorUserBuildSettings.activeBuildTarget);

                //My.WSL("npm run deploy");
                Core.isBuilding = false;
            }

            if (GUILayout.Button(new GUIContent("P", "暂停Editor界面"), GUILayout.Width(30), GUILayout.Height(22))) {
                if (EditorUtility.DisplayDialog("暂停中...", "已暂停", "OK")) //显示对话框
                {
                    //testScript.MyTestA(); //点击第三个参数 “Yes”，调用该方法
                }

                //SceneHelper.StartScene("Assets/ToolbarExtender/Example/Scenes/Scene1.unity");
            }

            // if(GUILayout.Button(new GUIContent("2", "Start Scene 2")) )
            // {
            //     //SceneHelper.StartScene("Assets/ToolbarExtender/Example/Scenes/Scene2.unity");
            // }
        }
    }
}
#endif