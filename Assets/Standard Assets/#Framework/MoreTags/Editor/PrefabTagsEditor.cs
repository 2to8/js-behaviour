using Sirenix.OdinInspector.Editor;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MoreTags {

[CustomEditor(typeof(PrefabTags))]
public class PrefabTagsEditor : OdinEditor {

    TagGUI m_GameObjectTag;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var go = (target as PrefabTags).gameObject;
        if (go.scene.isLoaded) {
            EditorGUILayout.HelpBox(
                "Prefab Tags should not be attached to on scene GameObject. Please use Tags for on scene GameObject.",
                MessageType.Warning);
            return;
        }
        var tags = (target as PrefabTags).Tags;
        m_GameObjectTag.OnGUI(tags.OrderBy(tag => TagPreset.GetTagOrder(tag)));
        base.OnInspectorGUI();
    }

    protected override void OnEnable()
    {
        var go = (target as PrefabTags).gameObject;
        var tags = target as PrefabTags;
        m_GameObjectTag = new TagGUI();
        m_GameObjectTag.OnItemString += TagManagerEditor.OnItemString;
        m_GameObjectTag.OnItemColor += (item) => TagPreset.GetPresetColor(item);
        m_GameObjectTag.OnAddItem += (item) => {
            if (!string.IsNullOrEmpty(item)) {
                AddTag(tags, item);
            } else {
                var menu = new GenericMenu();
                foreach (var tag in TagPreset.GetPresets()
                    .Union(TagSystem.Tags())
                    .Except(Extensions.GetTags(go))) {
                    menu.AddItem(new GUIContent(tag), false, () => AddTag(tags, tag));
                }
                menu.ShowAsContext();
            }
        };
        m_GameObjectTag.OnClickItem += (item) => {
            tags.Tags.Remove(item);
        };
    }

    void AddTag(PrefabTags tags, string tag)
    {
        if (!tags.Tags.Contains(tag)) {
            tags.Tags.Add(tag);
        }
    }

}

}