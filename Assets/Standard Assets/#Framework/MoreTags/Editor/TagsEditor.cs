using Sirenix.OdinInspector.Editor;
using System.Linq;
// using Unity.Entities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MoreTags {

[CanEditMultipleObjects, CustomEditor(typeof(Tags))]
public class TagsEditor : OdinEditor {

    TagGUI m_GameObjectTag;
    Tags tags => (Tags)target;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        //var tags = (Tags)target;
        var go = tags.gameObject;
        if (!go.scene.isLoaded) {
            EditorGUILayout.HelpBox(
                "Tags should be attached to on scene GameObject. Please use Prefab Tags for off scene GameObject.",
                MessageType.Warning);
            return;
        }


        //TagSystem.AddTag(go);
        var list = targets.Cast<Tags>().Select(item => item.gameObject);
        if (!list.Skip(1).Any()) {
            m_GameObjectTag.OnGUI(Extensions.GetTags(go).OrderBy(tag => TagPreset.GetTagOrder(tag)));
        } else {
            TagManagerEditor.ShowGameObjectsTag(list);
        }
        GUILayout.Space(10);
        GUILayout.Label($"Tags: " + string.Join(", ", tags.ids));
        base.OnInspectorGUI();
    }

    protected override void OnEnable()
    {
        //var tags = (Tags)target;
        var go = tags.gameObject;
        // if (!go.GetComponent<ConvertToEntity>()) {
        //     go.AddComponent<ConvertToEntity>();
        // }
        // (go.GetComponent<ConvertToEntity>() ?? go.AddComponent<ConvertToEntity>()).ConversionMode =
        //     ConvertToEntity.Mode.ConvertAndInjectGameObject;
        m_GameObjectTag = new TagGUI();

        //m_GameObjectTag.maxWidth
        //= EditorGUIUtility.currentViewWidth;
        m_GameObjectTag.OnItemString += TagManagerEditor.OnItemString;
        m_GameObjectTag.OnItemColor += TagManagerEditor.OnItemColor;
        m_GameObjectTag.OnRightClickItem += TagManagerEditor.OnRightClickItem;
        m_GameObjectTag.OnAddItem += (item) => {
            if (!string.IsNullOrEmpty(item)) {
                AddTag(go, item);
            } else {
                var menu = new GenericMenu();
                foreach (var tag in TagPreset.GetPresets()
                    .Union(TagSystem.Tags())
                    .Except(Extensions.GetTags(go))) {
                    menu.AddItem(new GUIContent(tag), false, () => AddTag(go, tag));
                }
                menu.ShowAsContext();
            }
        };
        m_GameObjectTag.OnClickItem += (item) => {
            RemoveTag(go, item);
        };
    }

    void RemoveTag(GameObject go, string tag)
    {
        go.RemoveTag(tag);
        EditorSceneManager.MarkSceneDirty(go.scene);
        Debug.Log("set dirty");
    }

    void AddTag(GameObject go, string tag)
    {
        if (!TagSystem.AllTags().Contains(tag)) {
            TagSystem.AddTag(tag);
            TagSystem.SetTagColor(tag, TagPreset.GetPresetColor(tag));
        }
        go.AddTag(tag);
        EditorSceneManager.MarkSceneDirty(go.scene);
        Debug.Log("set dirty");
    }

}

}