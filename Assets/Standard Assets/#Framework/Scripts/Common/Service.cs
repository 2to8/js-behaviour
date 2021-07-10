// using Unity.Entities;
//
// namespace Common {
//
// public abstract class Service<T> : SystemBase where T : Service<T> {
//
//     static T m_Instance;
//     static World m_World => World.DefaultGameObjectInjectionWorld;
//
//     public static T instance {
//         get {
//             if (m_World == null) {
//                 DefaultWorldInitialization.DefaultLazyEditModeInitialize();
//             }
//             return m_Instance ??(m_Instance= m_World?.GetOrCreateSystem<T>());
//         }
//         protected set => m_Instance = value;
//     }
//
//     protected override void OnCreate()
//     {
//         base.OnCreate();
//         m_Instance = (T)this;
//     }
//
// }
//
// }

