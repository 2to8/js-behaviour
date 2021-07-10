using GameEngine.Kernel;
using GameEngine.Models.Contracts;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine.SceneManagement;

namespace GameEngine.Providers {

public abstract class Controller<M, T> : Controller where T : Controller<M, T> where M : Manager<M, T> {

    static readonly List<Action> m_RunOnceActions = new List<Action>();
    M m_Mb;

    public M mb { get => m_Mb ?? (m_Mb = Manager<M, T>.Instance); set => m_Mb = value; }

    public static void RunOnce(Action action)
    {
        if (action != null && !m_RunOnceActions.Contains(action)) {
            action?.Invoke();
            m_RunOnceActions.Add(action);
        }
    }

    public static void RunOnce(Action action, ref bool onceVar)
    {
        if (!onceVar && action != null) {
            action?.Invoke();
            onceVar = true;
        }
    }

    public void SendEvent<E>() where E : struct, IEvent
    {
        // var type = typeof(E);
        // var value = FormatterServices.GetUninitializedObject(type) ??
        //     Activator.CreateInstance(type);

        // 发送消息
        SendEvent(typeof(E));
    }

    public void SendEvent(Type type, object value = null)
    {
        //var type = typeof(E);

        if (Events.TryGetValue(type, out var commands)) {
            commands.ForEach(func => {
                // var value = FormatterServices.GetUninitializedObject(type) ??
                //     Activator.CreateInstance(type);
                var sys = Core.GetInstance(func.DeclaringType) as Controller;
                var p2 = func.GetParameters().Count() > 1 ? func.GetParameters()[1].ParameterType : null;

                // 第一个参数是事件类, 第二个参数(可选)是状态机当前状态类名, 事件可以仅当前状态接收
                if (p2 == null || p2 == sys?.CurruentState) {
                    // var param = new object[] { value };
                    //
                    // if( p2 != null ) {
                    //     param.AddRange(new object[] {
                    //         FormatterServices.GetUninitializedObject(p2) ??
                    //         Activator.CreateInstance(p2)
                    //     });
                    // }
                    var ps = new object[func.GetParameters().Count()];

                    func.GetParameters()
                        .ForEach((t, i) => {
                            ps[i] = i == 0 && value != null
                                ? value
                                : FormatterServices.GetUninitializedObject(t.ParameterType) ??
                                Activator.CreateInstance(t.ParameterType);
                        });

                    func.Invoke(sys, ps);
                }
            });
        }

        // var value = FormatterServices.GetUninitializedObject(type) ??
        //     Activator.CreateInstance(type);
        //
        // // 发送消息
        //
        //     SendEvent(value);
    }

    public void SendEvent<E>(object value) where E : struct, IEvent
    {
        SendEvent(typeof(E), value);
    }

    public void RunMethod<T1>(string name)
    {
        RunMethod(name, typeof(T1));
    }

