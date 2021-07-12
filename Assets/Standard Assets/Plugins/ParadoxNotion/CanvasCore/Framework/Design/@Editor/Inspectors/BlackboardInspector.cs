#if UNITY_EDITOR

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;


namespace NodeCanvas.Editor
{

    [CustomEditor(typeof(Blackboard))]
    public class BlackboardInspector : OdinEditor
    {

        private Blackboard bb { get { return (Blackboard)target; } }

        private SerializedProperty parentBlackboardProp;

        void OnEnable() {
            parentBlackboardProp = serializedObject.FindProperty("_parentBlackboard");
        }

        public override void OnInspectorGUI() {
            GUI.color = GUI.color.WithAlpha(parentBlackboardProp.objectReferenceValue ? 1 : 0.6f);
            EditorGUILayout.PropertyField(parentBlackboardProp, EditorUtils.GetTempContent("Parent Asset Blackboard", null, "Optional Parent Asset Blackboard to 'inherit' variables from."));
            serializedObject.ApplyModifiedProperties();
            GUI.color = Color.white;

            BlackboardEditor.ShowVariables(bb);       

            UnityEditor.EditorGUILayout.HelpBox( "DrawDefaultInspector() 关键: 显示默认字段, 支持 odin", UnityEditor.MessageType.Info
            );

            // Show default inspector property editor
            // TODO:MO 关键: 显示默认字段, 支持odin
            //DrawDefaultInspector();
            base.OnInspectorGUI();

            EditorUtils.EndOfInspector();
            if ( Event.current.isMouse ) {
                Repaint();
            }
        }
    }
}

#endif