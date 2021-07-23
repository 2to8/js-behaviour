#if UNITY_EDITOR
using Puerts.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Common.PropertyDrawers
{
    [PuertsIgnore, CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class SScriptableObjectDrawer : PropertyDrawer
    {
        // Cached scriptable object editor
        Editor editor;
        const int buttonWidth = 18;

        Type GetFieldType()
        {
            var type = fieldInfo.FieldType;
            if (type.IsArray) {
                type = type.GetElementType();
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) {
                type = type.GetGenericArguments()[0];
            }

            return type;
        }

        // Creates a new ScriptableObject via the default Save File panel
        static ScriptableObject CreateAssetWithSavePrompt(Type type, string path)
        {
            path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", type.Name + ".asset", "asset",
                "Enter a file name for the ScriptableObject.", path);
            if (path == "") {
                return null;
            }

            var asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            EditorGUIUtility.PingObject(asset);
            return asset;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //
            // if(GetFieldType().IsDefined(typeof(DontDrawThisAttribute), true)) {
            //     return;
            // }

            // Draw label
            EditorGUI.PropertyField(position, property, label, true);

            // Draw foldout arrow
            if (property.objectReferenceValue != null) {
                property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
            }
            else {
                var buttonRect =
                    new Rect(position.x + EditorGUIUtility.labelWidth /* + position.width-buttonWidth-20*/ - 16,
                        position.y, buttonWidth, EditorGUIUtility.singleLineHeight);
                if (fieldInfo != null) {
                    if (GUI.Button(buttonRect, "N")) {
                        // Debug.Log("test");
                        //property.propertyType.
                        //Core.Dialog(fieldInfo.FieldType.FullName);
                        var obj = ScriptableObject.CreateInstance(GetFieldType());
                        var filename = Core.TempFilename();
                            //Path.ChangeExtension(Path.GetRandomFileName(), "asset");
                        var path = $"Assets/Settings/{GetFieldType().Name}-{filename}";
                        AssetDatabase.CreateAsset(obj, path);
                        AssetDatabase.SaveAssets();
                        property.objectReferenceValue = AssetDatabase.LoadAssetAtPath(path, GetFieldType());
                        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
                    }
                }
            }

            // Draw foldout properties
            if (property.isExpanded && property.objectReferenceValue != null) {
                // Make child fields be indented
                //EditorGUI.indentLevel++;

                // background
                GUILayout.BeginVertical("box");
                if (!editor) {
                    Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);
                }

                // Draw object properties
                EditorGUI.BeginChangeCheck();
                if (editor) // catch empty property
                {
                    editor.OnInspectorGUI();
                }

                if (EditorGUI.EndChangeCheck()) {
                    property.serializedObject.ApplyModifiedProperties();
                }

                GUILayout.EndVertical();

                // Set indent back to what it was
                //EditorGUI.indentLevel--;
            }
            else {
                editor = null;
            }
        }
    }
}

#endif