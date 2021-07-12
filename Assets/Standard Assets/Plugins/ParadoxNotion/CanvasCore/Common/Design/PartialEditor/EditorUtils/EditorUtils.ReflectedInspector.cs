#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityObject = UnityEngine.Object;

namespace ParadoxNotion.Design
{
    ///Automatic Inspector functions
    partial class EditorUtils
    {
        private static GUIContent tempContent;

        ///A cached temporary content
        public static GUIContent GetTempContent(string text = "", Texture image = null, string tooltip = null)
        {
            if (tempContent == null) {
                tempContent = new GUIContent();
            }

            tempContent.text = text;
            tempContent.image = image;
            tempContent.tooltip = tooltip;
            return tempContent;
        }

        ///A cached temporary content
        public static GUIContent GetTempContent(Texture image = null, string tooltip = null)
        {
            return GetTempContent(null, image, tooltip);
        }

        public static Node currentNode;

        ///Show an automatic editor GUI inspector for target object, taking into account drawer attributes
        public static void ReflectedObjectInspector(object target, UnityObject unityObjectContext)
        {
            if (target == null) {
                return;
            }

            var fields = target.GetType().RTGetFields();
            for (var i = 0; i < fields.Length; i++) {
                var field = fields[i];

                //no statics
                if (field.IsStatic) {
                    continue;
                }

                //hide type altogether?
                if (field.FieldType.RTIsDefined(typeof(HideInInspector), true)) {
                    continue;
                }

                //inspect only public fields or private fields with the [ExposeField] attribute
                if (field.IsPublic || field.RTIsDefined(typeof(ExposeFieldAttribute), true)) {
                    var attributes = field.RTGetAllAttributes();
                    //Hide field?
                    if (attributes.Any(a => a is HideInInspector)) {
                        continue;
                    }

                    var serializationInfo = new InspectedFieldInfo(unityObjectContext, field, target, attributes);
                    var currentValue = field.GetValue(target);

                    // fix: bbparameter 添加 node 支持
                    if (typeof(BBParameter).IsAssignableFrom(field.FieldType)) {
                        if (currentValue is BBParameter bb && currentNode != null) {
                            if (bb.node == null) {
                                Debug.Log($"{field.Name} value {currentValue}" +
                                    $" {target.GetType().GetNiceFullName()} {currentNode.name}");
                            }

                            bb.node = currentNode;
                        }
                    }

                    //end
                    var newValue = ReflectedFieldInspector(new GUIContent(field.Name), currentValue, field.FieldType,
                        serializationInfo);
                    var changed = !object.Equals(newValue, currentValue);
                    if (changed) {
                        UndoUtility.RecordObject(unityObjectContext, field.Name);
                    }

                    if (changed || field.FieldType.IsValueType) {
                        field.SetValue(target, newValue);
                    }

                    if (changed) {
                        UndoUtility.SetDirty(unityObjectContext);
                    }
                }
            }
        }

        ///Draws an Editor field for object of type directly WITH taking into acount object drawers and drawer attributes
        public static object ReflectedFieldInspector(string name, object value, Type t, InspectedFieldInfo info,
            EdtDummy dummy)
        {
            var content = GetTempContent(name.SplitCamelCase());
            if (info.attributes != null) {
                ///Create proper GUIContent
                var nameAtt = info.attributes.FirstOrDefault(a => a is NameAttribute) as NameAttribute;
                if (nameAtt != null) {
                    content.text = nameAtt.name;
                }

                var tooltipAtt = info.attributes.FirstOrDefault(a => a is TooltipAttribute) as TooltipAttribute;
                if (tooltipAtt != null) {
                    content.tooltip = tooltipAtt.tooltip;
                }
            }

            return ReflectedFieldInspector(content, value, t, info, dummy);
        }