    public override void RunMethod<T1>()
    {
        var type = typeof(T1);

        m_Methods.Where(t =>

                // t.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                t.GetParameters().Any() && t.GetParameters()[0].ParameterType == type)
            .ForEach(mi => {
                // var data = FormatterServices.GetUninitializedObject(type) ??
                //     Activator.CreateInstance(type);
                var ps = new object[mi.GetParameters().Count()];

                mi.GetParameters()
                    .ForEach((t, i) => {
                        ps[i] = FormatterServices.GetUninitializedObject(t.ParameterType) ??
                            Activator.CreateInstance(t.ParameterType);
                    });

                mi.Invoke(this, ps);
            });
    }

    public override void RunMethod<T1>(Type state)
    {
        var type = typeof(T1);

        m_Methods.Where(t =>

                // t.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                t.GetParameters().Count() > 1 &&
                t.GetParameters()[0].ParameterType == type &&
                t.GetParameters()[1].ParameterType == state)
            .ForEach(mi => {
                // var data = FormatterServices.GetUninitializedObject(type) ??
                //     Activator.CreateInstance(type);
                // var e = FormatterServices.GetUninitializedObject(state) ??
                //     Activator.CreateInstance(state);
                var ps = new object[mi.GetParameters().Count()];

                mi.GetParameters()
                    .ForEach((t, i) => {
                        ps[i] = FormatterServices.GetUninitializedObject(t.ParameterType) ??
                            Activator.CreateInstance(t.ParameterType);
                    });

                mi.Invoke(this, ps);
            });
    }

    /// <summary>
    ///     执行状态机的方法
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    public void RunMethod(string name, Type type = null)
    {
        m_Methods.Where(t =>
                t.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                t.GetParameters().Any() &&
                (type == null || t.GetParameters()[0].ParameterType == type))
            .ForEach(mi => {
                var target = type ?? mi.GetParameters()[0].ParameterType;

                var data = FormatterServices.GetUninitializedObject(target) ?? Activator.CreateInstance(target);

                var ps = new object[mi.GetParameters().Count()];

                mi.GetParameters()
                    .ForEach((t, i) => {
                        ps[i] = FormatterServices.GetUninitializedObject(t.ParameterType) ??
                            Activator.CreateInstance(t.ParameterType);
                    });

                //ps[ 0 ] = data;
                mi.Invoke(this, ps);
            });
    }

#region Overrides of ComponentSystemBase

    List<MethodInfo> m_Methods;
    static Dictionary<Type, List<MethodInfo>> Events = new Dictionary<Type, List<MethodInfo>>();
    Type DefaultState = typeof(Idle);
    public override Type CurruentState { get; set; }

#if ECS
    protected override void OnCreate()
    {
        base.OnCreate();
        Manager<M, T>.m_System = (T)this;

        m_Methods = GetType()
            .GetMethods(BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.DeclaredOnly)
            .Where(t => t.GetParameters().Any() &&
                    typeof(IComponentData).IsAssignableFrom(t.GetParameters()[0].ParameterType)
                // new[] { nameof(OnUpdate), nameof(OnActivate), nameof(OnDeactivate) }.Contains(
                //     t.Name)
            )
            .ToList();

        RunMethod<OnCreate>();

        if(GetType().IsDefined(typeof(OnAttribute))) {
            DefaultState = GetType().GetCustomAttribute<OnAttribute>().type;
        }

        GetType()
            .GetMethods(BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.DeclaredOnly)
            .Where(t =>
                t.GetParameters().Any() &&
                typeof(IEvent).IsAssignableFrom(t.GetParameters()[0].ParameterType))
            .ForEach(t => {
                var type = t.GetParameters()[0].ParameterType;

                if(!Events.TryGetValue(type, out var methodInfos)) {
                    Events[type] = new List<MethodInfo>();
                }

                if(!Events[type].Contains(t)) {
                    Events[type].Add(t);
                }
            });
    }
#endif

#endregion

#region Overrides of SystemBase

    protected virtual void OnUpdate()
    {
        //if( Main.Booted == false ) return;

        if (CurruentState == null) {
            CurruentState = DefaultState;
            RunMethod<OnStart>(CurruentState);
            RunMethod<OnActivate>(CurruentState);
        }

        RunMethod<OnUpdate>(CurruentState);
    }

    public void SetState<T1>() // where T1 : IComponentData
    {
        SetState(typeof(T1));
    }

    public void SetState(Type state)
    {
        if (state == null) {
            return;
        }

        if (CurruentState != null) {
            RunMethod<OnDeactivate>(CurruentState);
        }

        CurruentState = state;
        RunMethod<OnActivate>(CurruentState);
    }

    protected virtual void OnActivate() { }

    protected virtual void OnDeactivate() { }

    protected virtual void OnStart() { }

#endregion

}

// public class MyGameObjectConversionSystem : GameObjectConversionSystem {
//
//     protected override void OnUpdate() { }
//
// }

static class TypeExtensions {

