// using UnityEngine;
//
// namespace Common.JSRuntime {
//
// public class JsService : Service<JsService> {
//
// #region MyRegion
//     // [InitializeOnLoadMethod, DidReloadScripts, InitializeOnEnterPlayMode]
//     // static void Initial()
//     // {
//     //     //ReloadSystems(PlayModeStateChange.EnteredEditMode);
//     //     EditorApplication.playModeStateChanged += ReloadSystems;
//     //
//     //     //EditorApplication.delayCall += () => ReloadSystems();
//     // }
//     //
//     // public static void ReloadSystems(PlayModeStateChange modeStateChange)
//     // {
//     //     if (modeStateChange == PlayModeStateChange.EnteredEditMode) {
//     //         if (World.DefaultGameObjectInjectionWorld == null) {
//     //             DefaultWorldInitialization.DefaultLazyEditModeInitialize();
//     //         }
//     //         if (World.DefaultGameObjectInjectionWorld != null &&
//     //             World.DefaultGameObjectInjectionWorld.GetExistingSystem<JsEnvSystem>() == null) {
//     //             FindAndCreateWorldFromNamespace(World.DefaultGameObjectInjectionWorld);
//     //         }
//     //     }
//     // }
//     //
//     // public static void FindAndCreateWorldFromNamespace(World world, string name = null)
//     // {
//     //     //World.DefaultGameObjectInjectionWorld = world;
//     //     foreach (var ass in AppDomain.CurrentDomain.GetAssemblies()) {
//     //         if (ass.ManifestModule.ToString() == "Microsoft.CodeAnalysis.Scripting.dll") continue;
//     //
//     //         var allTypes = ass.GetTypes();
//     //         var systemTypes = allTypes.Where(t =>
//     //             t.IsSubclassOf(typeof(ComponentSystemBase)) &&
//     //             !t.IsAbstract &&
//     //             !t.ContainsGenericParameters &&
//     //             (name == null || t.Namespace != null && (t.Namespace == name || t.Namespace.StartsWith(name + "."))) &&
//     //             t.FullName?.StartsWith("Unity") == false &&
//     //             !t.Assembly.GetName().Name.StartsWith("Unity") &&
//     //             t.GetCustomAttributes(typeof(DisableAutoCreationAttribute), true).Length == 0);
//     //         foreach (var type in systemTypes) {
//     //             GetBehaviourManagerAndLogException(world, type);
//     //         }
//     //     }
//     // }
//     //
//     // public static void GetBehaviourManagerAndLogException(World world, Type type)
//     // {
//     //     try {
//     //         Debug.Log("Found: " + type.FullName);
//     //         world.GetOrCreateSystem(type);
//     //     } catch (Exception e) {
//     //         Debug.LogException(e);
//     //     }
//     // }
// #endregion
//
//     protected override void OnCreate()
//     {
//         base.OnCreate();
//         Debug.Log("jsenv system created");
//     }
//
//     protected override void OnUpdate()
//     {
//         //Debug.Log("jsenv system tick");
//     }
//
// }
//
// }