        ///Draws an Editor field for object of type directly WITH taking into acount object drawers and drawer attributes
        public static object ReflectedFieldInspector(GUIContent content, object value, Type t, InspectedFieldInfo info,
            EdtDummy dummy)
        {
            if (t == null) {
                GUILayout.Label("NO TYPE PROVIDED!");
                return value;
            }

            var newValue = value;

            // if(t != typeof(AssetReference)) {
            //
            // }
            if (new[] {
                typeof(AssetReference), typeof(AssetLabelReference), typeof(AssetReferenceGameObject),
                typeof(AssetLabelReference[])
            }.Contains(t)) {
                dummy.so.Update();
                EditorGUILayout.PropertyField(dummy.sp);
                dummy.so.ApplyModifiedProperties();
                if (t == typeof(AssetReference)) {
                    newValue = dummy.value;
                }
                else if (t == typeof(AssetLabelReference[])) {
                    newValue = dummy.assetLabelReferences.ToArray();
                }
                else if (t == typeof(AssetLabelReference)) {
                    newValue = dummy.assetLabelReference;
                }
                else if (t == typeof(AssetReferenceGameObject)) {
                    newValue = dummy.assetReferenceGameObject;
                }
                else if (t == typeof(AssetReferenceTexture)) {
                    newValue = dummy.assetReferenceTexture;
                }
                else if (t == typeof(AssetReferenceSprite)) {
                    newValue = dummy.assetReferenceSprite;
                }
                else if (t == typeof(AssetReferenceTexture3D)) {
                    newValue = dummy.assetReferenceTexture3D;
                }
                else if (t == typeof(AssetReferenceTexture2D)) {
                    newValue = dummy.assetReferenceTexture3D;
                }
            }
            else {
                if (typeof(ScriptableObject).IsAssignableFrom(t) && false) {
                    var obj = (ScriptableObject) value;
                    if (!info.editor) {
                        if (obj == null) {
                            obj = ScriptableObject.CreateInstance(t);
                        }

                        UnityEditor.Editor.CreateCachedEditor(obj, null, ref info.editor);
                    }

                    info.editor.OnInspectorGUI();
                    newValue = obj;
                }
                else {
                    ///Use drawers
                    var objectDrawer = PropertyDrawerFactory.GetObjectDrawer(t);
                    if (value == null && typeof(ScriptableObject).IsAssignableFrom(t)) {
                        value = ScriptableObject.CreateInstance(t);
                    }
                    else if (value == null && !typeof(Component).IsAssignableFrom(t) && !t.IsGenericType) {
                        // fix: 可能是错的, 如果不加, string 类型会报错, 不能直接用Activator.CreateInstance(t);
                        object obj = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(t);
                        if (obj == null) {
                            value = Activator.CreateInstance(t);
                        }
                        else {
                            value = obj;
                        }
                    }

                    newValue = objectDrawer.DrawGUI(content, value, info);
                }
            }

            var changed = !object.Equals(newValue, value);
            if (changed) {
                UndoUtility.RecordObjectComplete(info.unityObjectContext, content.text + "Field Change");
            }

            value = newValue;
            if (changed) {
                UndoUtility.SetDirty(info.unityObjectContext);
            }

            return value;
        }

        ///Draws an Editor field for object of type directly WITH taking into acount object drawers and drawer attributes
        public static object ReflectedFieldInspector(GUIContent content, object value, Type t, InspectedFieldInfo info)
        {
            if (t == null) {
                GUILayout.Label("NO TYPE PROVIDED!");
                return value;
            }

            var newValue = value;
            if (new[] {
                typeof(AssetReference), typeof(AssetLabelReference), typeof(AssetReferenceGameObject),
                typeof(AssetLabelReference[])
            }.Contains(t)) {
                //GUILayout.Label(content);
                var dummy = info.dummy;
                if ((new[] {typeof(AssetReference)}).Contains(t)) {
                    dummy.value = (AssetReference) value;
                }
                else if (t == typeof(AssetLabelReference)) {
                    dummy.assetLabelReference = (AssetLabelReference) value;
                    dummy.sp = dummy.so.FindProperty("assetLabelReference");
                }
                else if (typeof(AssetReferenceGameObject) == t) {
                    dummy.assetReferenceGameObject = (AssetReferenceGameObject) value;
                    dummy.sp = dummy.so.FindProperty("assetReferenceGameObject");
                }
                else if (typeof(AssetLabelReference[]) == t) {
                    dummy.assetLabelReferences = new List<AssetLabelReference>();
                    ((AssetLabelReference[]) value)?.ForEach(l => dummy.assetLabelReferences.Add(l));

                    //dummy.so = new SerializedObject( dummy )
                    dummy.sp = dummy.so.FindProperty("assetLabelReferences");
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
                else if (t == typeof(AssetReferenceTexture)) {
                    dummy.assetReferenceTexture = (AssetReferenceTexture) value;
                    dummy.sp = dummy.so.FindProperty("assetReferenceTexture");
                }
                else if (t == typeof(AssetReferenceTexture2D)) {
                    dummy.assetReferenceTexture2D = (AssetReferenceTexture2D) value;
                    dummy.sp = dummy.so.FindProperty("assetReferenceTexture2D");
                }
                else if (t == typeof(AssetReferenceTexture3D)) {
                    dummy.assetReferenceTexture3D = (AssetReferenceTexture3D) value;
                    dummy.sp = dummy.so.FindProperty("assetReferenceTexture3D");
                }

                if (t == typeof(AssetReferenceSprite)) {
                    dummy.assetReferenceSprite = (AssetReferenceSprite) value;
                    dummy.sp = dummy.so.FindProperty("assetReferenceSprite");
                }

                dummy.so.Update();
                EditorGUILayout.PropertyField(dummy.sp,content);
                dummy.so.ApplyModifiedProperties();
                //info.dummy = dummy;
                if (t == typeof(AssetReference)) {
                    newValue = dummy.value;
                }
                else if (t == typeof(AssetLabelReference[])) {
                    newValue = dummy.assetLabelReferences.ToArray();
                }
                else if (t == typeof(AssetLabelReference)) {
                    newValue = dummy.assetLabelReference;
                }
                else if (t == typeof(AssetReferenceGameObject)) {
                    newValue = dummy.assetReferenceGameObject;
                }
                else if (t == typeof(AssetReferenceTexture)) {
                    newValue = dummy.assetReferenceTexture;
                }
                else if (t == typeof(AssetReferenceSprite)) {
                    newValue = dummy.assetReferenceSprite;
                }
                else if (t == typeof(AssetReferenceTexture3D)) {
                    newValue = dummy.assetReferenceTexture3D;
                }
                else if (t == typeof(AssetReferenceTexture2D)) {
                    newValue = dummy.assetReferenceTexture3D;
                }
            }
            else {
                ;
                ///Use drawers
                var objectDrawer = PropertyDrawerFactory.GetObjectDrawer(t);
                newValue = objectDrawer.DrawGUI(content, value, info);
            }

            var changed = !object.Equals(newValue, value);
            if (changed) {
                UndoUtility.RecordObjectComplete(info.unityObjectContext, content.text + "Field Change");
            }

            value = newValue;
            if (changed) {
                UndoUtility.SetDirty(info.unityObjectContext);
            }

            return value;
        }

