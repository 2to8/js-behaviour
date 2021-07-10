using GameEngine.Kernel;
using GameEngine.Utils;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using SqlCipher4Unity3D;
using SQLite.Attributes;
using SQLiteNetExtensions.Extensions;
using System;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Util;

#if ECS
using Unity.Entities;
#endif

namespace GameEngine.Models.Contracts
{
    public interface Idle { }

    public interface ISingle
    {
        void SetInstance(ScriptableObject target);
    }

    [CreateAssetMenu(fileName = nameof(DbTable<T>), menuName = "Models/" + nameof(DbTable<T>), order = 0)]
    public class DbTable<T> : TableBase, IData, ISingle/*, IObjectInitializationDataProvider*/ where T : DbTable<T>
    {
        //public static T CreateOne() => ScriptableObject.CreateInstance<T>();
        protected static T m_Instance;

        protected bool m_TableInited;

        protected virtual void OnEnable()
        {
            m_TableInited = true;

            //        if(m_Instance != null) throw new UnityException(typeof(T) + " is already instantiated");
            if (m_Instance == null) {
                m_Instance = (T)this;
            }
        }

        protected virtual void OnDisable()
        {
            if (m_Instance == this) {
                m_Instance = null;
            }
        }

        public void SaveChange(bool force = false)
        {
            if (!m_TableInited) {
                return;
            }

        #if UNITY_EDITOR
            if (!Application.isPlaying || force) {
                UnityEditor.EditorUtility.SetDirty(this);
                Debug.Log("set dirty");
                UnityEditor.AssetDatabase.SaveAssets();

                //UnityEditor.AssetDatabase.Refresh();
            }
        #endif
        }

        protected static void SetValue<T>(ref T target, T value)
        {
            target = value;

            // if (m_Instance != null) {
            //     m_Instance.SaveChange();
            // }
        }

        [SerializeField, HideInInspector]
        Type m_DefaultState = typeof(Idle);

        // [OdinSerialize]
        // public bool AUTO_SAVE { get; set; }

        // public static SQLiteConnection knex_db => Core.Connection;
        //
        // [ PrimaryKey, AutoIncrement, OdinSerialize,ShowInInspector ]
        // public virtual int Id { get; set; }
    #if ECS
    [Ignore, FoldoutGroup("System"), FormerlySerializedAs("m_Mothod"), ValueDropdown(nameof(GetSystemMethod))]
    public string m_Method;
    #endif
    #if ECS
    [Ignore, FoldoutGroup("System"), ValueDropdown(nameof(GetSystemList))]
    public Type m_System;
    #endif

        public static T instance {
        #region Instance define

            get {
                if (m_Instance == null) {
                    m_Instance = Core.FindOrCreatePreloadAsset<T>(); // Res.Load<T>() ?? CreateInstance<T>();
                }

                //if(m_Instance == null) {
                //UniTask.WhenAll(loadInstance($"Config/{typeof(T).Name}.asset"));

                //m_Instance = Resources.Load<T>($"Data/{typeof(T).Name}");
                // if ( m_Instance == null ) {
                //     loadInstance($"{fileName}.asset")
                //         .ToObservable( /*Scheduler.Immediate*/)
                //         .Subscribe(t => { m_Instance = t; });
                // }

                //Resources.Load<T>("Config/" + typeof(T).Name);
                //}

                // if(m_Instance == null) {
                //     Debug.LogError("Config/" + typeof(T).Name + ".asset not exist");
                // }
                return m_Instance;
            }
            set { m_Instance = value; }

        #endregion
        }

        [Ignore]
        public Type CurrentState { get; set; }

        [Ignore]
        public Type DefaultState {
            get => m_DefaultState;
            set => m_DefaultState = value;
        }

        public void RunMethod(Type type, object target, Type state = null, object[] values = null) { }

        protected virtual void Awake()
        {
            if (m_Instance == null) {
                m_Instance = (T)this;
            }
        }

        // protected virtual void OnValidate()
        // {
        //     Debug.Log("saved");
        //     Save();
        // }
        public static T GetInstance() => instance;

        public static T Create() => CreateInstance<T>();