    public static bool IsDerivedFromOpenGenericType(this Type type, Type openGenericType)
    {
        Contract.Requires(type != null);
        Contract.Requires(openGenericType != null);
        Contract.Requires(openGenericType.IsGenericTypeDefinition);

        return type.GetTypeHierarchy()
            .Where(t => t.IsGenericType)
            .Select(t => t.GetGenericTypeDefinition())
            .Any(t => openGenericType.Equals(t));
    }

#if ECS
    public static ComponentType[] GetComponentType(this IEnumerable<Type> types)
    {
        var componentTypes = new List<ComponentType>();
        types?.ForEach(t => componentTypes.Add(t));

        return componentTypes.ToArray();
    }
#endif

    public static IEnumerable<Type> GetTypeHierarchy(this Type type)
    {
        Contract.Requires(type != null);
        var currentType = type;

        while (currentType != null) {
            yield return currentType;

            currentType = currentType.BaseType;
        }
    }

}

//[WorldSystemFilter(WorldSystemFilterFlags.GameObjectConversion)]
//[UpdateInGroup(typeof(GameObjectAfterConversionGroup))]

//[ UpdateAfter(typeof(InitializationSystemGroup)) ]
//[ UpdateInGroup(typeof(InitializationSystemGroup)) ]
//[UpdateAfter(typeof(UpdateWorldTimeSystem))]
public abstract class Controller /*: SystemBase*/ {

    // public static World m_World;
    // protected static EntityCommandBuffer.ParallelWriter m_Ecb;
    protected static bool m_Result;

    //public static EntityManager m_EntityManager;
    //public static readonly EntityManager entityManager = m_EntityManager;
    //protected static ComponentType m_outType;
    // protected static EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
    //public static GameObjectConversionSystem m_Conversion;
    // public static Dictionary<Transform, Entity> primaryEntity = new Dictionary<Transform, Entity>();
    //public EntityQuery m_Query;
    public virtual Type CurruentState { get; set; }

    //public static EntitiesCache Cache => EntitiesCache.Instance;

    protected static void SetResult(bool value)
    {
        m_Result = value;
    }

    public abstract void RunMethod<T1>();

    public abstract void RunMethod<T1>(Type state);

    //
    //protected static ComponentType GetOutType() => m_outType;

    //public List<T1> Find<T1, TV>() where T1 : Component where TV : IComponentData =>
    //    Find<T1>(typeof(TV));

    // public T1 FindOne<T1, TV>(params Type[] all) where T1 : Component where TV : IComponentData
    // {
    //     var arr = (all ?? new Type[] { }).ToList();
    //     arr.Add(typeof(TV));
    //
    //     return FindOne<T1>(arr.ToArray());
    // }
    //
    // public T1 FindOne<T1>(params Type[] all) where T1 : Component => Find<T1>(all)?.FirstOrDefault();

    //public List<T1> Find<T1>(params Type[] all) where T1 : Component => FindAll<T1>(all);

    public static List<T> FindObjectsOfTypeAll<T>()
    {
        return SceneManager.GetActiveScene()
            .GetRootGameObjects()
            .SelectMany(g => g.GetComponentsInChildren<T>(true))
            .ToList();
    }

    // public List<T1> Find<T1, T2>() where T1 : Component where T2 : IComponentData => Find<T1>(typeof(T2));
    // public List<T1> Find<T1>(params Type[] types) where T1 : Component => FindAll<T1>(types);
    // public List<Transform> TFind<T1>() where T1 : IComponentData => Find<Transform, T1>();
    // public Transform TFindOne<T1>() where T1 : IComponentData => Find<Transform, T1>()?.FirstOrDefault();
    // public List<RectTransform> RFind<T1>() where T1 : IComponentData => Find<RectTransform, T1>();
    //
    // public RectTransform RFindOne<T1>() where T1 : IComponentData =>
    //     Find<RectTransform, T1>()?.FirstOrDefault();

