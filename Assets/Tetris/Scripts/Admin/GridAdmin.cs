#if UNITY_EDITOR
using Consts;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Tetris.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Grid = Tetris.Grid;

namespace Admin
{
    public class GridAdmin : OdinEditorWindow
    {
        [ShowInInspector]
        [ShowIf(nameof(isMain))]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = true)]
        public Grid data => Grid.instance;

        bool isMain => SceneManager.GetActiveScene().name == $"{SceneName.Main}";

        [MenuItem("Debug/Grid Editor")]
        static void OpenWindow()
        {
            var window = GetWindow<GridAdmin>();
            window.titleContent = new GUIContent("Grid");
            window.Show();
        }
    }
}
#endif