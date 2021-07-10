using MoreTags;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Common {

[CreateAssetMenu(fileName = nameof(EventTriggerData), menuName = "Scriptable/" + nameof(EventTriggerData))]
public class EventTriggerData : SerializedScriptableObject {

    [HideInInspector]
    public UnityAction<float> singleEvent;

    [OdinSerialize, HideInInspector]
    public FieldInfo fieldInfo;

    [OdinSerialize, HideInInspector]
    public PropertyInfo propertyInfo;

#if UNITY_EDITOR
    [HideInInspector]
    public UnityEditor.Editor editor;
#endif

    // [ValueDropdown("TextureSizes")]
    // public int SomeSize1;
    //
    // [ValueDropdown("FriendlyTextureSizes")]
    // public int SomeSize2;

    // [ValueDropdown("FriendlyTextureSizes", AppendNextDrawer = true, DisableGUIInAppendedDrawer = true)]
    // public int SomeSize3;
    //
    // [ValueDropdown("GetListOfMonoBehaviours", AppendNextDrawer = true)]
    // public MonoBehaviour SomeMonoBehaviour;
    //
    // [ValueDropdown("KeyCodes")]
    // public KeyCode FilteredEnum;

    [OdinSerialize, HideInInspector]
    List<(int, int)> tags = new List<(int, int)>();

    public GameObject target;

    [OdinSerialize, ValueDropdown("TreeViewOfInts", ExpandAllMenuItems = true)]
    public List<(int, int)> IntTreview {
        get {
            if (tags == null) {
                tags = new List<(int, int)>();
            }

            return tags;
        }
        set => tags = value;
    }

    // [ValueDropdown("GetAllSceneObjects", IsUniqueList = true)]
    // public List<GameObject> UniqueGameobjectList;

    // [ValueDropdown("GetAllSceneObjects", IsUniqueList = true, DropdownTitle = "Select Scene Object", DrawDropdownForListElements = false, ExcludeExistingValuesInList = true)]
    // public List<GameObject> UniqueGameobjectListMode2;

    IEnumerable TreeViewOfInts {
        get {
            var ret = new ValueDropdownList<(int, int)>();
            TagSystem.GetByName.ForEach(t => {
                ret.Add(new ValueDropdownItem<(int, int)>(t.Key.Replace(".", "/"), (t.Value.Id, -1)));
            });

            // {
            //     { "Node 1/Node 1.1", (1, -1) },
            //     { "Node 1/Node 1.2", (2, -1) },
            //     { "Node 2/Node 2.1", (3, -1) },
            //     { "Node 3/Node 3.1", (4, -1) },
            //     { "Node 3/Node 3.2", (5, -1) },
            //     { "Node 1/Node 3.1/Node 3.1.1", (6, -1) },
            //     { "Node 1/Node 3.1/Node 3.1.2", (7, -1) },
            // };
            return ret;
        }
    }

    // private IEnumerable<MonoBehaviour> GetListOfMonoBehaviours()
    // {
    //     return GameObject.FindObjectsOfType<MonoBehaviour>();
    // }

    // private static IEnumerable<KeyCode> KeyCodes = Enumerable.Range((int)KeyCode.Alpha0, 10).Cast<KeyCode>();

    // private static IEnumerable GetAllSceneObjects()
    // {
    //     Func<Transform, string> getPath = null;
    //     getPath = x => (x ? getPath(x.parent) + "/" + x.gameObject.name : "");
    //     return GameObject.FindObjectsOfType<GameObject>().Select(x => new ValueDropdownItem(getPath(x.transform), x));
    // }

    // private static IEnumerable GetAllScriptableObjects()
    // {
    //     return UnityEditor.AssetDatabase.FindAssets("t:ScriptableObject")
    //         .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
    //         .Select(x => new ValueDropdownItem(x, UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(x)));
    // }

    // private static IEnumerable GetAllSirenixAssets()
    // {
    //     var root = "Assets/Plugins/Sirenix/";
    //
    //     return UnityEditor.AssetDatabase.GetAllAssetPaths()
    //         .Where(x => x.StartsWith(root))
    //         .Select(x => x.Substring(root.Length))
    //         .Select(x => new ValueDropdownItem(x, UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(root + x)));
    // }

    // private static IEnumerable FriendlyTextureSizes = new ValueDropdownList<int>()
    // {
    //     { "Small", 256 },
    //     { "Medium", 512 },
    //     { "Large", 1024 },
    // };

    // private static int[] TextureSizes = new int[] { 256, 512, 1024 };
    public void testEvent(float value) { }

}

}