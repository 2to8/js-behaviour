#if UNITY_EDITOR
using UnityEditor;

namespace Common.EditorMenu {

public class PuertsDebug {

    //const string kConfigName = "Puerts.DebugMode";
    const string kMenuText = "Puerts/Enable Debug Mode";

    public static bool DebugMode {
        get => EditorPrefs.GetBool(kMenuText);
        set => EditorPrefs.SetBool(kMenuText, value);
    }

    [MenuItem(kMenuText, false, 201)]
    static void EnableDebug()
    {
        // Check/Uncheck menu.
        var isChecked = !Menu.GetChecked(kMenuText);
        Menu.SetChecked(kMenuText, isChecked);

        // Save to EditorPrefs.
        EditorPrefs.SetBool(kMenuText, isChecked);

        //DebugMode = true;
    }

    [MenuItem(kMenuText, true, 201)]
    static bool EnableDebugCheck()
    {
        // Check/Uncheck menu from EditorPrefs.
        Menu.SetChecked(kMenuText, EditorPrefs.GetBool(kMenuText, false));

        return true;
    }

    // [MenuItem("Puerts/Disable Debug Mode")]
    // static void DisableDebug() => DebugMode = false;
    //
    // [MenuItem("Puerts/Disable Debug Mode", true)]
    // static bool DisableDebugCheck() => DebugMode;

}

}
#endif