        protected virtual void OnValidate()
        {
            // if (m_Instance == this) {
            //     SaveChange();
            // }
        }
    #if ECS
    Type[] GetSystemList()
    {
        return Core.GetTypes<SystemBase>()
            .Where(t => t.BaseType != null &&
                t.BaseType.IsGenericType && t.BaseType.GetGenericArguments().Last() == typeof(T))
            .ToArray();
    }
    #endif
    #if ECS
    string[] GetSystemMethod()
    {
        return m_System
            ?.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
                BindingFlags.DeclaredOnly)
            .Select(mi => mi.Name)
            .Where(s => !new[] { "OnUpdate", "OnCreate" }.Contains(s))
            .ToArray();
    }
    #endif
    #if ECS
    [FoldoutGroup("System"), Button(ButtonSizes.Large)]
    void RunSystem()
    {
        var func = m_System?.GetMethod(m_Method,
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        func?.Invoke(World.DefaultGameObjectInjectionWorld.GetOrCreateSystem(m_System), new object[] { });
    }
    #endif

    #region ============= Database ==============

        public T FirstOrDefault(Expression<Func<T, bool>> expr = null, Action<T> func = null)
        {
            var ret = Table().FirstOrDefault(expr);

            if (ret.Id == 0 && func != null) {
                func?.Invoke(ret);
                ret._保存表();
            }

            return ret;

            //var query = _db.Table<T>().FirstOrDefault(expr) ?? CreateInstance<T>();
            // var ret = Core.FindOrCreatePreloadAsset<T>();
            // if (query.Id == 0) {
            //     func?.Invoke(expr == null ? ret : query);
            //     _db.Insert(expr == null ? ret : query);
            // }
            // if (expr == null) {
            //     JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(query), ret);
            // }
            // if (ret == null) {
            //     // if (typeof(ScriptableObject).IsAssignableFrom(typeof(T))) {
            //     ret = CreateInstance(typeof(T)) as T;
            //
            //     // } else {
            //     //     ret = new T();
            //     // }
            //     _db.Insert(ret);
            // }
            //
            // // 同步数据库结构
            // _db.CreateTable<T>();
            // _db.GetChildren(ret, true);
            // return ret;
        }

        // [ButtonGroup("Common")]
        // public void _Update()
        // {
        //     //FirstOrDefault();
        //     Updated = Core.TimeStamp();
        //     _db.Update(this);
        //     _db.UpdateWithChildren(this);
        // }

        [ButtonGroup("Common"), FoldoutGroup("base")]
        public void _保存表(bool replace = true)
        {
            _db.CreateTable<T>();
            Updated = Core.TimeStamp();

            if (Id == 0) {
                if (replace) {
                    _db.InsertOrReplaceWithChildren(this, true);
                } else {
                    _db.InsertWithChildren(this);
                }
            } else {
                _db.UpdateWithChildren(this);
            }

            //FirstOrDefault();
            // Updated = Core.TimeStamp();
            // _db.InsertOrReplaceWithChildren(this, true);
        }

        public T DB_Fetch()
        {
            _db.GetChildren(this, true);

            return (T)this;
        }

        public static TableQuery<T> table => instance.Table();

        public TableQuery<T> Table()
        {
            // 同步数据库结构
            _db.CreateTable<T>();

            return _db.Table<T>();
        }

        public TableQuery<T> Where(Expression<Func<T, bool>> predExp) => _db.Table<T>().Where(predExp);

        // public static T FirstOrDefault(Expression<Func<T, bool>> predExpr) =>
        //     _db.Table<T>().Where(predExpr).FirstOrDefault();

        public void DB_SaveAsDefault()
        {
            var id = Id;
            Id = 1;
            _保存表();
            Id = id;
        }

        public void DB_Insert(bool replace = true)
        {
            _db.CreateTable<T>();

            if (replace) {
                _db.InsertOrReplaceWithChildren(this);
            } else {
                _db.InsertWithChildren(this);
            }
        }

        public void DB_Delete()
        {
            _db.Delete(this);
        }

        //[Button]
        //[TitleGroup("Common/Database")]
        // [ ButtonGroup("Common") ]
        // public void Refresh(T obj = null)
        // {
        //     GetType()
        //         .GetProperties()
        //         .ForEach(attr => {
        //             if ( attr.CanWrite ) {
        //                 attr.SetValue(this, attr.GetValue(obj == null ? this : obj,null));
        //             }
        //         });
        // }

        [ButtonGroup("Common")]
        public void _加载表()
        {
            _db.CreateTable<T>();
            var t = _db.Find<T>(Id);

            //Debug.Log($"id: {this.Id} data: {JsonUtility.ToJson(t)}");
            if (t != null) {
                JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(t), this);
            }

            //Refresh(t);
        }

        public void DB_LoadWhere(Expression<Func<T, bool>> predExp, Action action = null)
        {
            var tmp = FirstOrDefault(predExp);
            var id = Id;
            DB_LoadDefault(tmp);

            if (id != Id) {
                action?.Invoke();
                _保存表();
            }
        }

        //[Button]
        //[TitleGroup("Common/Database")]
        // public void Save()
        // {
        //     //var tmp = FirstOrDefault() ?? CreateInstance<T>();
        //     //Id = tmp.Id;
        //     Save();
        //     // Refresh();
        // }

        [ButtonGroup("Common")]
        public void _删除表()
        {
            _db.DropTable<T>();
            _db.CreateTable<T>();
        }

        public void DB_LoadDefault(T tIn)
        {
            var tInType = tIn.GetType();

            foreach (var itemOut in GetType().GetProperties()) {
                if (!itemOut.CanWrite) {
                    continue;
                }
                var itemIn = tInType.GetProperty(itemOut.Name);

                if (itemIn != null) {
                    itemOut.SetValue(this, itemIn.GetValue(tIn));
                }
            }

            // this.GetType()
            //     .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            //     .Where(property => property.CanRead) // Not necessary
            //     .ForEach(property => property.SetValue(origin,
            //         origin.GetType().GetProperties().Where(ta => ta == property).Select(tb => tb.GetValue(origin))));
        }

    #endregion

        // public ObjectInitializationData CreateObjectInitializationData()
        // {
        //     m_Instance = m_Instance ??= Core.FindOrCreatePreloadAsset<T>();
        // #if UNITY_EDITOR
        //     if (Application.isEditor) {
        //         return ObjectInitializationData.CreateSerializedInitializationData<T>(typeof(T).Name, m_Instance);
        //     }
        // #endif
        //     return default;
        // }
        //
        // [Ignore]
        // public string Name => typeof(T).Name;
        public void SetInstance(ScriptableObject target)
        {
            m_Instance = target as T;
        }
    }
}