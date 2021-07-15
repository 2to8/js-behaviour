namespace PuertsStaticWrap
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using JsEnv = Puerts.JsEnv;
    using BindingFlags = System.Reflection.BindingFlags;

    public static class AutoStaticUsing
    {
        public static void AutoUsing(this JsEnv jsEnv)
        {
            jsEnv.UsingAction<System.Int32>();
            jsEnv.UsingAction<System.Boolean>();
            jsEnv.UsingAction<System.Single>();
            jsEnv.UsingAction<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle, System.Exception>();
            jsEnv.UsingAction<UnityEngine.ResourceManagement.Diagnostics.DiagnosticEvent>();
            jsEnv.UsingAction<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>();
            jsEnv.UsingAction<FlowCanvas.Flow>();
            jsEnv.UsingAction<System.Double>();
            jsEnv.UsingAction<System.UInt32>();
            jsEnv.UsingAction<System.Int64>();
            jsEnv.UsingAction<System.UInt64>();
            jsEnv.UsingAction<UnityEngine.Vector2>();
            jsEnv.UsingAction<UnityEngine.Vector3>();
            jsEnv.UsingAction<UnityEngine.Vector4>();
            jsEnv.UsingAction<UnityEngine.Quaternion>();
            jsEnv.UsingAction<UnityEngine.Color>();
            jsEnv.UsingAction<UnityEngine.Rect>();
            jsEnv.UsingAction<DG.Tweening.Color2>();
            jsEnv.UsingAction<DG.Tweening.Plugins.Options.PathOptions, DG.Tweening.Tween, UnityEngine.Quaternion, UnityEngine.Transform>();
            jsEnv.UsingAction<System.String, System.Int32, System.Int32>();
            jsEnv.UsingAction<NodeCanvas.Framework.Status>();
            jsEnv.UsingAction<System.Single, System.Single, System.Single>();
            jsEnv.UsingAction<System.Int32, UnityEngine.UI.Extensions.MovementDirection>();
            jsEnv.UsingAction<System.Int32, System.Int32, System.Int32>();
            jsEnv.UsingAction<System.Single, System.Single>();
            jsEnv.UsingAction<System.String, System.Boolean>();
            jsEnv.UsingAction<System.Int32, UnityEngine.Vector2>();
            jsEnv.UsingAction<UnityEngine.Rect, System.String>();
            jsEnv.UsingAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>();
            jsEnv.UsingAction<UnityEngine.SceneManagement.Scene>();
            jsEnv.UsingFunc<System.Int32, System.Boolean>();
            jsEnv.UsingFunc<System.Int32, System.Int32, System.Int32>();
            jsEnv.UsingFunc<System.Reflection.MemberInfo, System.Object, System.Boolean>();
            jsEnv.UsingFunc<System.Reflection.Assembly, System.String, System.Boolean, System.Type>();
            jsEnv.UsingFunc<System.Type, System.Object, System.Boolean>();
            jsEnv.UsingFunc<Unity.Entities.ComponentSystemGroup, System.Boolean>();
            jsEnv.UsingFunc<System.Single, System.Single>();
            jsEnv.UsingFunc<UnityEngine.Transform, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.UI.ILayoutElement, System.Single>();
            jsEnv.UsingFunc<System.Object, System.Boolean>();
            jsEnv.UsingFunc<System.Single, System.Single, System.Single, System.Single, System.Single>();
            jsEnv.UsingFunc<System.Int32>();
            jsEnv.UsingFunc<System.Single>();
            jsEnv.UsingFunc<System.ValueTuple<System.Single, System.Single>>();
            jsEnv.UsingFunc<System.Single, System.Single, System.Single, System.Single>();
            jsEnv.UsingFunc<System.Double>();
            jsEnv.UsingFunc<System.UInt32>();
            jsEnv.UsingFunc<System.Int64>();
            jsEnv.UsingFunc<System.UInt64>();
            jsEnv.UsingFunc<UnityEngine.Vector2>();
            jsEnv.UsingFunc<UnityEngine.Vector3>();
            jsEnv.UsingFunc<UnityEngine.Vector4>();
            jsEnv.UsingFunc<UnityEngine.Quaternion>();
            jsEnv.UsingFunc<UnityEngine.Color>();
            jsEnv.UsingFunc<UnityEngine.Rect>();
            jsEnv.UsingFunc<DG.Tweening.Color2>();
            jsEnv.UsingFunc<System.Int32, System.String, TMPro.TMP_FontAsset>();
            jsEnv.UsingFunc<System.Int32, System.String, TMPro.TMP_SpriteAsset>();
            jsEnv.UsingFunc<System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.ResourceManagement.AsyncOperations.DownloadStatus>();
            jsEnv.UsingFunc<System.Type, System.Boolean>();
            jsEnv.UsingFunc<System.Reflection.FieldInfo, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.GameObject, System.Boolean>();
            jsEnv.UsingFunc<System.Boolean, System.Boolean>();
            jsEnv.UsingFunc<System.UInt64, System.Boolean>();
            jsEnv.UsingFunc<System.Reflection.MethodInfo, System.Boolean>();
            jsEnv.UsingFunc<System.Reflection.ConstructorInfo, System.Boolean>();
            jsEnv.UsingFunc<System.Reflection.MemberInfo, System.Boolean>();
            jsEnv.UsingFunc<Newtonsoft.Json.Serialization.JsonProperty, System.Int32>();
            jsEnv.UsingFunc<Newtonsoft.Json.Schema.JsonSchemaType, System.Boolean>();
            jsEnv.UsingFunc<System.String, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.LogType, System.Object, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.UI.Graphic, UnityEngine.UI.Graphic, System.Int32>();
            jsEnv.UsingFunc<UnityEngine.Component, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.UI.Toggle, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.AnimationClip, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.Timeline.TimelineClip, UnityEngine.Timeline.TimelineClip, System.Int32>();
            jsEnv.UsingFunc<System.Char, System.Boolean>();
            jsEnv.UsingFunc<TMPro.TMP_Character, System.UInt32>();
            jsEnv.UsingFunc<UnityEngine.TextCore.Glyph, System.UInt32>();
            jsEnv.UsingFunc<TMPro.KerningPair, System.UInt32>();
            jsEnv.UsingFunc<TMPro.TMP_GlyphPairAdjustmentRecord, System.UInt32>();
            jsEnv.UsingFunc<TMPro.TMP_SpriteGlyph, System.UInt32>();
            jsEnv.UsingFunc<TMPro.TMP_SpriteCharacter, System.UInt32>();
            jsEnv.UsingFunc<UnityEngine.AI.NavMeshModifierVolume, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.AI.NavMeshModifier, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.AI.NavMeshBuildSource, System.Boolean>();
            jsEnv.UsingFunc<System.Reflection.PropertyInfo, System.Boolean>();
            jsEnv.UsingFunc<SqlCipher4Unity3D.SQLiteConnectionWithLock, System.Int32>();
            jsEnv.UsingFunc<System.Byte, System.String>();
            jsEnv.UsingFunc<UnityEngine.Object, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.ResourceManagement.Diagnostics.DiagnosticEvent, System.Int32>();
            jsEnv.UsingFunc<System.String, UnityEngine.Color>();
            jsEnv.UsingFunc<System.Reflection.Assembly, System.Boolean>();
            jsEnv.UsingFunc<System.Diagnostics.StackFrame, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.SceneManagement.Scene, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.SceneManagement.Scene, System.Collections.Generic.IEnumerable<UnityEngine.GameObject>>();
            jsEnv.UsingFunc<Utils.RndReferenceItem, System.Int32>();
            jsEnv.UsingFunc<MoreTags.Tags, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.UI.ExtensionsToggle, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.UI.Image, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.UIVertex, System.Single>();
            jsEnv.UsingFunc<System.Int32, UnityEngine.UI.Extensions.Examples.FancyScrollViewExample08.ItemData>();
            jsEnv.UsingFunc<System.Int32, UnityEngine.UI.Extensions.Examples.FancyScrollViewExample07.ItemData>();
            jsEnv.UsingFunc<System.Int32, UnityEngine.UI.Extensions.Examples.FancyScrollViewExample06.ItemData>();
            jsEnv.UsingFunc<UnityEngine.Vector4, System.Int32, System.ValueTuple<System.Int32, System.Int32, UnityEngine.Vector2>>();
            jsEnv.UsingFunc<System.Int32, UnityEngine.UI.Extensions.Examples.FancyScrollViewExample05.ItemData>();
            jsEnv.UsingFunc<System.Int32, UnityEngine.UI.Extensions.Examples.FancyScrollViewExample04.ItemData>();
            jsEnv.UsingFunc<System.Int32, UnityEngine.UI.Extensions.Examples.FancyScrollViewExample03.ItemData>();
            jsEnv.UsingFunc<System.Int32, UnityEngine.UI.Extensions.Examples.FancyScrollViewExample02.ItemData>();
            jsEnv.UsingFunc<System.Int32, UnityEngine.UI.Extensions.Examples.FancyScrollViewExample01.ItemData>();
            jsEnv.UsingFunc<qtools.qhierarchy.QObjectList, System.Boolean>();
            jsEnv.UsingFunc<PowerInject.FinalValue, System.Boolean>();
            jsEnv.UsingFunc<PowerInject.Producer, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.MonoBehaviour, System.Boolean>();
            jsEnv.UsingFunc<System.Reflection.ConstructorInfo, System.Int32>();
            jsEnv.UsingFunc<NodeCanvas.Framework.Node, UnityEngine.Rect>();
            jsEnv.UsingFunc<System.Reflection.ParameterInfo, System.Boolean>();
            jsEnv.UsingFunc<ParadoxNotion.DynamicParameterDefinition, System.Boolean>();
            jsEnv.UsingFunc<FlowCanvas.Port, System.Int32>();
            jsEnv.UsingFunc<FlowCanvas.ValueInput, System.Boolean>();
            jsEnv.UsingFunc<FlowCanvas.Port, System.Boolean>();
            jsEnv.UsingFunc<System.String, System.Int32>();
            jsEnv.UsingFunc<UnityEngine.RaycastHit2D, UnityEngine.Collider2D>();
            jsEnv.UsingFunc<UnityEngine.RaycastHit2D, UnityEngine.GameObject>();
            jsEnv.UsingFunc<UnityEngine.RaycastHit2D, System.Single>();
            jsEnv.UsingFunc<UnityEngine.RaycastHit2D, UnityEngine.Vector3>();
            jsEnv.UsingFunc<NodeCanvas.Framework.IUpdatable, System.Boolean>();
            jsEnv.UsingFunc<NodeCanvas.Framework.Node, System.Boolean>();
            jsEnv.UsingFunc<NodeCanvas.Framework.Node, System.Int32>();
            jsEnv.UsingFunc<NodeCanvas.Framework.BBParameter, System.Boolean>();
            jsEnv.UsingFunc<NodeCanvas.Framework.Connection, System.Single>();
            jsEnv.UsingFunc<NodeCanvas.Framework.Variable, System.Boolean>();
            jsEnv.UsingFunc<ParadoxNotion.Serialization.FullSerializer.fsDataType, System.String>();
            jsEnv.UsingFunc<System.Text.StringBuilder, System.Byte, System.Text.StringBuilder>();
            jsEnv.UsingFunc<UnityEngine.Transform, System.Int32>();
            jsEnv.UsingFunc<System.Int64, System.Boolean>();
            jsEnv.UsingFunc<UnityEngine.TextAsset, System.Boolean>();
            jsEnv.UsingFunc<MoreTags.TagQueryItem, System.ValueTuple<System.String[], System.Type[]>>();
            jsEnv.UsingFunc<NodeCanvas.Framework.Graph, System.Boolean>();
            jsEnv.UsingFunc<MoreTags.TagData, System.Boolean>();
            jsEnv.UsingFunc<System.Int32, System.String>();
            jsEnv.UsingFunc<MoreTags.TagData, System.Int32>();
            
        }
        public static void UsingAction(this JsEnv jsEnv, params string[] args)
        {
            jsEnv.UsingGenericA(true, FindTypes(args));
        }
        public static void UsingFunc(this JsEnv jsEnv, params string[] args)
        {
            jsEnv.UsingGenericA(false, FindTypes(args));
        }
        public static void UsingGenericA(this JsEnv jsEnv, bool usingAction, params Type[] types)
        {
            var name = usingAction ? "UsingAction" : "UsingFunc";
            var count = types.Length;
            var method = (from m in typeof(JsEnv).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                          where m.Name.Equals(name)
                             && m.IsGenericMethod
                             && m.GetGenericArguments().Length == count
                          select m).FirstOrDefault();
            if (method == null)
                throw new Exception("Not found type: '" + name + "', ArgsLength=" + count);
            method.MakeGenericMethod(types).Invoke(jsEnv, null);
        }
        static Type[] FindTypes(string[] args)
        {
            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
            var types = new List<Type>();
            foreach (var arg in args)
            {
                Type type = null;
                for (var i = 0; i < assemblys.Length && type == null; i++)
                    type = assemblys[i].GetType(arg, false);
                if (type == null)
                    throw new Exception("Not found type: '" + arg + "'");
                types.Add(type);
            }
            return types.ToArray();
        }
    }
}