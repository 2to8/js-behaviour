﻿using System.Collections.Generic;
using Base.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PuertsExt
{
    [CustomEditor(typeof(JsBehaviour))]
    public partial class JsBehaviourEditor : UnityEditor.Editor
    {
        protected void OnEnable()
        {
            jsBehaviour = (target as JsBehaviour);
            if (jsBehaviour == null)
            {
                return;
            }
            jsEnvEditor = new JsEnvEditor.JsEnvEditor();
            componentInfo = jsEnvEditor.GetJsComponentInfo(jsBehaviour.JsComponentName);
            EditorApplication.update += Update;
        }

        protected void OnDisable()
        {
            jsEnvEditor.Dispose();
            EditorApplication.update -= Update;
        }

        protected void Update()
        {
            jsEnvEditor.Tick();
        }

        public override void OnInspectorGUI()
        {
            UpdateComponentName();
            if (componentInfo == null)
            {
                var text = string.IsNullOrEmpty(jsBehaviour.JsComponentName) ? "Js Component required !" : $"Unknown Js component of {jsBehaviour.JsComponentName} !";
                var icon = string.IsNullOrEmpty(jsBehaviour.JsComponentName) ? "console.warnicon.sml" : "console.erroricon.sml";
                EditorGUILayout.LabelField(EditorGUIUtility.TrTextContentWithIcon(text, icon));
                return;
            }
            foreach (var propertyInfo in componentInfo.properties.Values)
            {
                var propertyType = jsBehaviour.JsComponentProp.GetType(propertyInfo.type);
                if (propertyType == null)
                {
                    EditorGUILayout.LabelField(EditorGUIUtility.TrTextContentWithIcon($"Unknown type of {propertyInfo.type}", "console.erroricon.sml"));
                }
                if (propertyInfo.isArray)
                {
                    ShowArrayProperty(propertyInfo, propertyType);
                }
                else
                {
                    ShowSingleProperty(propertyInfo, propertyType);
                }
            }
        }

        private void UpdateComponentName()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(EditorGUIUtility.TrTextContent("Select Js Component")))
            {
                var dropdown = new SelectJsComponentDropdown(jsEnvEditor.ComponentInfos);
                dropdown.Show(new Rect(19, 25, 0, 0));
                dropdown.OnItemSelected += SetJsComponentName;
            }
            SetJsComponentName(EditorGUILayout.TextField(jsBehaviour.JsComponentName));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
        }

        private void SetJsComponentName(string newJsComponentName)
        {
            if (newJsComponentName == jsBehaviour.JsComponentName)
            {
                return;
            }
            jsBehaviour.JsComponentProp.Clear();
            jsBehaviour.JsComponentName = newJsComponentName;
            componentInfo = jsEnvEditor.GetJsComponentInfo(jsBehaviour.JsComponentName);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        private JsEnvEditor.JsEnvEditor jsEnvEditor;
        private JsBehaviour jsBehaviour;
        private ComponentInfo componentInfo;
        private readonly Dictionary<string, bool> showArrayStatus = new Dictionary<string, bool>();
    }
}