        ///Draws an Editor field for object of type directly WITHOUT taking into acount object drawers and drawer attributes unless provided
        public static object DrawEditorFieldDirect(GUIContent content, object value, Type t, InspectedFieldInfo info,
            EdtDummy dummy)
        {
            ///----------------------------------------------------------------------------------------------
            bool handled;
            var newValue = DirectFieldControl(content, value, t, info.unityObjectContext, info.attributes, out handled);
            var changed = !object.Equals(newValue, value);
            if (changed) {
                UndoUtility.RecordObjectComplete(info.unityObjectContext, content.text + "Field Change");
            }

            value = newValue;
            if (changed) {
                UndoUtility.SetDirty(info.unityObjectContext);
            }

            if (handled) {
                return value;
            }
            ///----------------------------------------------------------------------------------------------

            if (typeof(IList).IsAssignableFrom(t)) {
                return ListEditor(content, (IList) value, t, info);
            }

            if (typeof(IDictionary).IsAssignableFrom(t)) {
                return DictionaryEditor(content, (IDictionary) value, t, info);
            }

            //show nested class members recursively
            if (value != null && (t.IsClass || t.IsValueType)) {
                if (EditorGUI.indentLevel <= 8) {
                    if (!CachedFoldout(t, content)) {
                        return value;
                    }

                    EditorGUI.indentLevel++;
                    ReflectedObjectInspector(value, info.unityObjectContext);
                    EditorGUI.indentLevel--;
                }
            }
            else {
                EditorGUILayout.LabelField(content,
                    GetTempContent(string.Format("NonInspectable ({0})", t.FriendlyName())));
            }

            return value;
        }

        static Dictionary<object[], EdtDummy> cache = new Dictionary<object[], EdtDummy>();

