#if UNITY_EDITOR
using Puerts;
using Puerts.Attributes;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Common.Config {

[Configure, PuertsIgnore]
public partial class PuertsFilters {

    // static string [] AssemblyList = { };
    //
    // static bool AssemblyFilter( Assembly assembly )
    // {
    //     return true;
    //
    //     if ( AssemblyList.Contains( assembly.GetName().Name ) ) return true;
    //
    //     return false;
    // }
    //
    // static List<string> namespaces = new List<string>() // 在这里添加名字空间
    //     { };

    [AssemblyFilter]
    static List<string> customAssemblys => new List<string> {
            //"UniRx",
            //"UniRx.Async",
            "MoreTags",
            "Unity.Entities",
            "Unity.Enities.Hybrid",
            "Newtonsoft.Json",
            "DOTween",
            "UnityEngine",
            "UnityEngine.JSONSerializeModule",
            "UnityEngine.UI",
            "DOTweenPro",
            "Unity.Timeline",
            "Unity.TextMeshPro",
            "NavMeshComponents",
            "SqlCipher4Unity3D",
            "Unity.Addressables",
            "Unity.CoreModule",
            "Unity.ResourceManager",
            "Assembly-CSharp",
            "Assembly-CSharp-firstpass",
            "ParadoxNotion",
        }.Distinct()
        .ToList();

    [SubTypeFilter]
    static List<Type> SubTypesExclude => new List<Type> {
        typeof(UnaryExpression),

        // typeof(Controller),
        // typeof(ModelBase),
        // typeof(ControllerBase),
        // typeof(IStateBase),
        // typeof(ViewBase),
        // typeof(IData),
    };

    [IncludeFilter]
    static List<string> IncluceList => new List<string> {
        // "UnityEngine.UI",
        // "Sirenix.Utilities",
        // "FairyGUI",
        // "FairyGUI.Utils",
        //"System.Linq",
        "Unity",
        "Engine",
        "UnityEngine",
        "UnityEngine.UI",
    };

    /***************如果你全lua编程，可以参考这份自动化配置***************/
    //--------------begin 纯lua编程配置参考----------------------------
    [ExcludeFilter]
    static List<string> m_ExcludeNames => new List<string> {
            "GetEntityQuery_ForOnUpdate_LambdaJob",
            "Importer",
            "GameEngine.GameKit.MVC",
            "System.Net",
            "Unity.Entities.InternalCompilerInterface",
            "Unity.Entities.CodeGeneratedJobForEach",
            "Reset",
            "AssemblyNotStrip",
            "NodeCanvas.Tasks.Actions.GetToString",
            "Puerts.",
            "Newtonsoft.Json.Serialization.ExpressionValueProvider",
            "Newtonsoft.Json.Linq",
            "UniRx.ScenePlaybackDetector",
            "UnityEngine.AssetGraph",
            "VersionControl",
            "UnityEditor",
            "Sirenix.OdinInspector.Demos",
            typeof(AssemblyTypeFlags).FullName,
            typeof(AssemblyUtilities).FullName,

            // typeof( CanvasGroupActivator ).FullName,
            "AssetDatabase",
            "UnityEngine.Caching",
            "UnityEngine.GUI",

            //typeof( TextPicIconListCopier ).FullName,
            //typeof( uGUITools ).FullName,
            $"{typeof(ShineEffect).FullName}.{nameof(ShineEffect.OnRebuildRequested)}",
            typeof(MemberFinderExtensions).FullName,

            //typeof( InputRegistering ).FullName,
            //typeof( ReadOnlyDrawer ).FullName,
            "System.Linq.Expressions.UnaryExpression",
            "UnityEngine.InputManagerEntry",
            "UnityEngine.ResourceManagement.ResourceProviders.",
            "UnityEngine.AddressableAssets.ResourceLocators.ContentCatalogData",
            "UniRx.Async.Internal.TaskTracker",

            //typeof(UnityEngine.LightingSettings).FullName,
            "Editor",
            "Editors",
            "FlyingWormConsole3.LiteNetLib.",
            "Simulation",
            "UnityEngine.UI.DefaultControls.",
            "PuertsStaticWrap",
            "EnvMapAnimator",
            "TMPro.Examples.",
            "HideInInspector",
            "ExecuteInEditMode",
            "AddComponentMenu",
            "ContextMenu",
            "RequireComponent",
            "DisallowMultipleComponent",
            "SerializeField",
            "AssemblyIsEditorAssembly",
            "Attribute",

            //"Types",
            "UnitySurrogateSelector",
            "TrackedReference",
            "TypeInferenceRules",
            "FFTWindow",
            "RPC",
            "Network",
            "MasterServer",
            "BitStream",
            "HostData",
            "ConnectionTesterStatus",

            //"GUI",
            "EventType",
            "EventModifiers",
            "FontStyle",
            "TextAlignment",
            "TextEditor",
            "TextEditorDblClickSnapping",
            "TextGenerator",
            "TextClipping",
            "Gizmos",
            "ADBannerView",
            "ADInterstitialAd",
            "Android",
            "Tizen",
            "jvalue",
            "iPhone",
            "iOS",
            "Windows",
            "CalendarIdentifier",
            "CalendarUnit",
            "CalendarUnit",
            "ClusterInput",
            "FullScreenMovieControlMode",
            "FullScreenMovieScalingMode",
            "Handheld",
            "LocalNotification",
            "NotificationServices",
            "RemoteNotificationType",
            "RemoteNotification",
            "SamsungTV",
            "TextureCompressionQuality",
            "TouchScreenKeyboardType",
            "TouchScreenKeyboard",
            "MovieTexture",
            "UnityEngineInternal",
            "Terrain",
            "Tree",
            "SplatPrototype",
            "DetailPrototype",
            "DetailRenderMode",
            "MeshSubsetCombineUtility",
            "AOT",
            "Social",
            "Enumerator",
            "SendMouseEvents",
            "Cursor",
            "Flash",
            "ActionScript",
            "OnRequestRebuild",
            "Ping",
            "ShaderVariantCollection",
            "SimpleJson.Reflection",
            "CoroutineTween",
            "GraphicRebuildTracker",
            "Advertisements",
            "UnityEditor",
            "WSA",
            "UnityEngine.XR.",
            "UnityEngine.UIElements.",
            "UnityEngine.VFX.",
            "UnityEngine.tvOS.",
            "PuertsStaticWrap",
            "EventProvider",
            "Apple",
            "ClusterInput",
            "Motion",
            "UnityEngine.Rendering.",
            "UnityEngine.UI.ReflectionMethodsCache",
            "NativeLeakDetection",
            "NativeLeakDetectionMode",
            "WWWAudioExtensions",
            "UnityEngine.TestTools.",
            "UnityEngine.Experimental",
        }.Distinct()
        .ToList();

    //黑名单
    [PuertsBlacklist]
    public static List<List<string>> BlackList => new List<List<string>>() {
        new List<string>() { "System.Xml.XmlNodeList", "ItemOf" },
        new List<string>() { "UnityEngine.WWW", "movie" },
    #if UNITY_WEBGL
                new List<string>(){"UnityEngine.WWW", "threadPriority"},
    #endif
        new List<string>() { "UnityEngine.Texture2D", "alphaIsTransparency" },
        new List<string>() { "UnityEngine.Security", "GetChainOfTrustValue" },
        new List<string>() { "UnityEngine.CanvasRenderer", "onRequestRebuild" },
        new List<string>() { "UnityEngine.Light", "areaSize" },
        new List<string>() { "UnityEngine.Light", "lightmapBakeType" },
        new List<string>() { "UnityEngine.WWW", "MovieTexture" },
        new List<string>() { "UnityEngine.WWW", "GetMovieTexture" },
        new List<string>() { "UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup" },
    #if !UNITY_WEBPLAYER
        new List<string>() { "UnityEngine.Application", "ExternalEval" },
    #endif
        new List<string>() { "UnityEngine.GameObject", "networkView" }, //4.6.2 not support
        new List<string>() { "UnityEngine.Component", "networkView" },  //4.6.2 not support
        new List<string>() {
            "System.IO.FileInfo", "GetAccessControl",
            "System.Security.AccessControl.AccessControlSections",
        },
        new List<string>() {
            "System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity",
        },
        new List<string>() {
            "System.IO.DirectoryInfo",
            "GetAccessControl",
            "System.Security.AccessControl.AccessControlSections",
        },
        new List<string>() {
            "System.IO.DirectoryInfo", "SetAccessControl",
            "System.Security.AccessControl.DirectorySecurity",
        },
        new List<string>() {
            "System.IO.DirectoryInfo",
            "CreateSubdirectory",
            "System.String",
            "System.Security.AccessControl.DirectorySecurity",
        },
        new List<string>() { typeof(Text).FullName, nameof(Text.OnRebuildRequested) },
        new List<string>() {
            typeof(DrivenRectTransformTracker).FullName,
            nameof(DrivenRectTransformTracker.StopRecordingUndo),
        },
        new List<string>() {
            typeof(DrivenRectTransformTracker).FullName,
            nameof(DrivenRectTransformTracker.StartRecordingUndo),
        },
        new List<string>() {
            typeof(ParticleSystemRenderer).FullName,
            nameof(ParticleSystemRenderer.supportsMeshInstancing),
        },
        new List<string>() { typeof(Input).FullName, nameof(Input.IsJoystickPreconfigured) },
        new List<string>() {
            typeof(ParticleSystemForceField).FullName, nameof(ParticleSystemForceField.FindAll),
        },
        new List<string>() { typeof(Graphic).FullName, nameof(Graphic.OnRebuildRequested) },
        new List<string>() { typeof(Texture).FullName, nameof(Texture.imageContentsHash) },
        new List<string>() {
            typeof(QualitySettings).FullName, nameof(QualitySettings.streamingMipmapsRenderersPerFrame),
        },
        new List<string>() {
            "System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity",
        },
        new List<string>() { "UnityEngine.MonoBehaviour", "runInEditMode" },

        // add
        new List<string>() { typeof(Light).FullName, nameof(Light.SetLightDirty) },
        new List<string>() { typeof(Light).FullName, nameof(Light.shadowAngle) },
        new List<string>() { typeof(Light).FullName, nameof(Light.shadowRadius) },
        new List<string>() { typeof(ContentCatalogData).FullName, nameof(ContentCatalogData.SetData) },
        new List<string>() {
            typeof(AnimatorControllerParameter).FullName,
            "set_" + nameof(AnimatorControllerParameter.name),
        },
        new List<string>() { typeof(Tilemap).FullName, nameof(Tilemap.HasEditorPreviewTile) },
        new List<string>() { typeof(Tilemap).FullName, nameof(Tilemap.GetEditorPreviewColor) },
        new List<string>() { typeof(MeshRenderer).FullName, nameof(MeshRenderer.stitchLightmapSeams) },
        new List<string>() {
            typeof(AssetReferenceSprite).FullName, nameof(AssetReferenceSprite.editorAsset),
        },
        new List<string>() {
            typeof(AssetReferenceSprite).FullName, nameof(AssetReferenceSprite.SetEditorAsset),
        },
        new List<string>() {
            typeof(AssetReferenceSprite).FullName, nameof(AssetReferenceSprite.SetEditorSubObject),
        },
        new List<string>() { typeof(LightProbeGroup).FullName, nameof(LightProbeGroup.dering) },
        new List<string>() {
            typeof(AssetReferenceAtlasedSprite).FullName, nameof(AssetReferenceAtlasedSprite.editorAsset),
        },
        new List<string>() { typeof(AssetReference).FullName, nameof(AssetReference.editorAsset) },
        new List<string>() { typeof(AssetReference).FullName, nameof(AssetReference.SetEditorAsset) },
        new List<string>() { typeof(AssetReference).FullName, nameof(AssetReference.SetEditorSubObject) },
        new List<string>() {
            typeof(AssetReferenceAtlasedSprite).FullName,
            nameof(AssetReferenceAtlasedSprite.SetEditorAsset),
        },
        new List<string>() {
            typeof(AssetReferenceAtlasedSprite).FullName,
            nameof(AssetReferenceAtlasedSprite.SetEditorSubObject),
        },
        new List<string>() {
            typeof(ObjectInitializationData).FullName,
            nameof(ObjectInitializationData.CreateSerializedInitializationData),
        },
        new List<string>() { typeof(MeshRenderer).FullName, nameof(MeshRenderer.scaleInLightmap) },
        new List<string>() {
            typeof(LightProbeGroup).FullName, "set_" + nameof(LightProbeGroup.probePositions),
        },
        new List<string>() {
            typeof(AnimatorControllerParameter).FullName,
            "set_" + nameof(AnimatorControllerParameter.name),
        },
        new List<string>() {
            typeof(AudioSettings).FullName, nameof(AudioSettings.GetSpatializerPluginNames),
        },
        new List<string>() { typeof(MeshRenderer).FullName, nameof(MeshRenderer.receiveGI) },
        new List<string>() {
            typeof(ObjectInitializationData).FullName, nameof(ObjectInitializationData.GetRuntimeTypes),
        },
        new List<string>() {
            typeof(AnimationPlayableAsset).FullName, nameof(AnimationPlayableAsset.LiveLink),
        },
        new List<string>() { typeof(Tilemap).FullName, nameof(Tilemap.tilemapTileChanged) },
        new List<string>() { typeof(Tilemap).FullName, nameof(Tilemap.SyncTile) },
        new List<string>() { typeof(AudioSettings).FullName, nameof(AudioSettings.SetSpatializerPluginName) },

        //end
    };

}

}

#endif