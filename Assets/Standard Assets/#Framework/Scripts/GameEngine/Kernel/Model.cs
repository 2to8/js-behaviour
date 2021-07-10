using GameEngine.Contacts;
using GameEngine.Domain;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using SqlCipher4Unity3D;
using SQLite.Attributes;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensions.Extensions.TextBlob;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using SQLiteNetExtensions.Extensions.TextBlob.Serializers;
using UniRx.Async;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using Core_1 = Core;

#if UNITY_EDITOR
#endif

namespace GameEngine.Kernel
{
    #region old

#if false
    /// <summary>
    /// 模型数据
    /// </summary>
    public abstract class Model : Config<Model>,IModel
    {
        //模型标识
       // public abstract string Name { get; set; }

        //发送事件
        protected void SendEvent(string eventName, object data = null) {
            Core.SendEvent(eventName, data);
        }

        public int Id { get; set; }
    }
#endif

    #endregion

    //public class DataConfig : App_Bootstrap { }

    [Preserve]
    public class Model<T> : Config<T>, ITextBlobSerializer, IModel where T : Model<T>, new()
    {
        protected static T m_Instance;
        public static SQLiteConnection Connection => Core_1.Connection;

        public static T Instance {
            get {
                var prefix = $"{typeof(T).Namespace}".Split('.').First();
                if (prefix.IsNullOrWhitespace()) {
                    prefix = "Data";
                }

                var parts = typeof(T).FullName?.Split('.');
                var suffix = parts?.Length > 1 ? parts[parts.Length - 2] + "_" : "";
                var fileName = $"{prefix}/{suffix}{typeof(T).Name}";
                if (m_Instance == null) {
                    m_Instance = Resources.Load<T>(fileName);
                }

#if UNITY_EDITOR
                if (m_Instance == null) {
                    var cd = Directory.GetCurrentDirectory();
                    var path = Directory.Exists($"{cd}/Assets/{prefix}")
                        ? $"Assets/{prefix}/Resources/{prefix}"
                        : $"Assets/Resources/{prefix}";
                    if (!Directory.Exists($"{cd}/{path}")) {
                        Directory.CreateDirectory($"{cd}/{path}");
                    }

                    var assetName = $"{path}/" + suffix + typeof(T).Name + ".asset";

                    //m_Instance = Addressables.LoadAssetAsync<T>($"Data/Config/{typeof(T).Name}.asset").Result;
                    //m_Instance = AssetDatabase.LoadAssetAtPath<T>(assetName);

                    //  Resources.Load<T>("Config/" + typeof(T).Name);
                    //if(m_Instance == null) {
                    var asset = CreateInstance<T>();

                    //var path = "Assets/Resources/Data";
                    //var assetPathAndName =
                    // UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).Name +
                    //     ".asset");
                    AssetDatabase.CreateAsset(asset, assetName);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    m_Instance = Resources.Load<T>(fileName);

                    // }
                }

#endif

                //if(m_Instance == null) {
                //UniTask.WhenAll(loadInstance($"Config/{typeof(T).Name}.asset"));

                //m_Instance = Resources.Load<T>($"Data/{typeof(T).Name}");

                // if( m_Instance == null ) {
                //     loadInstance($"{fileName}.asset")
                //         .ToObservable( /*Scheduler.Immediate*/)
                //         .Subscribe(t => {
                //             m_Instance = t;
                //         });
                // }

                //Resources.Load<T>("Config/" + typeof(T).Name);
                //}

                // if(m_Instance == null) {
                //     Debug.LogError("Config/" + typeof(T).Name + ".asset not exist");
                // }
                return m_Instance;
            }
            set { m_Instance = value; }
        }

        //[BoxGroup("Common", ShowLabel = false)]

        [PrimaryKey, AutoIncrement, ShowInInspector]
        public int Id { get; set; }

        public string Serialize(object element)
        {
            TypeDescriptor.AddAttributes(typeof((int, int)),
                new TypeConverterAttribute(typeof(TupleConverter<int, int>)));
            var str = JsonConvert.SerializeObject(element);
            return str;
        }

        public object Deserialize(string text, Type type)
        {
            TypeDescriptor.AddAttributes(typeof((int, int)),
                new TypeConverterAttribute(typeof(TupleConverter<int, int>)));
            var result = JsonConvert.DeserializeObject(text, type);
            return result;
        }

        void Awake()
        {
            if (m_Instance == null) {
                m_Instance = (T) this;
            }
        }

