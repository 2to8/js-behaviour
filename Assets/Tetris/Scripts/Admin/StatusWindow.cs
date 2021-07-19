#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Admin
{
    public class StatusWindow : StateWindow<Status>
    {
        [MenuItem("Debug/Status Editor")]
        static void OpenWindow()
        {
            var window = GetWindow<StatusWindow>();
            window.titleContent = new GUIContent(nameof(Admin.Status));
            window.Show();
        }
    }
}
#endif