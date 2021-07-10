#if UNITY_EDITOR
using UnityEditor.iOS.Extensions.Common;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameEngine.Extensions
{
    public static partial class SceneExtensions
    {
        public static void SetDirty(this Scene scene)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                EditorSceneManager.MarkSceneDirty(scene);
            }
#endif
        }
    }
}