    // public List<T1> FindAll<T1>(IEnumerable<Type> types = null, object action = null) where T1 : Component
    // {
    //     var ts = (types ?? new Type[] { }).ToList();
    //     ts.Add(typeof(T1));
    //     var res = new List<T1>();
    //
    //     if(typeof(UIBehaviour).IsAssignableFrom(typeof(T1)) ||
    //         typeof(RectTransform).IsAssignableFrom(typeof(T1))) {
    //         //Debug.Log($"find ui: {typeof(T1).Name}");
    //
    //         // if( action is Action<RectTransform, Entity, int> act ) {
    //         //     GetAllUi(all: ts, action: act);
    //         // } else {
    //         GetAllUi(ts, action: (transform, entity, i) => {
    //             if(action == null ||
    //                 action is Func<RectTransform, Entity, int, bool?> acts &&
    //                 acts.Invoke(transform, entity, i) != false) {
    //                 var cc = transform?.GetComponent<T1>();
    //
    //                 if(cc != null) {
    //                     res.Add(cc);
    //                 }
    //             }
    //         });
    //
    //         // }
    //     } else {
    //         // if( action is Action<Transform, Entity, int> act ) {
    //         //     GetAll(all: ts, action: act);
    //         // } else {
    //         GetAll(ts, action: (transform, entity, i) => {
    //             if(action == null ||
    //                 action is Func<Transform, Entity, int, bool?> acts &&
    //                 acts.Invoke(transform, entity, i) != false) {
    //                 var cc = transform?.GetComponent<T1>();
    //
    //                 if(cc != null) {
    //                     res.Add(cc);
    //                 }
    //             }
    //
    //             // var cc = transform?.GetComponent<T1>();
    //             // if( cc != null ) res.Add(cc);
    //         });
    //
    //         // }
    //     }
    //
    //     //ConvertToEntity
    //
    //     return res;
    // }

    /*
    public List<T1> FindAll<T1>(Type[] all = null, Type[] any = null, Type[] none = null)
        where T1 : Component
    {
        var result = new List<T1>();

        var res = GetAll(typeof(T1), query(all: all, none: none, any: any));
        Debug.Log($"all count: {all?.Length} result: {res?.Count} type: {typeof(T1).Name}");
        res?.ForEach(t => {
            if( t is T1 c ) { result.Add(c); }

        });

        return result;
    }
    */

