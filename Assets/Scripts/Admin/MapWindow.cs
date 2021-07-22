#if UNITY_EDITOR
using Consts;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Tetris.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Admin
{
    public class MapWindow : OdinEditorWindow
    {
        [ShowInInspector]
        [ShowIf(nameof(isMain))]
        public int[,] data => Map.instance.data;

        bool isMain => SceneManager.GetActiveScene().name == $"{SceneName.Main}";

        [MenuItem("Debug/Map Editor")]
        static void OpenWindow()
        {
            var window = GetWindow<MapWindow>();
            window.titleContent = new GUIContent("Map");
            window.Show();
        }
    }
}
#endif