        //...
        public static object DirectFieldControl(GUIContent content, object value, Type t,
            UnityEngine.Object unityObjectContext, object[] attributes, out bool handled,
            params GUILayoutOption[] options)
        {
            handled = true;

  

            //Check scene object type for UnityObjects. Consider Interfaces as scene object type. Assume that user uses interfaces with UnityObjects
            if (typeof(UnityObject).IsAssignableFrom(t) || t.IsInterface) {
                var isSceneObjectType =
                    (typeof(Component).IsAssignableFrom(t) || t == typeof(GameObject) || t.IsInterface);
                if (value == null || value is UnityObject) {
                    //check this to avoid case of interface but no unityobject
                    var newValue =
                        EditorGUILayout.ObjectField(content, (UnityObject) value, t, isSceneObjectType, options);
                    if (unityObjectContext != null && newValue != null) {
                        if (!Application.isPlaying && EditorUtility.IsPersistent(unityObjectContext) &&
                            !EditorUtility.IsPersistent(newValue as UnityEngine.Object)) {
                            ParadoxNotion.Services.Logger.LogWarning("Assets can not have scene object references",
                                "Editor", unityObjectContext);
                            newValue = value as UnityObject;
                        }
                    }

                    return newValue;
                }
            }

            //Check Type second
            if (t == typeof(Type)) {
                return Popup<Type>(content, (Type) value, TypePrefs.GetPreferedTypesList(true), options);
            }

            //get real current type
            t = value != null ? value.GetType() : t;

            //for these just show type information
            if (t.IsAbstract || t == typeof(object) || typeof(Delegate).IsAssignableFrom(t) ||
                typeof(UnityEngine.Events.UnityEventBase).IsAssignableFrom(t)) {
                EditorGUILayout.LabelField(content, new GUIContent(string.Format("({0})", t.FriendlyName())), options);
                return value;
            }

            //create instance for value types
            if (value == null && t.RTIsValueType()) {
                value = System.Activator.CreateInstance(t);
            }

            //create new instance with button for non value types
            if (value == null && !t.IsAbstract && !t.IsInterface &&
                (t.IsArray || t.GetConstructor(Type.EmptyTypes) != null)) {
                if (content != GUIContent.none) {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel(content, GUI.skin.button);
                }

                if (GUILayout.Button("(null) Create", options)) {
                    value = t.IsArray ? Array.CreateInstance(t.GetElementType(), 0) : Activator.CreateInstance(t);
                }

                if (content != GUIContent.none) {
                    GUILayout.EndHorizontal();
                }

                return value;
            }

            ///----------------------------------------------------------------------------------------------
            if (t == typeof(string)) {
                return EditorGUILayout.TextField(content, (string) value, options);
            }

            if (t == typeof(char)) {
                var c = (char) value;
                var s = c.ToString();
                s = EditorGUILayout.TextField(content, s, options);
                return string.IsNullOrEmpty(s) ? (char) c : (char) s[0];
            }

            if (t == typeof(bool)) {
                return EditorGUILayout.Toggle(content, (bool) value, options);
            }

            if (t == typeof(int)) {
                return EditorGUILayout.IntField(content, (int) value, options);
            }

            if (t == typeof(float)) {
                return EditorGUILayout.FloatField(content, (float) value, options);
            }

            if (t == typeof(byte)) {
                return Convert.ToByte(Mathf.Clamp(EditorGUILayout.IntField(content, (byte) value, options), 0, 255));
            }

            if (t == typeof(long)) {
                return EditorGUILayout.LongField(content, (long) value, options);
            }

            if (t == typeof(double)) {
                return EditorGUILayout.DoubleField(content, (double) value, options);
            }

            if (t == typeof(Vector2)) {
                return EditorGUILayout.Vector2Field(content, (Vector2) value, options);
            }

            if (t == typeof(Vector2Int)) {
                return EditorGUILayout.Vector2IntField(content, (Vector2Int) value, options);
            }

            if (t == typeof(Vector3)) {
                return EditorGUILayout.Vector3Field(content, (Vector3) value, options);
            }

            if (t == typeof(Vector3Int)) {
                return EditorGUILayout.Vector3IntField(content, (Vector3Int) value, options);
            }

            if (t == typeof(Vector4)) {
                return EditorGUILayout.Vector4Field(content, (Vector4) value, options);
            }

            if (t == typeof(Quaternion)) {
                var quat = (Quaternion) value;
                var vec4 = new Vector4(quat.x, quat.y, quat.z, quat.w);
                vec4 = EditorGUILayout.Vector4Field(content, vec4, options);
                return new Quaternion(vec4.x, vec4.y, vec4.z, vec4.w);
            }

            if (t == typeof(Color)) {
                var att = attributes?.FirstOrDefault(a => a is ColorUsageAttribute) as ColorUsageAttribute;
                var hdr = att != null ? att.hdr : false;
                var showAlpha = att != null ? att.showAlpha : true;
                return EditorGUILayout.ColorField(content, (Color) value, true, showAlpha, hdr, options);
            }

            if (t == typeof(Gradient)) {
                return EditorGUILayout.GradientField(content, (Gradient) value, options);
            }

            if (t == typeof(Rect)) {
                return EditorGUILayout.RectField(content, (Rect) value, options);
            }

            if (t == typeof(AnimationCurve)) {
                return EditorGUILayout.CurveField(content, (AnimationCurve) value, options);
            }

            if (t == typeof(Bounds)) {
                return EditorGUILayout.BoundsField(content, (Bounds) value, options);
            }

            if (t == typeof(LayerMask)) {
                return LayerMaskField(content, (LayerMask) value, options);
            }

            if (t.IsSubclassOf(typeof(System.Enum))) {
                if (t.RTIsDefined(typeof(FlagsAttribute), true)) {
#if UNITY_2017_3_OR_NEWER
                    return EditorGUILayout.EnumFlagsField(content, (System.Enum) value, options);
#else
					return EditorGUILayout.EnumMaskPopup(content, (System.Enum)value, options);
#endif
                }

                return EditorGUILayout.EnumPopup(content, (System.Enum) value, options);
            }

            handled = false;
            return value;
        }
    }
}

#endif