    // public virtual void ExecuteNode(TNode node, Finder finder)
    // {
    //     m_Ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
    //     //m_EntityManager = EntityManager;
    //     m_Query = GetEntityQuery(query(finder.FindIn));
    //     RunMethod<E_RunNode>();
    //
    //     finder.ResultAdd?.ForEach(tk => {
    //         if(tk.Value == null) {
    //             return;
    //         }
    //
    //         m_outType = ComponentType.ReadWrite(tk.Key.GetType());
    //         m_Query = GetEntityQuery(query(tk.Value));
    //
    //         Entities.WithStoreEntityQueryInField(ref m_Query)
    //             .ForEach((Entity e, int entityInQueryIndex) => {
    //                 if(!m_EntityManager.HasComponent(e, m_outType)) {
    //                     m_Ecb.AddComponent(entityInQueryIndex, e, m_outType);
    //                 }
    //             })
    //             .Schedule(Dependency)
    //             .Complete();
    //     });
    //
    //     finder.ResultRemove?.ForEach(tk => {
    //         if(tk.Value == null) {
    //             return;
    //         }
    //
    //         m_outType = ComponentType.ReadWrite(tk.Key.GetType());
    //         m_Query = GetEntityQuery(query(tk.Value));
    //
    //         Entities.WithStoreEntityQueryInField(ref m_Query)
    //             .ForEach((Entity e, int entityInQueryIndex) => {
    //                 if(m_EntityManager.HasComponent(e, m_outType)) {
    //                     m_Ecb.RemoveComponent(entityInQueryIndex, e, m_outType);
    //                 }
    //             })
    //             .Schedule(Dependency)
    //             .Complete();
    //     });
    //
    //     finder.OutResult = new List<bool>();
    //
    //     finder.FindOut?.ForEach(q => {
    //         m_Query = GetEntityQuery(query(q));
    //         m_Result = false;
    //
    //         Entities.WithStoreEntityQueryInField(ref m_Query)
    //             .ForEach((Entity e, int entityInQueryIndex) => {
    //                 //
    //                 SetResult(true);
    //             })
    //             //.Run();
    //             .Schedule(Dependency)
    //             .Complete();
    //
    //         finder.OutResult.Add(m_Result);
    //     });
    // }
    //
    // public EntityQueryDesc query(Type[] all = null, Type[] none = null, Type[] any = null)
    // {
    //     var ct_all = new List<ComponentType>();
    //     var ct_none = new List<ComponentType>();
    //     var ct_any = new List<ComponentType>();
    //     all?.ForEach(t => ct_all.Add(t));
    //     none?.ForEach(t => ct_none.Add(t));
    //     any?.ForEach(t => ct_any.Add(t));
    //
    //     return new EntityQueryDesc {
    //         None = ct_none.ToArray(),
    //         Any = ct_any.ToArray(),
    //         All = ct_all.ToArray(),
    //     };
    // }
    //
    // public EntityQueryDesc query(Dictionary<TNode.QueryType, List<IComponentData>> find)
    // {
    //     var ct_all = new List<ComponentType>();
    //     var ct_none = new List<ComponentType>();
    //     var ct_any = new List<ComponentType>();
    //     // var all = new List<Type>();
    //     // var none = new List<Type>();
    //     // var any = new List<Type>();
    //     List<IComponentData> the_all = null;
    //     List<IComponentData> the_any = null;
    //     List<IComponentData> the_none = null;
    //
    //     if(find != null) {
    //         if(find.TryGetValue(TNode.QueryType.WithAll, out the_all)) {
    //             the_all?.ForEach(t1 => ct_all.Add(t1.GetType()));
    //         }
    //
    //         if(find.TryGetValue(TNode.QueryType.WithAny, out the_any)) {
    //             the_any?.ForEach(t1 => ct_any.Add(t1.GetType()));
    //         }
    //
    //         if(find.TryGetValue(TNode.QueryType.WithNone, out the_none)) {
    //             the_none?.ForEach(t1 => ct_none.Add(t1.GetType()));
    //         }
    //     }
    //
    //     // all.ForEach(t => ct_all.Add(t));
    //     // any.ForEach(t => ct_any.Add(t));
    //     // none.ForEach(t => ct_none.Add(t));
    //     var query = new EntityQueryDesc {
    //         None = ct_none.ToArray(),
    //         Any = ct_any.ToArray(),
    //         All = ct_all.ToArray(),
    //     };
    //
    //     return query;
    // }

    /*
   public List<Component> GetAll(Type component, EntityQueryDesc queryDesc)
   {
       m_InGroup = GetEntityQuery(queryDesc);
       // var component = typeof(T);
       var result = new List<Component>();
       Entities.WithoutBurst()
           .WithStoreEntityQueryInField(ref m_InGroup)
           .ForEach((Transform transform, Entity e, int entityInQueryIndex) => {
               Debug.Log(transform.gameObject);
               var t = transform.GetComponent(component);

               if( t != null ) {
                   if( !TransformEntity.ContainsKey(transform) ) {
                       TransformEntity[ transform ] = e;
                   }

                   result.Add(t);
               }
           })
           .Run();

       ClearTransformEntityCache();

       return result;
   }
   */

