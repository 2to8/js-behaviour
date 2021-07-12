
namespace PuertsStaticWrap {

    public static partial class PuertsHelper {

        public static (int runtime, int editor) UsingActions(this Puerts.JsEnv jsEnv)
        {
            jsEnv.UsingAction<System.Reflection.Assembly, string, bool>();
            jsEnv.UsingAction<bool>();
            jsEnv.UsingAction<int>();
            jsEnv.UsingAction<string, bool, string>();
            jsEnv.UsingAction<string, string, UnityEngine.LogType>();
            jsEnv.UsingAction<UnityEngine.CullingGroupEvent>();
            jsEnv.UsingAction<UnityEngine.ReflectionProbe, UnityEngine.ReflectionProbe.ReflectionProbeEvent>();
            jsEnv.UsingAction<UnityEngine.CustomRenderTexture, int>();
            jsEnv.UsingAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>();
            jsEnv.UsingAction<UnityEngine.SceneManagement.Scene>();
            jsEnv.UsingAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.Scene>();
            jsEnv.UsingAction<bool, bool, int>();
            jsEnv.UsingAction<int, int>();
            jsEnv.UsingAction<string, UnityEngine.Rect>();
            jsEnv.UsingAction<int, UnityEngine.Rect>();
            jsEnv.UsingAction<System.Enum>();
            jsEnv.UsingAction<UnityEngine.Rect>();
            jsEnv.UsingAction<System.Object, string[], int>();
            jsEnv.UsingAction<UnityEngine.Vector2, UnityEngine.Vector3, UnityEngine.Vector3>();
            jsEnv.UsingAction<float>();
            jsEnv.UsingAction<float, UnityEngine.Color>();
            jsEnv.UsingAction<float, UnityEngine.Color, System.Single, System.Single>();
            jsEnv.UsingAction<System.Net.ServicePoint, System.Net.IPEndPoint, int>();
            jsEnv.UsingAction<int, System.Net.WebHeaderCollection>();
            jsEnv.UsingAction<System.Object, System.Security.Cryptography.X509Certificates.X509Certificate, System.Security.Cryptography.X509Certificates.X509Chain, System.Net.Security.SslPolicyErrors>();
            jsEnv.UsingAction<UnityRoyale.CardData, UnityEngine.Vector3, UnityRoyale.Placeable.Faction>();
            jsEnv.UsingAction<int, UnityEngine.Vector2>();
            jsEnv.UsingAction<NodeCanvas.Framework.Status>();
            jsEnv.UsingAction<string, int, System.Char>();
            jsEnv.UsingAction<FlowCanvas.Flow>();
            jsEnv.UsingAction<UnityEngine.Transform, UnityEngine.Transform, bool, System.Object[]>();
            jsEnv.UsingAction<ParadoxNotion.EventData<UnityEngine.EventSystems.PointerEventData>>();
            jsEnv.UsingAction<ParadoxNotion.EventData<UnityEngine.EventSystems.BaseEventData>>();
            jsEnv.UsingAction<ParadoxNotion.EventData<UnityEngine.EventSystems.AxisEventData>>();
            jsEnv.UsingAction<ParadoxNotion.EventData>();
            jsEnv.UsingAction<ParadoxNotion.EventData<int>>();
            jsEnv.UsingAction<ParadoxNotion.EventData<UnityEngine.ControllerColliderHit>>();
            jsEnv.UsingAction<ParadoxNotion.EventData<UnityEngine.GameObject>>();
            jsEnv.UsingAction<ParadoxNotion.EventData<UnityEngine.Collision>>();
            jsEnv.UsingAction<ParadoxNotion.EventData<UnityEngine.Collision2D>>();
            jsEnv.UsingAction<ParadoxNotion.EventData<UnityEngine.Collider>>();
            jsEnv.UsingAction<ParadoxNotion.EventData<UnityEngine.Collider2D>>();
            jsEnv.UsingAction<ParadoxNotion.Services.Logger.Message>();
            jsEnv.UsingAction<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>();
            jsEnv.UsingAction<int, int, int>();
            jsEnv.UsingAction<System.Collections.Generic.List<System.Collections.Generic.List<int>>, System.Collections.Generic.List<int>, int>();
            jsEnv.UsingAction<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle, System.Exception>();
            jsEnv.UsingAction<UnityEngine.ResourceManagement.ResourceManager.DiagnosticEventContext>();
            jsEnv.UsingAction<int, string>();
            jsEnv.UsingAction<UnityEngine.ResourceManagement.Diagnostics.DiagnosticEvent>();
            jsEnv.UsingAction<System.IntPtr, System.IntPtr>();
            jsEnv.UsingAction<double>();
            jsEnv.UsingAction<uint>();
            jsEnv.UsingAction<long>();
            jsEnv.UsingAction<ulong>();
            jsEnv.UsingAction<UnityEngine.Vector2>();
            jsEnv.UsingAction<UnityEngine.Vector3>();
            jsEnv.UsingAction<UnityEngine.Vector4>();
            jsEnv.UsingAction<UnityEngine.Quaternion>();
            jsEnv.UsingAction<UnityEngine.Color>();
            jsEnv.UsingAction<UnityEngine.LogType, System.Object>();
            jsEnv.UsingAction<float, float, float, float>();
            jsEnv.UsingAction<DG.Tweening.Plugins.Options.PathOptions, DG.Tweening.Tween, UnityEngine.Quaternion, UnityEngine.Transform>();
#if UNITY_EDITOR
            jsEnv.UsingAction<UnityEditor.BuildPlayerOptions>();
            jsEnv.UsingAction<UnityEditor.PauseState>();
            jsEnv.UsingAction<UnityEditor.PlayModeStateChange>();
            jsEnv.UsingAction<UnityEditor.MaterialProperty, int, System.Object>();
            jsEnv.UsingAction<UnityEngine.AnimationClip, UnityEditor.EditorCurveBinding, UnityEditor.AnimationUtility.CurveModifiedType>();
            jsEnv.UsingAction<UnityEditor.SceneView.CameraMode>();
            jsEnv.UsingAction<UnityEditor.CacheServerConnectionChangedParameters>();
            jsEnv.UsingAction<UnityEditor.ModeService.ModeChangedArgs>();
#endif
            return (175, 8);
        }

    }

    }