        public static bool AddressableResourceExists(object key)
        {
            foreach (var l in Addressables.ResourceLocators) {
                IList<IResourceLocation> locs;
                if (key != null && l.Locate(key, typeof(T), out locs)) {
                    return true;
                }
            }

            return false;
        }

        public static async UniTask<T> loadInstance(string path = "")
        {
            if (m_Instance == null) {
                if (path.IsNullOrWhitespace()) {
                    path = $"Data/{typeof(T).Name}.asset";
                }

                await Addressables.InitializeAsync().Task;
                var exists = AddressableResourceExists(path);
                var res = exists ? await Addressables.LoadAssetAsync<T>(path).Task : FirstOrDefault();
                m_Instance = res;
                Debug.Log($"{typeof(T).Name}  loaded Asset: {exists}");
            }

            return m_Instance;
        }

        public static async UniTask LoadDefaultAsset()
        {
            await loadInstance();
            if (m_Instance != null) {
                Debug.Log($"{typeof(T).Name} instace loaded");
            }
        }

        // public static void Self(Action<T> action)
        // {
        //     // loadInstance()
        //     //     .ToObservable( /*Scheduler.Immediate*/)
        //     //     .Subscribe(t => {
        //     //         action?.Invoke(Instance);
        //     //     });
        // }
        public static T GetInstance() => Instance;

        //模型标识
        //public override string Name { get; set; } = typeof(T).Name;

        //发送事件
        //public override string Name { get; }

        protected void SendEvent(string eventName, object data = null)
        {
            Core.SendEvent(eventName, this, data);
        }

        protected void SendEvent<A>(object data = null)
        {
            Core.SendEvent(typeof(A).Name, this, data);
        }

        public static async UniTask<T> LoadDefault(string path = null) =>
            await Addressables.LoadAssetAsync<T>(path ?? $"Config/{typeof(T).Name}.asset").Task;

        public void Update()
        {
            Connection.CreateTable<T>();
            Connection.UpdateWithChildren(this);
        }

        public void Insert(bool replace = true)
        {
            Connection.CreateTable<T>();
            if (replace) {
                Connection.InsertOrReplaceWithChildren(this);
            }
            else {
                Connection.InsertWithChildren(this);
            }
        }

        public void Delete()
        {
            Connection.Delete(this);
        }

        public T Fetch()
        {
            Connection.GetChildren(this, true);
            return (T) this;
        }

        public static TableQuery<T> Table()
        {
            // 同步数据库结构
            Connection.CreateTable<T>();
            return Connection.Table<T>();
        }

        public static TableQuery<T> Where(Expression<Func<T, bool>> predExp) => Connection.Table<T>().Where(predExp);

        public static T FirstOrDefault(Expression<Func<T, bool>> predExpr) =>
            Connection.Table<T>().Where(predExpr).FirstOrDefault();

        public static T FirstOrDefault()
        {
            var query = Connection.Table<T>().Take(1);
            var ret = query.ToList().FirstOrDefault();
            if (ret == null) {
                if (typeof(T).IsAssignableFrom(typeof(ScriptableObject))) {
                    ret = CreateInstance(typeof(T)) as T;
                }
                else {
                    ret = new T();
                }

                Connection.Insert(ret);
            }

            // 同步数据库结构
            Connection.CreateTable<T>();
            Connection.GetChildren(ret, true);
            return ret;
        }

        public void SaveDefault(int id = 1)
        {
            Id = id;
            Update();
        }

        //[Button]
        //[TitleGroup("Common/Database")]
        [ButtonGroup("Common")]
        public void Refresh()
        {
            GetType().GetProperties().ForEach(attr => {
                if (attr.CanWrite) {
                    attr.SetValue(this, attr.GetValue(this));
                }
            });
        }

        public void LoadWhere(Expression<Func<T, bool>> predExp, Action action = null)
        {
            var tmp = FirstOrDefault(predExp);
            var id = Id;
            LoadDefault(tmp);
            if (id != Id) {
                action?.Invoke();
                Update();
            }
        }

        //[Button]
        //[TitleGroup("Common/Database")]
        [ButtonGroup("Common")]
        public void SaveToFirst()
        {
            var tmp = FirstOrDefault();
            Id = tmp.Id;
            Update();
            Refresh();
        }

        [ButtonGroup("Common")]
        public void DropTable()
        {
            Connection.DropTable<T>();
            Connection.CreateTable<T>();
        }

        public void LoadDefault(T tIn)
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
    }
}