    // public int GetAll(IEnumerable<Type> all = null, IEnumerable<Type> any = null,
    //     IEnumerable<Type> none = null, Action<Transform, Entity, int> action = null)
    // {
    //     //var ret = new List<Component>();
    //     var _all = (all?.GetComponentType() ?? new ComponentType[] { }).ToList();
    //
    //     if(all?.Contains(typeof(Transform)) != true) {
    //         _all.Add(typeof(Transform));
    //         //_all.Add(typeof(RectTransform));
    //     }
    //
    //     //m_Query.ResetFilter();
    //     m_Query = GetEntityQuery(new EntityQueryDesc {
    //         All = _all.ToArray(),
    //         Any = any?.GetComponentType() ?? new ComponentType[] { },
    //         None = none?.GetComponentType() ?? new ComponentType[] { },
    //         Options = EntityQueryOptions.IncludeDisabled | EntityQueryOptions.IncludePrefab,
    //     });
    //
    //     var transforms = m_Query.ToComponentArray<Transform>();
    //     var entities = m_Query.ToEntityArray(Allocator.Persistent);
    //
    //     for(var j = 0; j < transforms.Length; j++) {
    //         primaryEntity[transforms[j]] = entities[j];
    //         action?.Invoke(transforms[j], entities[j], j);
    //     }
    //
    //     m_Query.ResetFilter();
    //     entities.Dispose();
    //     ClearTransformEntityCache();
    //
    //     return entities.Length;
    //
    //     var i = 0;
    //
    //     Entities.WithoutBurst()
    //         //.WithStoreEntityQueryInField(ref m_Query)
    //         .ForEach((Transform transform, Entity entity, int entityInQueryIndex) => {
    //             var t_any = any?.Any(type => m_EntityManager.HasComponent(entity, type));
    //             var t_all = all?.Any(type => !m_EntityManager.HasComponent(entity, type));
    //             var t_none = none?.Any(type => m_EntityManager.HasComponent(entity, type));
    //
    //             var has = (t_any == true || t_all == false || t_any == null && t_all == null) &&
    //                 t_all != true;
    //
    //             if(has) {
    //                 i += 1;
    //                 primaryEntity[transform] = entity;
    //                 action?.Invoke(transform, entity, entityInQueryIndex);
    //             }
    //
    //             // i += 1;
    //             // action?.Invoke(transform, entity, entityInQueryIndex);
    //             //ret.Add(transform.GetComponent(component));
    //             //Debug.Log(transform.gameObject.name, transform.gameObject);
    //         })
    //         .Run();
    //
    //     ClearTransformEntityCache();
    //
    //     return i;
    // }
    //
    // public int GetAllUi(IEnumerable<Type> all = null, IEnumerable<Type> any = null,
    //     IEnumerable<Type> none = null, Action<RectTransform, Entity, int> action = null)
    // {
    //     //var ret = new List<Component>();
    //     var _all = (all?.GetComponentType() ?? new ComponentType[] { }).ToList();
    //
    //     if(all?.Contains(typeof(RectTransform)) != true) {
    //         _all.Add(typeof(RectTransform));
    //     }
    //
    //     m_Query = GetEntityQuery(new EntityQueryDesc {
    //         All = _all.ToArray(), //all?.GetComponentType() ?? new ComponentType[] { },
    //         Any = any?.GetComponentType() ?? new ComponentType[] { },
    //         None = none?.GetComponentType() ?? new ComponentType[] { },
    //         Options = EntityQueryOptions.IncludeDisabled | EntityQueryOptions.IncludePrefab,
    //     });
    //
    //     var transforms = m_Query.ToComponentArray<RectTransform>();
    //     var entities = m_Query.ToEntityArray(Allocator.Persistent);
    //
    //     for(var j = 0; j < transforms.Length; j++) {
    //         primaryEntity[transforms[j]] = entities[j];
    //         action?.Invoke(transforms[j], entities[j], j);
    //     }
    //
    //     m_Query.ResetFilter();
    //     entities.Dispose();
    //     ClearTransformEntityCache();
    //
    //     return entities.Length;
    //
    //     var i = 0;
    //
    //     Entities.WithoutBurst()
    //         //.WithStoreEntityQueryInField(ref m_Query)
    //         .ForEach((RectTransform transform, Entity entity, int entityInQueryIndex) => {
    //             var t_any = any?.Any(type => m_EntityManager.HasComponent(entity, type));
    //             var t_all = all?.Any(type => !m_EntityManager.HasComponent(entity, type));
    //             var t_none = none?.Any(type => m_EntityManager.HasComponent(entity, type));
    //
    //             var has = (t_any == true || t_all == false || t_any == null && t_all == null) &&
    //                 t_all != true;
    //
    //             if(has) {
    //                 i += 1;
    //                 primaryEntity[transform] = entity;
    //                 action?.Invoke(transform, entity, entityInQueryIndex);
    //             }
    //
    //             //ret.Add(transform.GetComponent(component));
    //             //Debug.Log(transform.gameObject.name, transform.gameObject);
    //         })
    //         .Run();
    //
    //     ClearTransformEntityCache();
    //
    //     return i;
    // }
    //
    // public static void AddRemoveComponentData(Transform transform, Type[] removeTypes, Type[] addTypes = null)
    // {
    //     if(m_EntityManager == null || !primaryEntity.TryGetValue(transform, out var entity)) {
    //         return;
    //     }
    //
    //     // var entity = TransformEntity[ transform ];
    //     removeTypes?.ForEach(t => {
    //         if(typeof(IComponentData).IsAssignableFrom(t) &&
    //             m_EntityManager.HasComponent(entity, ComponentType.ReadWrite(t))) {
    //             m_EntityManager.RemoveComponent(entity, ComponentType.ReadWrite(t));
    //         }
    //     });
    //
    //     addTypes?.ForEach(t => {
    //         if(typeof(IComponentData).IsAssignableFrom(t) &&
    //             !m_EntityManager.HasComponent(entity, ComponentType.ReadWrite(t))) {
    //             m_EntityManager.AddComponent(entity, ComponentType.ReadWrite(t));
    //         }
    //     });
    // }
    //
    // protected override void OnCreate()
    // {
    //     base.OnCreate();
    //     Debug.Log($"system init: {GetType().GetNiceName()}");
    //
    //     //if( m_EntityManager == default ) {
    //     m_EntityManager = EntityManager;
    //     // }
    //
    //     //if( m_EndSimulationEcbSystem == null ) {
    //     m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    //     //}
    //
    //     //if( m_World == null ) {
    //     m_World = World;
    //     // }
    //
    //     // if( m_Conversion == null ) {
    //     //     m_Conversion = World.GetOrCreateSystem<MyGameObjectConversionSystem>();
    //     //
    //     //     if( m_Conversion == null ) {
    //     //         Debug.LogError("conversion system not exists");
    //     //     }
    //     // }
    //
    //     // Assert.IsNotNull(App.Instance, "main instance not found");
    //     Assert.IsNotNull(World.DefaultGameObjectInjectionWorld, "DefaultGameObjectInjectionWorld is null");
    //
    //     // if( Main.Instance.GetComponent<GameObjectEntity>()?.Entity == Entity.Null ) {
    //     //     Debug.LogError("entity conversion system not work");
    //     // }
    // }
    //
    // static void ClearTransformEntityCache()
    // {
    //     primaryEntity?.ForEach(t => {
    //         if(t.Key == null) {
    //             primaryEntity.Remove(t.Key);
    //         }
    //     });
    // }

}

// public class EntitiesCache : Dictionary<IComponentData, Entity>, IDisposable {
//
//     static int refs;
//     public static EntitiesCache Instance = new EntitiesCache();
//
//     public void Dispose()
//     {
//         refs -= 1;
//
//         if(refs < 1) {
//             Instance.Clear();
//         }
//     }
//
//     public EntitiesCache Use()
//     {
//         refs += 1;
//
//         return Instance;
//     }
//
// }

public static class ControllerExtensions {

