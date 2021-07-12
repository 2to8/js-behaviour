#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ParadoxNotion.Design
{

    ///A generic popup editor
    public class GenericInspectorWindow : OdinEditorWindow
    {

        private static GenericInspectorWindow current;

        private string friendlyTitle;
        private System.Type targetType;
        private Object unityObjectContext;
        private System.Func<object> read;
        private System.Action<object> write;
        private Vector2 scrollPos;
        private bool willRepaint;
        private EdtDummy dummy;

        // ...
        protected override void OnEnable() {
            base.OnEnable();

            titleContent = new GUIContent("Object Editor");
            current = this;

#if UNITY_2017_2_OR_NEWER
            EditorApplication.playModeStateChanged -= PlayModeChange;
            EditorApplication.playModeStateChanged += PlayModeChange;
#else
        	EditorApplication.playmodeStateChanged -= PlayModeChange;
            EditorApplication.playmodeStateChanged += PlayModeChange;
#endif
            dummy = ScriptableObject.CreateInstance<EdtDummy>();

        }

        //...
        void OnDisable() {
#if UNITY_2017_2_OR_NEWER
            EditorApplication.playModeStateChanged -= PlayModeChange;
#else
        	EditorApplication.playmodeStateChanged -= PlayModeChange;
#endif
        }

#if UNITY_2017_2_OR_NEWER
        void PlayModeChange(PlayModeStateChange state) { Close(); }
#else
        void PlayModeChange(){ Close(); }
#endif

        ///Open utility window to inspect target object of type in context using read/write delegates.
        public static void Show(string title, System.Type targetType, Object unityObjectContext, System.Func<object> read, System.Action<object> write) {
            var window = current != null ? current : CreateInstance<GenericInspectorWindow>();
            window.friendlyTitle = title;
            window.targetType = targetType;
            window.unityObjectContext = unityObjectContext;
            window.write = write;
            window.read = read;
            window.ShowUtility();
        }

        //...
        void Update() {
            if ( willRepaint ) {
                willRepaint = false;
                Repaint();
            }
        }

        //...
        protected override void OnGUI() {

            base.OnGUI();

            if ( targetType == null ) {
                return;
            }

            var e = Event.current;
            if ( e.type == EventType.ValidateCommand && e.commandName == "UndoRedoPerformed" ) {
                GUIUtility.hotControl = 0;
                GUIUtility.keyboardControl = 0;
                e.Use();
                return;
            }

            GUILayout.Space(10);
            GUILayout.Label(string.Format("<size=14><b>{0}</b></size>", targetType.FriendlyName()), Styles.centerLabel);
            EditorUtils.Separator();
            GUILayout.Space(10);
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            var serializationInfo = new InspectedFieldInfo(unityObjectContext, null, null, null);
            var oldValue = read();


        if ( ((IList) new[] {typeof(AssetReference)}).Contains( targetType ) ) {
            dummy.value = ( AssetReference )oldValue;
        }  else if ( targetType == typeof( AssetLabelReference ) ) {
            dummy.assetLabelReference = ( AssetLabelReference )oldValue;
            dummy.sp = dummy.so.FindProperty( "assetLabelReference");
        }  else if ( typeof( AssetReferenceGameObject ) == targetType ) {
            dummy.assetReferenceGameObject = ( AssetReferenceGameObject )oldValue;
            dummy.sp = dummy.so.FindProperty( "assetReferenceGameObjec" );
        }
        else if ( typeof( AssetLabelReference [] ) == targetType ) {
            dummy.assetLabelReferences = new List<AssetLabelReference>();

            (( AssetLabelReference [] )oldValue)?.ForEach( l => dummy.assetLabelReferences.Add( l ) );

            //dummy.so = new SerializedObject( dummy )
            dummy.sp = dummy.so.FindProperty( "assetLabelReferences");
           // Debug.Log( "test" );
        }
        //
        // else if ( targetType == typeof( AssetLabelReference [] ) ) {
        //     dummy.assetLabelReferences = (AssetLabelReference []) oldValue;
        // } else if (targetType  == typeof(AssetLabelReference)) {
        //     dummy.assetLabelReference = ( AssetLabelReference )oldValue;
        // }else if (targetType  == typeof(AssetReferenceGameObject)) {
        //     dummy.assetReferenceGameObject = ( AssetReferenceGameObject )oldValue;
        // }
        else if (targetType  == typeof(AssetReferenceTexture)) {
            dummy.assetReferenceTexture= ( AssetReferenceTexture )oldValue;
            dummy.sp = dummy.so.FindProperty( "assetReferenceTexture");
        }else if (targetType  == typeof(AssetReferenceTexture2D)) {
            dummy.assetReferenceTexture2D = ( AssetReferenceTexture2D )oldValue;
            dummy.sp = dummy.so.FindProperty( "assetReferenceTexture2D");
        }else if (targetType  == typeof(AssetReferenceTexture3D)) {
            dummy.assetReferenceTexture3D = ( AssetReferenceTexture3D )oldValue;
            dummy.sp = dummy.so.FindProperty( "assetReferenceTexture3D");

        }if (targetType  == typeof(AssetReferenceSprite)) {
            dummy.assetReferenceSprite = ( AssetReferenceSprite )oldValue;
            dummy.sp = dummy.so.FindProperty( "assetReferenceSprite");

        }


            var newValue = EditorUtils.ReflectedFieldInspector(friendlyTitle, oldValue, targetType, serializationInfo, dummy);
            if ( !Equals(oldValue, newValue) || GUI.changed ) {
                write(newValue);
            }
            GUILayout.EndScrollView();

            willRepaint = true;
        }
    }
}

#endif