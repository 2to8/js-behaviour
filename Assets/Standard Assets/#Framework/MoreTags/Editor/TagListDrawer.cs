using GameEngine.Kernel;
using MoreTags.TagHelpers;
using MoreTags.TagHelpers.Extensions;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MoreTags {

[CustomPropertyDrawer(typeof(TagList))]
public class TagListDrawer : PropertyDrawer {

    TagGUI m_GameObjectTag;
    TagList tagList = new TagList();

    //List<string> tags = new List<string>() { };

    // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    // {
    //     // The 6 comes from extra spacing between the fields (2px each)
    //     return EditorGUIUtility.singleLineHeight;
    // }

    void OnEnable()
    {
        //var go = (target as Tags).gameObject;
        m_GameObjectTag = new TagGUI();
        m_GameObjectTag.OnItemString += TagManagerEditor.OnItemString;
        m_GameObjectTag.OnItemColor += TagManagerEditor.OnItemColor;
        m_GameObjectTag.OnRightClickItem += TagManagerEditor.OnRightClickItem;
        m_GameObjectTag.OnClickItem += (item) => {
            RemoveTag(item);
            makeAddMenu();
        };
        makeAddMenu();
    }

    void makeAddMenu()
    {
        m_GameObjectTag.OnAddItem += (item) => {
            if (!string.IsNullOrEmpty(item)) {
                AddTag(item);
            } else {
                var menu = new GenericMenu();
                foreach (var tag in TagPreset.GetPresets()
                    .Union(TagSystem.Tags())
                    .Except(tagList.tags)) {
                    menu.AddItem(new GUIContent(tag), false, () => AddTag(tag));
                }
                menu.ShowAsContext();
            }
        };
    }

    void RemoveTag(string tag)
    {
        TagSystem.RemoveTag(tag);
        if (tagList.tags.Contains(tag)) {
            tagList.tags.Remove(tag);
        }

        //tagList.tags.RemoveUnique(tag);
    }

    void AddTag(string tag)
    {
        if (!TagSystem.AllTags().Contains(tag)) {
            TagSystem.AddTag(tag);
            TagSystem.SetTagColor(tag, TagPreset.GetPresetColor(tag));
        }
        TagSystem.AddTag(tag);
        if (!tagList.tags.Contains(tag)) {
            tagList.tags.Add(tag);
        }

        //tagList.tags.AddUnique(tag);

        //EditorSceneManager.MarkSceneDirty(go.scene);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position,property,label);
        if (m_GameObjectTag == null) {
            OnEnable();

            //m_GameObjectTag.indentLeft = position.xMin + 10f;
        }

        //if (property?.serializedObject?.targetObject != null && fieldInfo != null) {
        tagList = fieldInfo.GetValue(property.serializedObject.targetObject) as TagList;

        //if (tagList != null) {
        //}
        //}

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, label);

        // Make child fields be indented
        EditorGUI.indentLevel++;

        // background
        GUILayout.BeginVertical("box");

        // Don't make child fields be indented
        //var indent = EditorGUI.indentLevel;
        //EditorGUI.indentLevel += 2;
        m_GameObjectTag.maxWidth = EditorGUIUtility.currentViewWidth - 300; //;
        EditorGUI.BeginChangeCheck();
        m_GameObjectTag.OnGUI(tagList.tags.OrderBy(tag => TagPreset.GetTagOrder(tag)), " ");
        if (EditorGUI.EndChangeCheck()) {
            DB.Update(tagList);
            Debug.Log("saved");
        }
        GUILayout.EndVertical();

        // Set indent back to what it was
        EditorGUI.indentLevel--;

        //
        //
        // Set indent back to what it was
        //EditorGUI.indentLevel -= 2;
        EditorGUI.EndProperty();

        // Draw label
        //EditorGUI.PropertyField(position, property, label, true);
        //EditorGUI.EndProperty();
    }

}

}