    //public static EntitiesCache entitiesCache => EntitiesCache.Instance;

    // public static bool TryGetData<T>(this GameObject gameObject, out T data)
    //     where T : struct, IComponentData => TryGetData(gameObject.transform, out data);

    // public static bool TryGetData<T>(this Component component, out T data) where T : struct, IComponentData =>
    //     TryGetData(component.transform, out data);
    //
    // public static bool TryGetData<T>(this Transform transform, out T data) where T : struct, IComponentData
    // {
    //     data = new T();
    //
    //     if(!Controller.primaryEntity.TryGetValue(transform, out var entity) || entity == Entity.Null) {
    //         return false;
    //     }
    //     //var entity = Controller.m_Conversion.GetPrimaryEntity(transform);
    //
    //     if(Controller.m_EntityManager.HasComponent<T>(entity)) {
    //         data = Controller.m_EntityManager.GetComponentData<T>(entity);
    //         entitiesCache[data] = entity;
    //
    //         return true;
    //     }
    //
    //     return false;
    // }

    // public static void Save(this IComponentData data)
    // {
    //     if(entitiesCache.TryGetValue(data, out var entity)) {
    //         Controller.m_EntityManager.SetComponentData(entity, data);
    //         entitiesCache.Remove(data);
    //     }
    // }

