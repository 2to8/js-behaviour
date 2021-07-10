using GameEngine.Extensions;
using MoreTags;
using Sirenix.Utilities;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
// using WebSocketSharp;

namespace Menus {

public static class PlayFromTheFirstScene {
    const string playFromFirstMenuStr = "Edit/Always Start From Scene 0 &p";

    static bool playFromFirstScene {
        get { return EditorPrefs.HasKey(playFromFirstMenuStr) && EditorPrefs.GetBool(playFromFirstMenuStr); }
        set { EditorPrefs.SetBool(playFromFirstMenuStr, value); }
    }

    [MenuItem(playFromFirstMenuStr, false, 150)]
    static void PlayFromFirstSceneCheckMenu()
    {
        playFromFirstScene = !playFromFirstScene;
        Menu.SetChecked(playFromFirstMenuStr, playFromFirstScene);

        ShowNotifyOrLog(playFromFirstScene ? "Play from scene 0" : "Play from current scene");
    }

    [InitializeOnLoadMethod]
    static void SaveBeforeOpenScene()
    {
        EditorApplication.playModeStateChanged += change => {
            if (change == PlayModeStateChange.ExitingEditMode) {
                var stage = PrefabStageUtility.GetCurrentPrefabStage();

                if (stage == null) return;

                //Debug.Log("stage: " + stage.prefabContentsRoot.GetTags().Join().ToGreen());
                // Debug.Log("stage: " + stage.prefabContentsRoot.AssetPath().ToGreen());
                // EditorUtility.DisplayDialog("check", stage.prefabContentsRoot.GetTags().Join(), "ok");


                stage.prefabContentsRoot.SyncAssets(stage.assetPath);
                // stage.prefabContentsRoot.GetComponentsInChildren<Transform>(true)
                //     .Where(go => PrefabUtility.IsOutermostPrefabInstanceRoot(go.gameObject))
                //     .ForEach(go => {
                //         go.gameObject?.SyncAssets(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go));
                //     });

                if (stage.scene.isDirty) {
                    var check = EditorUtility.DisplayDialog("save",
                        stage.prefabContentsRoot.name + " => " + stage.assetPath, "ok", "cancel");

                    if (check) {
                        var fn = stage.GetType()
                            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                            .FirstOrDefault(mb => mb.Name == "Save");
                        fn!.Invoke(stage, null);
                    } else {
                        EditorApplication.isPlaying = false;
                    }
                }
            }
        };
    }

    // The menu won't be gray out, we use this validate method for update check state
    [MenuItem(playFromFirstMenuStr, true)]
    static bool PlayFromFirstSceneCheckMenuValidate()
    {
        Menu.SetChecked(playFromFirstMenuStr, playFromFirstScene);

        return true;
    }

    // This method is called before any Awake. It's the perfect callback for this feature
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadFirstSceneAtGameBegins()
    {
        if (!playFromFirstScene) return;

        if (EditorBuildSettings.scenes.Length == 0) {
            Debug.LogWarning("The scene build list is empty. Can't play from first scene.");

            return;
        }

        var path = "Assets/Prefabs/SandBox/TestPrefabScene.unity";

        if (SceneManager.GetActiveScene().path != path) {
            foreach (GameObject go in Object.FindObjectsOfType<GameObject>()) go.SetActive(false);

            if (Application.isPlaying) {
                SceneManager.LoadScene(path);
            } else {
                EditorSceneManager.OpenScene(path);
            }
        } else {
            var prefabPath = PrefabStageUtility.GetCurrentPrefabStage()?.assetPath;

            if (!prefabPath.IsNullOrEmpty()) {
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                Object.Instantiate(go);
            }
        }

        //SceneManager.LoadScene(0);
    }

    static void ShowNotifyOrLog(string msg)
    {
        if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
            EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
        else
            Debug.Log(msg); // When there's no scene view opened, we just print a log
    }
}

}