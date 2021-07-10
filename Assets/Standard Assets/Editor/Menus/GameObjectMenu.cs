using MoreTags;
using UnityEditor;
using UnityEngine;

namespace Menus {

public class GameObjectMenu {

    const string title = "GameObject/== Select ======/";
    const string bind = "GameObject/Binding/";

    [MenuItem(title + "测试", priority = -201, validate = false)]
    static void BindTestMenu() { }

    [MenuItem(bind + "测试", priority = 1, validate = false)]
    static void BindTestMenu2() { }

    [MenuItem("Tests/check asset")]
    static void CheckAsset()
    {
        var t = Core.CheckLoadAsset<TagManager>();
        Debug.Log(t == null);
    }
}

}