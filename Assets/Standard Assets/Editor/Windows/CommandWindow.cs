using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Windows {

public class CommandWindow : OdinEditorWindow {

    [MenuItem("Window/My Editor")]
    private static void OpenWindow()
    {
        var win = GetWindow<CommandWindow>();
        win.titleContent = new GUIContent("#");
        win.Show();
    }

    protected override void Initialize()
    {
        this.WindowPadding = Vector4.zero;
    }

    protected override object GetTarget()
    {
        return Selection.activeObject;
    }

    public string Hello;
}

}