    // public static void RemoveData<T>(this GameObject component) where T : IComponentData
    // {
    //     Controller.AddRemoveComponentData(component.transform, new[] { typeof(T) });
    // }

    // public static void RemoveData(this Transform transform, Type[] types) =>
    //     Controller.AddRemoveComponentData(transform, types);

    // public static void AddData<T>(this GameObject component) where T : IComponentData
    // {
    //     Controller.AddRemoveComponentData(component.transform, null, new[] { typeof(T) });
    // }

    // public static void RemoveData<T>(this Component component) where T : IComponentData
    // {
    //     Controller.AddRemoveComponentData(component.transform, new[] { typeof(T) });
    // }

    // public static void RemoveData(this Transform transform, Type[] types) =>
    //     Controller.AddRemoveComponentData(transform, types);

    // public static void AddData<T>(this Component component) where T : IComponentData
    // {
    //     Controller.AddRemoveComponentData(component.transform, null, new[] { typeof(T) });
    // }
    //
    // public static void RemoveData<T>(this Transform transform) where T : IComponentData
    // {
    //     Controller.AddRemoveComponentData(transform, new[] { typeof(T) });
    // }

    // public static void RemoveData(this Transform transform, Type[] types) =>
    //     Controller.AddRemoveComponentData(transform, types);

    // public static void AddData<T>(this Transform transform) where T : IComponentData
    // {
    //     Controller.AddRemoveComponentData(transform, null, new[] { typeof(T) });
    // }
    //
    // // new.................
    //
    // public static void RemoveData(this GameObject component, params Type[] types)
    // {
    //     Controller.AddRemoveComponentData(component.transform, types);
    // }
    //
    // // public static void RemoveData(this Transform transform, Type[] types) =>
    // //     Controller.AddRemoveComponentData(transform, types);
    //
    // public static void AddData(this GameObject component, params Type[] types)
    // {
    //     Controller.AddRemoveComponentData(component.transform, null, types);
    // }
    //
    // public static void RemoveData(this Component component, params Type[] types)
    // {
    //     Controller.AddRemoveComponentData(component.transform, types);
    // }
    //
    // // public static void RemoveData(this Transform transform, Type[] types) =>
    // //     Controller.AddRemoveComponentData(transform, types);
    //
    // public static void AddData(this Component component, params Type[] types)
    // {
    //     Controller.AddRemoveComponentData(component.transform, null, types);
    // }
    //
    // public static void RemoveData(this Transform transform, params Type[] types)
    // {
    //     Controller.AddRemoveComponentData(transform, types);
    // }
    //
    // // public static void RemoveData(this Transform transform, Type[] types) =>
    // //     Controller.AddRemoveComponentData(transform, types);
    //
    // public static void AddData(this Transform transform, params Type[] types)
    // {
    //     Controller.AddRemoveComponentData(transform, null, types);
    // }

}

}