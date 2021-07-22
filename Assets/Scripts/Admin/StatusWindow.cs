#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Admin
{
    public class StatusWindow : StateWindow<Status>
    {
        [MenuItem("Window/[Status] Window")]
        static void OpenWindow()
        {
            var window = GetWindow<StatusWindow>();
            window.titleContent = new GUIContent(nameof(Admin.Status));
            window.Show();
        }
    }
}
#endif